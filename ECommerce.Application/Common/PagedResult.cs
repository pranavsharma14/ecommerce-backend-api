using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanAPI.Application.Common
{
    public class PagedResult<T>
    {
        public IEnumerable<T> Data { get; set; }
        public int Page {  get; set; }
        public int PageSize {  get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
        public bool HasNextPage { get; set; }
        public bool HasPreviousPage { get; set; }

        public PagedResult(IEnumerable<T> data, int page, int pageSize, int totalCount)
        {
            Data = data;
            Page = page;
            PageSize = pageSize;
            TotalCount = totalCount;
            TotalPages = (int)Math.Ceiling(totalCount/(double)pageSize);
            HasNextPage = page < TotalPages;
            HasPreviousPage = page > 1;
        }
    }
}
