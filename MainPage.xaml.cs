using LightIntensityAnalyzer.Imaging;
using LightIntensityAnalyzer.PictureManagement;
using LightIntensityAnalyzer.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Media.Capture;
using Windows.Media.MediaProperties;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace LightIntensityAnalyzer
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            DO.Click += (s, e) => RunFlow();
            PreviewBtn.Click += (s, e) => SwitchPreview();
            PrepareCamera();

        }

        private async void PrepareCamera()
        {
            Capture = new MediaCapture();
            await Capture.InitializeAsync();
        }

        private async void SwitchPreview()
        {
            // Using Windows.Media.Capture.MediaCapture APIs 
            // to stream from webcam
            MediaCapture mediaCaptureMgr = new MediaCapture();
            await mediaCaptureMgr.InitializeAsync();

            // Start capture preview.                
            cameraFrame.Source = mediaCaptureMgr;
            await mediaCaptureMgr.StartPreviewAsync();
        }

        private async Task<SoftwareBitmap> TakePhotoFromCamera()
        {
            // Prepare and capture photo
            var properties = ImageEncodingProperties.CreateUncompressed(MediaPixelFormat.Bgra8);
            var lowLagCapture = await Capture.PrepareLowLagPhotoCaptureAsync(properties);

            var capturedPhoto = await lowLagCapture.CaptureAsync();
            var softwareBitmap = capturedPhoto.Frame.SoftwareBitmap;

            await lowLagCapture.FinishAsync();
            return softwareBitmap;
        }

        private async Task<SoftwareBitmap> TakePhoto(bool fromFile = false)
        {
            if(!fromFile)
                return await TakePhotoFromCamera();

            var picker = new PhotoPicker();
            var rawPicture = await picker.SelectPhoto();
            if (rawPicture == null)
                return null;
            return await Converter.DecodeToBitmap(rawPicture);
        }

        public async void RunFlow()
        {
            // 1) Pick a photo
            var photo = await TakePhoto();
            if (photo is null)
                return;

            // 2) Convert to appropriate type to process
            var convertedPicture = Converter.ConvertToGray8(photo);

            // 3) Find faces
            var faceManager = new FaceManager();
            var faces = await faceManager.DetectFacesAsync(convertedPicture, photo);
            if (!faces.Any())
                return;

            // 4) Compare back and front planes
            var (frontPixels, backPixels) = await PlaneComparator.GetFrontBackPixelsAsync(photo, faces);
            var contrastReport = PlaneComparator.CompareBackAndFrontAsync((frontPixels, backPixels));

            // 5) Are there strong light sources in background?
            var image = await ToUsableBitmapConverter.Convert(photo);
            var thresholdOverride = 255*3;
            var binaryImage = SourcesDetector.GetBinary(image, thresholdOverride);
            var backLightReport = SourcesDetector.AnalyzeBackground(binaryImage, frontPixels);

            // 6) Find lights in eyes

            // 7) Combine messages and push notification
            var messages = new List<Report> { contrastReport,
                                              backLightReport
                                              }; //"You are blinded! Switch off the lamp in front of you!"
            if (!messages.All(m => m.IsEmpty()))
                Notificator.Display(messages.Select(m => m.ToString()));
        }

        private MediaCapture Capture;
        private PictureConverter Converter = new PictureConverter();
    }
}
