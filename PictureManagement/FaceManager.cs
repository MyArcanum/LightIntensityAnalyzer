using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Media.FaceAnalysis;
using DlibDotNet;
using DlibDotNet.Extensions;
using LightIntensityAnalyzer.Imaging;

namespace LightIntensityAnalyzer.PictureManagement
{
    public sealed class FaceManager
    {
        public static async Task<IEnumerable<DetectedFace>> DetectFacesAsync(SoftwareBitmap convertedBitmap, SoftwareBitmap sourceBitmap)
        {
            var faceDetector = await FaceDetector.CreateAsync();

            return await faceDetector.DetectFacesAsync(convertedBitmap);
            //ShowDetectedFaces(sourceBitmap, detectedFaces);
        }

        public void GetEyePixels(Image img)
        {
            using (var fd = Dlib.GetFrontalFaceDetector())
            using (var sp = ShapePredictor.Deserialize("shape_predictor_68_face_landmarks.dat"))
            {
                //Dlib.LoadImageData(ImagePixelFormat.Bgr,  img.W, img.H, 3)
            }
        }
    }
}
