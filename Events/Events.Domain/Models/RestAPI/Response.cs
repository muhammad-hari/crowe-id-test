using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events.Domain.Models.RestAPI
{
	public class Response<T>
	{
		public Response() { }
		public Response(T data)
		{
			Succeeded = true;
			Message = string.Empty;
			Errors = null;
			Data = data;
		}
		public T? Data { get; set; }
		public bool Succeeded { get; set; } = false;
		public string[]? Errors { get; set; }
		public string? ErrorCode { get; set; }
		public string? Message { get; set; }
	}

}
