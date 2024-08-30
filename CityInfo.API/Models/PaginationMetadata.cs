namespace CityInfo.API.Models
{
    public class PaginationMetadata
    {
        public int CurrentPage { get; private set; }
        public int PageSize { get; private set; }
        public int TotalPageCount { get; private set; }
        public int TotalItemCount { get; private set; }

        public PaginationMetadata(int totalItemCount, int pageSize, int pageNumber)
        {
            CurrentPage = pageNumber;
            TotalItemCount = totalItemCount;
            PageSize = pageSize;
            TotalPageCount = (int)Math.Ceiling(totalItemCount / (double)pageSize);
        }
    }
}
