using Cashly.Application.CashflowContext.Interfaces.Repository;
using Cashly.Application.Identity.Interfaces.Repository;
using Cashly.Application.Identity.Interfaces.Security;
using Cashly.Application.Identity.UseCases.loginUser;
using Cashly.Application.Identity.UseCases.RegisterUser;
using Cashly.Application.Shared.Abstractions;
using Cashly.Infrastructure.Authentication;
using Cashly.Infrastructure.Data.Context;
using Cashly.Infrastructure.Data.Repositories.CashflowContext;
using Cashly.Infrastructure.Data.Repositories.Identity;
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
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        var jwtOptionsSection = configuration.GetSection("Jwt");

        services.AddDbContext<CashlyDbContext>(options => options.UseSqlServer(connectionString));
        services.Configure<JwtOptions>(jwtOptionsSection);


        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<RegisterUserHandler>();
        services.AddScoped<LoginUserHandler>();
        services.AddScoped<IValidator<RegisterUserCommand>, RegisterUserCommandValidator>();
        services.AddScoped<IValidator<LoginUserCommand>, LoginUserCommandValidator>();
        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddScoped<ICashflowRepository, CashflowRepository>();

        return services;
    }
}
