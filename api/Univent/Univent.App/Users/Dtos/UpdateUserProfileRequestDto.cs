using System.ComponentModel.DataAnnotations;
using Univent.Domain.Enums;

namespace Univent.App.Users.Dtos
{
    public class UpdateUserProfileRequestDto
    {
        [Required]
        [MinLength(3)]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(50)]
        public string LastName { get; set; }

        [Required]
        public DateTime Birthday { get; set; }

        public string? PictureUrl { get; set; }

        [Range(1, 8, ErrorMessage = "Invalid university year option.")]
        public UniversityYear Year { get; set; }

        [Required]
        public Guid UniversityId { get; set; }
    }
}
