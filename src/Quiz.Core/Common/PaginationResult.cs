namespace Quiz.Core.Common;

public class PaginationResult<T>
{
    public List<T> Items { get; private set; }
    
    public int TotalCount { get; private set; }
    
    public int TotalPages { get; private set; }
    
    public int Page { get; private set; }
    
    public int PageSize { get; private set; }
    
    private PaginationResult(List<T> items, long totalCount, int page = 1, int pageSize = 10)
    {
        Items      = items;
        TotalCount = (int)totalCount;
        TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
        Page       = page;
        PageSize   = pageSize;
    }
    
    public static PaginationResult<T> Create(List<T> items, long totalCount, int page = 1, int pageSize = 10)
    {
        return new PaginationResult<T>(items, totalCount, page, pageSize);
    }
}