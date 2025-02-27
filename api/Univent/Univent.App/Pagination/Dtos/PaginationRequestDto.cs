using System.ComponentModel.DataAnnotations;

namespace Univent.App.Pagination.Dtos
{
    public class PaginationRequestDto
    {
        [Required]
        public int PageIndex { get; set; }
        public int PageSize { get; set; } = 10;
    }
}
