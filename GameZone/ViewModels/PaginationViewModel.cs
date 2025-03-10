namespace GameZone.ViewModels
{
    public class PaginationViewModel
    {
        public IEnumerable<Game> Games { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; } = 1;
        public int PageSize { get; set; } = 3;
        public string SearchTerm { get; set; }
    }
}
