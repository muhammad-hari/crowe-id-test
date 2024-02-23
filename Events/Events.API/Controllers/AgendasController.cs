using Events.API.Helper;
using Events.Domain.Models;
using Events.Domain.Paginations;
using Events.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Events.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Authorize]
    public class AgendasController : ControllerBase
    {

        private readonly ILogger<AgendasController> logger;
        private readonly IAgendasService agendaService;
        private readonly IUriPagination uriPagination;

        public AgendasController(ILogger<AgendasController> logger, IAgendasService agendaService, IUriPagination uriPagination)
        {
            this.logger = logger;
            this.agendaService = agendaService;
            this.uriPagination = uriPagination; 
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PaginationFilter filter)
        {
            var route = Request.Path.Value;
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            var pagedData = await agendaService.GetAllAsync((validFilter.PageNumber - 1) * validFilter.PageSize, validFilter.PageSize);
            var totalRecords = await agendaService.GetCountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse(pagedData!.Data.ToList(), validFilter, totalRecords, uriPagination,  route);
            return Ok(pagedReponse);
        }


        [HttpPost, Route("arguments")]
        public IActionResult GetByArguments([FromQuery] PaginationFilter filter, [FromBody] AgendasModel param)
        {
            var route = Request.Path.Value;
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            var pagedData = agendaService.FindByArgument(o => o.Name == param.Name && !o.IsDeleted, (validFilter.PageNumber - 1) * validFilter.PageSize, validFilter.PageSize);
            var totalRecords = agendaService.GetCountAsync().Result;
            var pagedReponse = PaginationHelper.CreatePagedReponse(pagedData.Data.ToList(), validFilter, totalRecords, uriPagination, route);
            return Ok(pagedReponse);
        }

        [HttpGet, Route("{id}")]
        public IActionResult GetByID(Guid id)
        {
            var result = agendaService.FindByID(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AgendasModel item)
        {
            var result = await agendaService.CreateAsync(item);
            return Ok(result);
        }
      
        [HttpPut]
        public async Task<IActionResult> Update(AgendasModel item)
        {
            var result = await agendaService.UpdateAsync(item);
            return Ok(result);
        }

        [HttpDelete, Route("{id}")]
        public async Task<IActionResult> DeleteByID(Guid id)
        {
            var result = await agendaService.DeleteAsync(id);
            return Ok(result);
        }

        [HttpDelete, Route("BulkDelete")]
        public async Task<IActionResult> DeleteByID([FromBody] List<Guid> ids)
        {
            var result = await agendaService.DeleteBulkAsync(ids);
            return Ok(result);
        }

    }
}