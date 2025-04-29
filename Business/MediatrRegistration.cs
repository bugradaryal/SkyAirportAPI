using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Utilitys.ExceptionHandler;
using Business.Features.Generic.Commands.Add;
using Business.Features.Generic.Commands.Delete;
using Business.Features.Generic.Commands.Update;
using Business.Features.Generic.Queries.GetAll;
using Business.Features.Generic.Queries.GetById;
using Entities;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Business
{
    public static class MediatrRegistration
    {
        public static void AddMediatRApplication(this IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();

            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));

            var entityTypes = new[] { 
                typeof(Aircraft), 
                typeof(AircraftStatus), 
                typeof(Airport),
                typeof(Crew),
                typeof(Flight),
                typeof(OperationalDelay),
                typeof(Personal),
                typeof(Seat),
                typeof(Ticket)
            };

            foreach(var entity in entityTypes)
{
                // Add
                services.AddTransient(
                    typeof(IRequestHandler<,>).MakeGenericType(
                        typeof(GenericAddRequest<>).MakeGenericType(entity),
                        typeof(CustomException)
                    ),
                    typeof(GenericAddHandle<>).MakeGenericType(entity)
                );

                // Delete
                services.AddTransient(
                    typeof(IRequestHandler<,>).MakeGenericType(
                        typeof(GenericDeleteRequest<>).MakeGenericType(entity),
                        typeof(CustomException)
                    ),
                    typeof(GenericDeleteHandle<>).MakeGenericType(entity)
                );

                // Update
                services.AddTransient(
                    typeof(IRequestHandler<,>).MakeGenericType(
                        typeof(GenericUpdateRequest<>).MakeGenericType(entity),
                        typeof(CustomException)
                    ),
                    typeof(GenericUpdateHandle<>).MakeGenericType(entity)
                );

                // GetAll
                services.AddTransient(
                    typeof(IRequestHandler<,>).MakeGenericType(
                        typeof(GenericGetAllRequest<>).MakeGenericType(entity),
                        typeof(GenericGetAllResponse<>).MakeGenericType(entity)
                    ),
                    typeof(GenericGetAllHandler<>).MakeGenericType(entity)
                );

                // GetById
                services.AddTransient(
                    typeof(IRequestHandler<,>).MakeGenericType(
                        typeof(GenericGetByIdRequest<>).MakeGenericType(entity),
                        typeof(GenericGetByIdResponse<>).MakeGenericType(entity)
                    ),
                    typeof(GenericGetByIdHandler<>).MakeGenericType(entity)
                );
            }
        }
    }
}
