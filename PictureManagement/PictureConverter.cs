using System;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Storage;

namespace LightIntensityAnalyzer.PictureManagement
{
    public sealed class PictureConverter
    {
        public async Task<SoftwareBitmap> DecodeToBitmap(StorageFile picture)
        {
            var fileStream = await picture.OpenAsync(FileAccessMode.Read);
            var decoder = await BitmapDecoder.CreateAsync(fileStream);

            var transform = new BitmapTransform();
            const float sourceImageHeightLimit = 1280f;

            if (decoder.PixelHeight > sourceImageHeightLimit)
            {
                float scalingFactor = (float)sourceImageHeightLimit / (float)decoder.PixelHeight;
                transform.ScaledWidth = (uint)Math.Floor(decoder.PixelWidth * scalingFactor);
                transform.ScaledHeight = (uint)Math.Floor(decoder.PixelHeight * scalingFactor);
            }

            return await decoder.GetSoftwareBitmapAsync(decoder.BitmapPixelFormat,
                                                        BitmapAlphaMode.Premultiplied,
                                                        transform,
                                                        ExifOrientationMode.IgnoreExifOrientation,
                                                        ColorManagementMode.DoNotColorManage);
        }

        public SoftwareBitmap ConvertToGray8(SoftwareBitmap sourceBitmap)
        {
            // Use FaceDetector.GetSupportedBitmapPixelFormats and IsBitmapPixelFormatSupported to dynamically
            // determine supported formats
            const BitmapPixelFormat faceDetectionPixelFormat = BitmapPixelFormat.Gray8;

            SoftwareBitmap convertedBitmap;

            if (sourceBitmap.BitmapPixelFormat != faceDetectionPixelFormat)
                convertedBitmap = SoftwareBitmap.Convert(sourceBitmap, faceDetectionPixelFormat);
            else
                convertedBitmap = sourceBitmap;
            return convertedBitmap;
        }
    }
}
