
namespace GameZone.Models
{
    public class Device : BaseEntity
    {
        
        [MaxLength(100)]
        [Required]
        public string Icon { get; set; }
        public ICollection<GameDevice> GameDevices { get; set; }
    }
}
