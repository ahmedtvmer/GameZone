using GameZone.Attributes;

namespace GameZone.ViewModels
{
    public class GameBaseViewModel
    {
        [MaxLength(250)]
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }
        [MaxLength(2500)]
        [Required(ErrorMessage = "Description is required.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Please select a category.")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a category.")]
        public int CategoryId { get; set; } = default!;
        public IEnumerable<SelectListItem>? Categories { get; set; }
        public IEnumerable<SelectListItem>? Devices { get; set; }
        [Required(ErrorMessage = "Please select at least one device.")]
        public IEnumerable<int> SelectedDevices { get; set; } = default!;
    }
}
