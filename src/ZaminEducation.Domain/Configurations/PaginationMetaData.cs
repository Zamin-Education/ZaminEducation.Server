namespace ZaminEducation.Domain.Configurations;

public class PaginationMetaData
{
    public PaginationMetaData(int totalCount, PaginationParmas @parmas)
    {
        TotalCount = totalCount;
        TotalPage = (int) Math.Ceiling(totalCount / (double) parmas.PageSize);
        CurrentPage = parmas.PageIndex;
    }
    public int CurrentPage { get; set; }
    public int TotalCount { get; set; }
    public int TotalPage { get; set; }
    public bool HasPrevious => CurrentPage > 1;
    public bool HasNext => CurrentPage < TotalCount;
}
