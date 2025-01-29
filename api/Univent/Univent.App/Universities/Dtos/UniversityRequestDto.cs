using System.ComponentModel.DataAnnotations;

namespace Univent.App.Universities.Dtos
{
    public class UniversityRequestDto
    {
        [Required]
        public string Name { get; set; }
    }
}
