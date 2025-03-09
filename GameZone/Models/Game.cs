using System.ComponentModel.DataAnnotations;

namespace GameZone.Models
{
    public class Game : BaseEntity
    {
        [MaxLength(2500)]
        [Required]
        public string Description { get; set; }
        [MaxLength(500)]
        [Required]
        public string Cover { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public ICollection<GameDevice> GameDevices { get; set; }
    }
}
