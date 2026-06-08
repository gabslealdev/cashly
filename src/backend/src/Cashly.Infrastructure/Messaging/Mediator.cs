using Cashly.Application.Abstractions.Messaging;
using Cashly.Application.Shared.Results;
using Cashly.Domain.Shared.Exceptions;
using Microsoft.Extensions.DependencyInjection;

namespace Cashly.Infrastructure.Messaging;

public sealed class Mediator : IMediator
{
    private readonly IServiceProvider _serviceProvider;

    public Mediator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    public async Task<TResponse> SendAsync<TResponse>(ICommand<TResponse> command, CancellationToken cancellationToken = default)
    {
        var commandType = command.GetType();
        
        var handlerType = typeof(ICommandHandler<,>).MakeGenericType(commandType, typeof(TResponse));

        var handler = _serviceProvider.GetRequiredService(handlerType);
        
        var method = handlerType.GetMethod("HandleAsync");
        if (method is null)
            throw new InvalidOperationException($"No handler found for {commandType}");
        
        if(method.Invoke(handler, [command]) is not Task<TResponse> task)
            throw new InvalidOperationException($"No task found for {commandType}");

        return await ExecuteAsync(task, cancellationToken);
    }

    public async Task<TResponse> SendAsync<TResponse>(IQuery<TResponse> query, CancellationToken cancellationToken = default)
    {
        var queryType = query.GetType();

        var handleType = typeof(IQueryHandler<,>).MakeGenericType(queryType, typeof(TResponse));
        
        var handler = _serviceProvider.GetRequiredService(handleType);

        var method = handleType.GetMethod("HandleAsync");
        if (method is null)
            throw new InvalidOperationException($"No task handler for {queryType}");

        if (method.Invoke(handler, [query]) is not Task<TResponse> task)
            throw new InvalidOperationException($"No task found for {queryType}");
        
        return await ExecuteAsync(task, cancellationToken);
    }

    private static async Task<TResponse> ExecuteAsync<TResponse>(
        Task<TResponse> task,
        CancellationToken cancellationToken)
    {
        try
        {
            return await task.WaitAsync(cancellationToken);
        }
        catch (DomainException exception)
        {
            return CreateFailureResponse<TResponse>(exception);
        }
    }

    private static TResponse CreateFailureResponse<TResponse>(DomainException exception)
    {
        var responseType = typeof(TResponse);
        var applicationError = ApplicationError.FromDomain(exception.Error);

        if (responseType == typeof(Result))
            return (TResponse)(object)Result.Failure(applicationError);

        if (!responseType.IsGenericType ||
            responseType.GetGenericTypeDefinition() != typeof(Result<>)) throw exception;
        
        var failureMethod = responseType.GetMethod(
            nameof(Result<object>.Failure),
            [typeof(ApplicationError)]);

        if (failureMethod is null)
            throw new InvalidOperationException($"Failure method not found for {responseType.Name}.");

        return (TResponse)failureMethod.Invoke(null, [applicationError])!;

    }
}
