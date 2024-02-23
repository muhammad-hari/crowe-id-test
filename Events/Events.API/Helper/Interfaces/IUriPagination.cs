using Events.Domain.Paginations;

namespace Events.API.Helper
{
    public interface IUriPagination
    {
        public Uri GetPageUri(PaginationFilter filter, string route);
    }
}
