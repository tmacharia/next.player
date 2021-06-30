using Windows.Foundation;
using Windows.Storage;
using Windows.Storage.Pickers;

namespace Next.WRC
{
    public sealed class StorageFileExts
    {
        public static IAsyncOperation<StorageFile> PickFileAsync()
        {
            FileOpenPicker picker = new FileOpenPicker
            {
                ViewMode = PickerViewMode.Thumbnail,
                SuggestedStartLocation = PickerLocationId.ComputerFolder
            };
            picker.FileTypeFilter.Add(".mkv");
            return picker.PickSingleFileAsync();
        }
    }
}