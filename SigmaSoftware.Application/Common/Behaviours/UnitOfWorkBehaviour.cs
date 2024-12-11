using System.Net;
using System.Transactions;
using MediatR;
using Microsoft.Extensions.Logging;
using SigmaSoftware.Application.Common.Exceptions;
using SigmaSoftware.Application.Common.Interfaces;
using SigmaSoftware.Domain.Common.Model;

namespace SigmaSoftware.Application.Common.Behaviours;

public sealed class UnitOfWorkBehavior<TRequest, TResponse>(
    IUnitOfWork unitOfWork,
    ILogger<UnitOfWorkBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (!typeof(TRequest).Name.EndsWith("Command"))
        {
            return await next();
        }

        using var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
        var response =  await next();
        if (response is Response { IsSuccess: false } res )
        {
            logger.LogInformation("UnitOfWork request Fails for: {@RequestName}", typeof(TRequest).Name);
            if (!res.Error!.CommitTransaction) // Check if transaction needs to be commited for the given failure case.
                return response;
        }
        try
        {
            await unitOfWork.CommitAsync(cancellationToken);
            transactionScope.Complete();
            logger.LogInformation("UnitOfWork request Success for: {@RequestName}",typeof(TRequest).Name);
            return response;
        }
        catch (Exception ex)
        {
            logger.LogInformation("UnitOfWork request Fails for: {@RequestName}",typeof(TRequest).Name);
            throw new UnitOfWorkExceptions($"{ex.Message} : {ex.InnerException?.Message}", HttpStatusCode.InternalServerError);
        }
    }
}