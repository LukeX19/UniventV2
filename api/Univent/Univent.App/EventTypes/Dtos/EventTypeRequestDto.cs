using System.ComponentModel.DataAnnotations;

namespace Univent.App.EventTypes.Dtos
{
    public class EventTypeRequestDto
    {
        [Required]
        [MinLength(3)]
        [MaxLength(50)]
        public string Name { get; set; }
    }
}
