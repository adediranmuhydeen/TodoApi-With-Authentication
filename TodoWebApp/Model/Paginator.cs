namespace TodoWebApp.Model
{
    public class Paginator
    {
        public int PerPage { get; set; }
        public int CurrentPage { get; set; }

        public Paginator()
        {
            PerPage = 5; CurrentPage = 1;
        }

        public Paginator(int perPage, int currentPage)
        {
            PerPage = perPage > 5 ? 5 : perPage;
            CurrentPage = currentPage < 1 ? 1 : currentPage;
        }
    }
}
