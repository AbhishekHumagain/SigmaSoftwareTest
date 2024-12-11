using MediatR;
using SigmaSoftware.Domain.Common.Model;

namespace SigmaSoftware.Application.Common.Messaging
{
    public interface ICommand : IRequest<Response>, IBaseCommand
    {
    }

    public interface ICommand<TResponse> : IRequest<Response<TResponse>> , IBaseCommand
    {

    }

    public interface IBaseCommand
    {

    }
}
