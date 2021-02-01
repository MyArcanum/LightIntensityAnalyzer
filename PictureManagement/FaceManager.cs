using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Media.FaceAnalysis;

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
    }
}
