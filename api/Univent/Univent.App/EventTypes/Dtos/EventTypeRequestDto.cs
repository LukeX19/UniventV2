using System.ComponentModel.DataAnnotations;

namespace Univent.App.EventTypes.Dtos
{
    public class EventTypeRequestDto
    {
        [Required]
        public string Name { get; set; }
    }
}
