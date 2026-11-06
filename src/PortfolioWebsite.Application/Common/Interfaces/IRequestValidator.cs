namespace PortfolioWebsite.Application.Common.Interfaces;

public interface IRequestValidator<in TRequest>
{
    IEnumerable<ValidationError> Validate(TRequest request);
}
