using JG.Flix.Catalog.Application.Common;

namespace Jg.Flix.Catalog.Api.ApiModels.Response;

public class ApiResponseList<TItemData> : ApiResponse<IReadOnlyList<TItemData>>
{
    public ApiResponseListMeta Meta { get; private set; }

    public ApiResponseList(int currentPage, int perPage, int total, IReadOnlyList<TItemData> data) : base(data)
    {
        Meta = new(currentPage, perPage, total);
    }

    public ApiResponseList(PaginatedListOutput<TItemData> paginatedListOutput) : base(paginatedListOutput.Items)
    {
        Meta = new(paginatedListOutput.Page, paginatedListOutput.PerPage, paginatedListOutput.Total);
    }
}
