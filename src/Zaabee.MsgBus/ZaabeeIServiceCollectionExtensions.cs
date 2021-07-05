using System;
using System.Data;
using Microsoft.Extensions.DependencyInjection;
using Zaabee.MsgBus.Abstractions;

namespace Zaabee.MsgBus
{
    public static class ZaabeeIServiceCollectionExtensions
    {
        public static IServiceCollection AddMsgBus(this IServiceCollection services,
            Action<ZaabeeMsgBusOptions> optionsAction = null)
        {
            var options = new ZaabeeMsgBusOptions();
            optionsAction?.Invoke(options);
            services.AddScoped<IZaabeeMsgBus>(p =>
                new ZaabeeMsgBus(options.Transaction ?? p.GetService<IDbTransaction>()));
            services.AddHostedService<ZaabeeMsgBusBackgroundService>();
            return services;
        }
    }
}