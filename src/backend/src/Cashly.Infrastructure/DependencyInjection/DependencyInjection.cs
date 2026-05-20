using Cashly.Application.Abstractions.Messaging;
using Cashly.Application.Abstractions.Persistence;
using Cashly.Application.CashflowContext.Interfaces.Repository;
using Cashly.Application.CashflowContext.UseCases.CreateCashflow;
using Cashly.Application.CashflowContext.UseCases.GetCashflowBoard;
using Cashly.Application.CashflowContext.UseCases.GetUserCashflows;
using Cashly.Application.IdentityContext.Interfaces.Repository;
using Cashly.Application.IdentityContext.Interfaces.Security;
using Cashly.Application.IdentityContext.UseCases.LoginUser;
using Cashly.Application.IdentityContext.UseCases.RegisterUser;
using Cashly.Application.Shared.Results;
using Cashly.Infrastructure.Authentication;
using Cashly.Infrastructure.Data.Context;
using Cashly.Infrastructure.Data.Repositories.CashflowContext;
using Cashly.Infrastructure.Data.Repositories.IdentityContext;
using Cashly.Infrastructure.Data.UnitOfWork;
using Cashly.Infrastructure.Messaging;
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
        services.AddMediator();
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

    private static IServiceCollection AddMediator(this IServiceCollection services)
    {
        services.AddScoped<IMediator, Mediator>();
        
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
        services.AddScoped<ICommandHandler<RegisterUserCommand, Result<RegisterUserResponse>>, RegisterUserHandler>();
        services.AddScoped<ICommandHandler<LoginUserCommand, Result<LoginUserResponse>>, LoginUserHandler>();
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
        services.AddScoped<ICommandHandler<CreateCashflowCommand, Result<CreateCashflowResponse>>, CreateCashflowHandler>();
        services.AddScoped<IValidator<CreateCashflowCommand>, CreateCashflowCommandValidator>();

        return services;
    }

    private static IServiceCollection AddCashflowRead(this IServiceCollection services)
    {
        services.AddScoped<ICashflowReadRepository, CashflowReadRepository>();
        services.AddScoped<ICashflowMemberReadRepository, CashflowMemberReadRepository>();
        
        services.AddScoped<IQueryHandler<GetUserCashflowsQuery,
            Result<GetUserCashflowsResponse>>, GetUserCashflowHandler>();

        services.AddScoped<IQueryHandler<GetCashflowBoardQuery,
            Result<GetCashflowBoardResponse>>, GetCashflowBoardHandler>();
        
        return services;
    }
    
}
