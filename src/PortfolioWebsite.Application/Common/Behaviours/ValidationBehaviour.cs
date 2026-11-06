using MediatR;
using PortfolioWebsite.Application.Common.Exceptions;
using PortfolioWebsite.Application.Common.Interfaces;

namespace PortfolioWebsite.Application.Common.Behaviours;

public sealed class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly IEnumerable<IRequestValidator<TRequest>> _validators;

    public ValidationBehaviour(IEnumerable<IRequestValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var errors = _validators
            .SelectMany(validator => validator.Validate(request))
            .ToArray();

        if (errors.Length > 0)
        {
            throw new ValidationException(errors);
        }

        return await next();
    }
}
