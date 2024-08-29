using System;
using Microsoft.Extensions.DependencyInjection;

namespace SmtpServer
{
  public static class SmtpServerExtensions
  {
    /// <summary>
    /// Configure the SmtpServer and add the SmtpService as a hosted service
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configure"></param>
    /// <returns></returns>
    public static IServiceCollection AddSmtpServer(this IServiceCollection services, Action<SmtpServerOptionsBuilder> configure)
    {
      var builder = new SmtpServerOptionsBuilder();
      configure(builder);
      var options = builder.Build();

      services.AddSingleton<ISmtpServerOptions>(options);

      services.AddHostedService<SmtpServerBackgroundService>();

      return services;
    }
  }

}
