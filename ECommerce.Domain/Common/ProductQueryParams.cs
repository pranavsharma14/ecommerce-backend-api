using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanAPI.Application.Common
{
    public class ProductQueryParams
    {
        //Pagination
        private int _pageSize = 10;
        private int _page = 1;
        private int MaxPageSize = 50;

        public int Page
        {
            get => _page;
            set => _page = value <= 0 ? 1 : value;
        }
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value <= 0 ? 10 : value > MaxPageSize ? MaxPageSize : value;
        }

        //Search and Filter
        public string? Search {  get; set; }
        public int? CategoryId { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }

        //Sorting
        public string? SortBy { get; set; } = "id";
        public string? SortOrder { get; set; } = "asc";
    }
}
