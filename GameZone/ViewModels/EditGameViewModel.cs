using GameZone.Attributes;

namespace GameZone.ViewModels
{
    public class EditGameViewModel : GameBaseViewModel
    {
        [AllowedExtensions(FileSettings.AllowedExtensions)]
        [MaxFileSize(FileSettings.MaxFileSizeInMB)]
        public IFormFile? Cover { get; set; } = default!;
        public string? CurrentCover { get; set; }
    }
}
