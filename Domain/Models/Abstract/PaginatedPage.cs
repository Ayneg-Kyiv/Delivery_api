namespace Domain.Models.Abstract
{
    public class PaginatedPage
    {
        public Pagination Pagination { get; set; } = new Pagination();
        public object? Data { get; set; }
    }
}
