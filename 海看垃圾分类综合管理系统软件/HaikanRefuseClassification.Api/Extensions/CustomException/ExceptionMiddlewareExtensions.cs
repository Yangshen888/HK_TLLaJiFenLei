﻿using Microsoft.AspNetCore.Builder;

namespace HaikanRefuseClassification.Api.Extensions.CustomException
{
    public static class ExceptionMiddlewareExtensions
    {
        public static void ConfigureCustomExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
