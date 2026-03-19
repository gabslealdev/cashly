using Cashly.Application.Identity.Interfaces.Repository;
using Cashly.Application.Identity.Interfaces.Security;
using Cashly.Application.Identity.UseCases.RegisterUser;
using Cashly.Application.Shared.Abstractions;
using Cashly.Infrastructure.Data.Context;
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

        services.AddDbContext<CashlyDbContext>(options => options.UseSqlServer(connectionString));

        
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<RegisterUserHandler>();
        services.AddScoped<IValidator<RegisterUserCommand>, RegisterUserCommandValidator>();

        return services;
    }
}
