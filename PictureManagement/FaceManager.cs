using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Media.FaceAnalysis;
using DlibDotNet;
using DlibDotNet.Extensions;

namespace LightIntensityAnalyzer.PictureManagement
{
    public sealed class FaceManager
    {
        public async Task<IEnumerable<DetectedFace>> DetectFacesAsync(SoftwareBitmap convertedBitmap, SoftwareBitmap sourceBitmap)
        {
            var faceDetector = await FaceDetector.CreateAsync();

            return await faceDetector.DetectFacesAsync(convertedBitmap);
            //ShowDetectedFaces(sourceBitmap, detectedFaces);
        }

        public void GetEyePixels()
        {
            using (var fd = Dlib.GetFrontalFaceDetector())
            using (var sp = ShapePredictor.Deserialize("shape_predictor_68_face_landmarks.dat"))
            {

            }
        }
    }
}
