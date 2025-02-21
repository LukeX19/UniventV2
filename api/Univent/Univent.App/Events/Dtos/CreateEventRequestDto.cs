using System.ComponentModel.DataAnnotations;
using Univent.App.ValidationAttributes;

namespace Univent.App.Events.Dtos
{
    public class CreateEventRequestDto
    {
        [Required]
        [MinLength(3)]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(3000)]
        public string Description { get; set; }

        [Range(1, int.MaxValue)]
        public int MaximumParticipants { get; set; }

        [Required]
        [FutureDate(ErrorMessage = "Start time must be in the future.")]
        public DateTime StartTime { get; set; }

        [Required]
        public string LocationAddress { get; set; }

        [Required]
        public double LocationLat { get; set; }

        [Required]
        public double LocationLong { get; set; }

        [Required]
        public string PictureUrl { get; set; }

        [Required]
        public Guid TypeId { get; set; }
    }
}
