using Cashly.Application.CashflowContext.Interfaces.Repository;
using Cashly.Application.CashflowContext.UseCases.CreateCashflow;
using Cashly.Application.CashflowContext.UseCases.GetUserCashflows;
using Cashly.Application.IdentityContext.Interfaces.Repository;
using Cashly.Application.IdentityContext.Interfaces.Security;
using Cashly.Application.IdentityContext.UseCases.loginUser;
using Cashly.Application.IdentityContext.UseCases.RegisterUser;
using Cashly.Application.Shared.Abstractions;
using Cashly.Infrastructure.Authentication;
using Cashly.Infrastructure.Data.Context;
using Cashly.Infrastructure.Data.Repositories.CashflowContext;
using Cashly.Infrastructure.Data.Repositories.IdentityContext;
using Cashly.Infrastructure.Data.UnitOfWork;
using Cashly.Infrastructure.Security;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Cashly.Infrastructure.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddUnitOfWork(configuration);
        services.AddAuthenticationInfrastructure(configuration);
        services.AddIdentityContext();
        services.AddCashflowContext();
        
        return services;
    }

    private static IServiceCollection AddUnitOfWork(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        
        services.AddDbContext<CashlyDbContext>(options => options.UseSqlServer(connectionString));
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        
        return services;
    }

    private static IServiceCollection AddAuthenticationInfrastructure(this IServiceCollection services,
        IConfiguration configuration)
    {
        var jwtOptionsSection = configuration.GetSection("Jwt");
        
        services.Configure<JwtOptions>(jwtOptionsSection);
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

        return services;
    }

    private static IServiceCollection AddIdentityContext(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<RegisterUserHandler>();
        services.AddScoped<LoginUserHandler>();
        services.AddScoped<IValidator<RegisterUserCommand>, RegisterUserCommandValidator>();
        services.AddScoped<IValidator<LoginUserCommand>, LoginUserCommandValidator>();
        
        return services;
    }

    private static IServiceCollection AddCashflowContext(this IServiceCollection services)
    {
        services.AddCashflowWrite();
        services.AddCashflowRead();
        
        return services;
    }

    private static IServiceCollection AddCashflowWrite(this IServiceCollection services)
    {
        services.AddScoped<ICashflowRepository, CashflowRepository>();
        services.AddScoped<CreateCashflowHandler>();
        services.AddScoped<IValidator<CreateCashflowCommand>, CreateCashflowCommandValidator>();

        return services;
    }

    private static IServiceCollection AddCashflowRead(this IServiceCollection services)
    {
        services.AddScoped<ICashflowReadRepository, CashflowReadRepository>();
        services.AddScoped<GetUserCashflowHandler>();
        
        return services;
    }
    
}
