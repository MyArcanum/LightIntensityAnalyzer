using System;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;

namespace LightIntensityAnalyzer.PictureManagement
{
    public class PhotoPicker
    {
        public async Task<StorageFile> SelectPhoto()
        {
            var photoPicker = new FileOpenPicker();
            photoPicker.ViewMode = PickerViewMode.Thumbnail;
            photoPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            photoPicker.FileTypeFilter.Add(".jpg");
            photoPicker.FileTypeFilter.Add(".jpeg");
            photoPicker.FileTypeFilter.Add(".png");
            photoPicker.FileTypeFilter.Add(".bmp");

            var photoFile = await photoPicker.PickSingleFileAsync();
            return photoFile;
        }
    }
}
