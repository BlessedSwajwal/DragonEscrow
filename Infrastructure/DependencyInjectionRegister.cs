using Application.Common.Algo;
using Application.Common.Services;
using Infrastructure.Algo;
using Infrastructure.Authentication;
using Infrastructure.Persistence.EntityFrameWork;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Stripe;
using System.Text;

namespace Infrastructure;

public static class DependencyInjectionRegister
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IWeightedScoringAlgo, WeightedScoringAlgo>();
        services.AddSingleton<IJwtGenerator, JwtGenerator>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddDbContext<DragonEscrowDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DragonEscrowDb")));

        AddAuth(services, configuration);
        AddHttpClients(services, configuration);

        services.AddSingleton<IConfiguration>(configuration);

        //Email
        services.Configure<EmailSettings>(configuration.GetSection(EmailSettings.SectionName));
        services.AddSingleton<IEmailSenderService, EmailSenderService>();

        //Stripe
        string? stripe_secret = configuration.GetValue<string>("StripeSecret");
        if (stripe_secret is null)
        {
            throw new Exception("No key for stripe.");
        }
        StripeConfiguration.ApiKey = stripe_secret;

        return services;
    }

    public static void AddHttpClients(IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClient<IPaymentService, PaymentService>(op =>
        {
            var secret = configuration.GetValue<string>("KhaltiSecret");
            op.BaseAddress = new Uri("https://a.khalti.com/api/v2/epayment");
            op.DefaultRequestHeaders.Add("Authorization", secret);
        });
    }

    public static void AddAuth(IServiceCollection services, IConfiguration configuration)
    {
        var jwtSettings = new JwtSettings();
        configuration.GetSection(JwtSettings.SectionName).Bind(jwtSettings);
        services.AddSingleton(Options.Create<JwtSettings>(jwtSettings));

        //Add Authentication
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings.Issuer,
                ValidAudience = jwtSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey))
            });
    }
}
