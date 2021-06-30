using Windows.Foundation;
using Windows.Storage;
using Windows.Storage.Pickers;

namespace Next.WRC
{
    public sealed class StorageFolderExts
    {
        public static IAsyncOperation<StorageFolder> PickFolderAsync()
        {
            FolderPicker picker = new FolderPicker
            {
                ViewMode = PickerViewMode.Thumbnail,
                SuggestedStartLocation = PickerLocationId.ComputerFolder
            };
            return picker.PickSingleFolderAsync();
        }
    }
}