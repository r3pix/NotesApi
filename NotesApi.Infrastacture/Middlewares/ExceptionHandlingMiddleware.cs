using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using NotesApi.Infrastacture.Models;
using NotesApi.Infrastacture.Exceptions;

namespace NotesApi.Infrastacture.Middlewares
{
    public class ExceptionHandlingMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (NotFoundException ex)
            {
                await HandleNotFoundError(context, ex);
            }
            catch (Exception ex)
            {
                await HandleError(context, ex);
            }
        }

        private async Task HandleError(HttpContext context, Exception ex)
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            await context.Response.WriteAsync(JsonSerializer.Serialize(new Response<string>()
            {
                IsError = true,
                Code = HttpStatusCode.InternalServerError,
                Message = ex.StackTrace,
                Result = ex.Message
            }));
        }

        private async Task HandleNotFoundError(HttpContext context, NotFoundException ex)
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            await context.Response.WriteAsync(JsonSerializer.Serialize(new Response<string>()
            {
                IsError = true,
                Code = HttpStatusCode.NotFound,
                Message = ex.StackTrace,
                Result = ex.Message
            }));
        }
    }
}
