namespace LightCap.InvestmentApi.Application.Common.Utilities.Filters;

public class PaginationFilter(int pageNumber = 1, int pageSize = 10)
{
    public int PageNumber => (pageNumber < 1) ? 1 : pageNumber;
    public int PageSize => (pageSize is > 1000 or < 1) ? 10 : pageSize;
}

public record PaginatorVm<T>(
    int PageSize,
    int PageNumber,
    int TotalPages,
    int TotalCount,
    T? PageItems
    );