using LightIntensityAnalyzer.Imaging;
using LightIntensityAnalyzer.PictureManagement;
using System;
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

        private async Task<SoftwareBitmap> TakePhoto()
        {
            // Prepare and capture photo
            var properties = ImageEncodingProperties.CreateUncompressed(MediaPixelFormat.Bgra8);
            var lowLagCapture = await Capture.PrepareLowLagPhotoCaptureAsync(properties);

            var capturedPhoto = await lowLagCapture.CaptureAsync();
            var softwareBitmap = capturedPhoto.Frame.SoftwareBitmap;

            await lowLagCapture.FinishAsync();
            return softwareBitmap;
        }

        public async void RunFlow()
        {
            // 1) Pick a photo (temporarily - after upgrade it will take frames from video)
            //var picker = new PhotoPicker();
            //var rawPicture = await picker.SelectPhoto();
            //if (rawPicture == null)
            //    return;
            // 2) Convert to appropriate type to process
            var converter = new PictureConverter();
            // Decoder is only one that can access pixels
            //var decodedPicture = await converter.DecodeToBitmap(rawPicture);

            var photo = await TakePhoto();
            var convertedPicture = converter.ConvertToGray8(photo);//decodedPicture);
            // 3) Find faces
            var faceManager = new FaceManager();
            var faces = await faceManager.DetectFacesAsync(convertedPicture, photo);//decodedPicture);
            if (!faces.Any())
                return;
            // 4) Compare back and front planes
            var comparator = new PlaneComparator();

            var (frontPixels, backPixels) = await PlaneComparator.GetFrontBackPixelsAsync(photo, faces);
            var comparisonReport = await comparator.CompareBackAndFrontAsync(photo, faces, (frontPixels, backPixels));
            // 5) Are there strong light sources in background?
            var sourcesDetector = new SourcesDetector();
            var image = await ToUsableBitmapConverter.Convert(photo);
            var (grayImg, binaryImg) = sourcesDetector.Detect(image);
            //sourcesDetector.FindCenterFront
        }

        private MediaCapture Capture;
    }
}
