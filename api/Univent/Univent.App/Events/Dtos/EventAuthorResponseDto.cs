namespace Univent.App.Events.Dtos
{
    public class EventAuthorResponseDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? PictureUrl { get; set; }
        public double Rating { get; set; }
    }
}
