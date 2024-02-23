using AutoMapper;
using CsvHelper;
using CsvHelper.Configuration;
using Events.Data.Context;
using Events.Data.Entities;
using Events.Domain.Models;
using Events.Domain.Models.RestAPI;
using Events.Domain.Validators;
using Events.Services.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace Events.Services
{
    public class AgendasService : IAgendasService
    {
        private readonly EventsDbContext dbContext;
        private readonly IMapper mapper;

        public AgendasService(EventsDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;   
        }

        public async Task<Response<AgendasModel>> CreateAsync(AgendasModel agenda)
        {
            var response = new Response<AgendasModel>();
            var agendasValidator = new AgendasValidator();
            var data = new AgendasModel();

            try
            {
                var validator = agendasValidator.Validate(agenda);
                if (!validator.IsValid)
                {
                    response = new Response<AgendasModel>()
                    {
                        Succeeded = false,
                        Message = $"{validator.Errors}",
                        Errors = validator.Errors.Select(error => $"[{error.PropertyName}]: {error.ErrorMessage}, {error.ErrorCode}.").ToArray(),
                        Data = data
                    };

                    return response;
                }

                agenda.Id = Guid.NewGuid();


                dbContext.Add(mapper.Map<AgendasModel, Agenda>(agenda));
                await dbContext.SaveChangesAsync();

                response.Data = agenda;
                response.Message = "Data created successfully";
                response.Succeeded = true;

            }
            catch(Exception ex)
            {
                response.Message = "Failed to create data!";
                response?.Errors?.Append(ex.Message.ToString());
            }

            return response!;
        }

        public async Task<Response<AgendasModel>> UpdateAsync(AgendasModel agenda)
        {
            var response = new Response<AgendasModel>();
            var agendasValidator = new AgendasValidator();
            var data = new AgendasModel();

            try
            {
                var validator = agendasValidator.Validate(agenda);
                if (!validator.IsValid)
                {
                    response = new Response<AgendasModel>()
                    {
                        Succeeded = false,
                        Message = $"{validator.Errors}",
                        Errors = validator.Errors.Select(error => $"[{error.PropertyName}]: {error.ErrorMessage}, {error.ErrorCode}.").ToArray(),
                        Data = data
                    };

                    return response;
                }

                var findAgenda = dbContext.Agendas.Where(c => c.Id == agenda.Id).AsNoTracking().FirstOrDefault();
                if (findAgenda == null)
                {
                    response = new Response<AgendasModel>()
                    {
                        Succeeded = false,
                        Message = $"Data not found",
                        Data = data
                    };

                    return response;
                }

                dbContext.Update(agenda);
                await dbContext.SaveChangesAsync();

                response.Data = agenda;
                response.Message = "Data updated successfully";
                response.Succeeded = true;
            }
            catch (Exception ex)
            {
                response.Message = "Failed to create data!";
                response?.Errors?.Append(ex.Message.ToString());
            }

            return response!;
        }

        public async Task<Response<List<Guid>>> DeleteBulkAsync(List<Guid> ids)
        {
            var response = new Response<List<Guid>>();
            var data = new List<Guid>();

            try
            {
                var agendas = dbContext.Agendas.Where(x => ids.Contains(x.Id)).ToList();
                if (agendas.Count == 0)
                {
                    response = new Response<List<Guid>>()
                    {
                        Succeeded = true,
                        Message = $"Data not found",
                        Data = ids
                    };

                    return response;
                }

                foreach (var agenda in agendas){ agenda.IsDeleted = false; }

                await dbContext.Agendas.BulkUpdateAsync(agendas);
                dbContext.SaveChanges();

                response.Data = ids;
                response.Message = "All data deleted successfully";
                response.Succeeded = true;
            }
            catch (Exception ex)
            {
                response.Message = "Failed to delete the data!";
                response?.Errors?.Append(ex.Message.ToString());
            }

            return response!;
        }

        public async Task<Response<AgendasModel>> DeleteAsync(Guid id)
        {
            var response = new Response<AgendasModel>();
            var data = new AgendasModel();

            try
            {
                var agenda = dbContext.Agendas.Where(c => c.Id == id).FirstOrDefault();
                if (agenda == null)
                {
                    response = new Response<AgendasModel>()
                    {
                        Succeeded = true,
                        Message = $"Data not found",
                        Data = data 
                    };

                    return response;
                }

                agenda.IsDeleted = true;
                dbContext.Agendas.Update(agenda);
                await dbContext.SaveChangesAsync();

                response.Data = mapper.Map<AgendasModel>(agenda);
                response.Message = "Data deleted successfully";
                response.Succeeded = true;
            }
            catch (Exception ex)
            {
                response.Message = "Failed to delete the data!";
                response?.Errors?.Append(ex.Message.ToString());
            }

            return response!;
        }

        public Response<IEnumerable<AgendasModel>> FindByArgument(Func<AgendasModel, bool> predicate, int start = 0, int end = 10)
        {
            var response = new Response<IEnumerable<AgendasModel>>();
            var data = new List<AgendasModel>();

            try
            {
                Func<Agenda, bool> agendaFunc = agenda => predicate.Invoke(new AgendasModel
                {
                    Id = agenda.Id,
                    Name = agenda.Name,
                    StartDate = agenda.StartDate,
                    EndDate = agenda.EndDate,
                    IsDeleted = agenda.IsDeleted,
                });

                var result = dbContext.Agendas.Where(agendaFunc).Skip(start).Take(end).ToList();

                if(!result.Any())
                {
                    response.Message = "Data not found!";
                    response.Succeeded = true;
                    response.Data = data;

                    return response;
                }

                data = mapper.Map<List<AgendasModel>>(result);

                response.Data = data;
                response.Message = "Data found!";
                response.Succeeded = true;
            }
            catch(Exception ex)
            {
                response.Message = "Failed to fetch data!";
                response?.Errors?.Append(ex.Message.ToString());
            }

            return response!;
        }

        public Response<AgendasModel> FindByID(Guid id)
        {
            var response = new Response<AgendasModel>();
            var data = new AgendasModel();

            try
            {
                var result = dbContext.Agendas.Where(x => !x.IsDeleted && x.Id == id).AsNoTracking().FirstOrDefault();

                if (result == null)
                {
                    response.Message = "Data not found!";
                    response.Succeeded = true;
                    response.Data = data;

                    return response;
                }

                data = mapper.Map<AgendasModel>(result);
                response.Data = data;
                response.Message = "Data found!";
                response.Succeeded = true;
            }
            catch (Exception ex)
            {
                response.Message = "Failed to fetch data!";
                response?.Errors?.Append(ex.Message.ToString());
            }

            return response!;
        }

        public async Task<Response<IEnumerable<AgendasModel>>> GetAllAsync(int start = 0, int end = 10)
        {
            var response = new Response<IEnumerable<AgendasModel>>();
            var data = new List<AgendasModel>();

            try
            {
                var result = await dbContext.Agendas.Where(x => !x.IsDeleted).Skip(start).Take(end).ToListAsync();
                var totalItem = await dbContext.Agendas.Where(x => !x.IsDeleted).CountAsync();

                if (totalItem == 0)
                {
                    response.Message = "Data not found!";
                    response.Succeeded = true;
                    response.Data = data;

                    return response;
                }


                data = mapper.Map<List<AgendasModel>>(result);

                response.Data = data;
                response.Message = "Data found!";
                response.Succeeded = true;

            }
            catch (Exception ex)
            {
                response.Message = "Failed to fetch data!";
                response?.Errors?.Append(ex.Message.ToString());
            }

            return response!;
        }

        public async Task<int> GetCountAsync() => await dbContext.Agendas.CountAsync(x => !x.IsDeleted);
    }

}
