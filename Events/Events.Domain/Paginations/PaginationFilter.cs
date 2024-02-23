using System;
using System.Collections.Generic;
using System.Linq;

namespace Events.Domain.Paginations
{
	public class PaginationFilter 
	{
		public PaginationFilter()
		{
			this.PageNumber = 1;
			this.PageSize = 10;
		}
		public PaginationFilter(int pageNumber, int pageSize)
		{
			this.PageNumber = pageNumber < 1 ? 1 : pageNumber;
			this.PageSize = pageSize > 10 ? 10 : pageSize;
		}
		public int PageNumber { get; set; } = 1;
		public int PageSize { get; set; } = 10;
	}
}
