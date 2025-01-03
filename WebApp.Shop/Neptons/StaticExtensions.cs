﻿using DataLayer.Infrastructure;
using DataLayer.Infrastructure.ViewModels;
using DataLayer.Infrastructure.ViewModels.Form;
using DataLayer.Infrastructure.WebModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Primitives;
using System.Text;
using static NuGet.Packaging.PackagingConstants;

namespace WebApp.Shop.Neptons
{
    public static class StaticExtensions
    {
        public static IApplicationBuilder UseReactCommunication(this IApplicationBuilder app)
        {
            return app.UseMiddleware<UrlMiddleware>();
        }

        public static IApplicationBuilder UseGlobalMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<GlobalMiddleware>();
        }

        public static CentralizeData CentralizeDataFiller(this Controller controller)
        {
            return new CentralizeData(controller.HttpContext, controller.ModelState);
        }


    }
}
