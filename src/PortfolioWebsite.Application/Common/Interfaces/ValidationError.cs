namespace PortfolioWebsite.Application.Common.Interfaces;

public sealed record ValidationError(string PropertyName, string ErrorMessage);
