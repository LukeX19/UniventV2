using Univent.Domain.Enums;

namespace Univent.App.Users.Dtos
{
    public class UserBasicInfoResponseDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? PictureUrl { get; set; }
        public AppRole Role { get; set; }
        public bool IsAccountConfirmed { get; set; }
    }
}
