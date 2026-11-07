using MediatR;
using PortfolioWebsite.Application.Common.Behaviours;
using PortfolioWebsite.Application.Common.Exceptions;
using PortfolioWebsite.Application.Common.Interfaces;

namespace PortfolioWebsite.Application.Tests.Behaviours;

public sealed class ValidationBehaviourTests
{
    [Fact]
    public async Task ValidationBehaviour_InvalidRequest_ThrowsAndDoesNotCallNext()
    {
        var behaviour = new ValidationBehaviour<TestRequest, int>(
            new IRequestValidator<TestRequest>[] { new FailingValidator() });
        var nextCalled = false;

        var exception = await Assert.ThrowsAsync<ValidationException>(() =>
            behaviour.Handle(
                new TestRequest(),
                _ =>
                {
                    nextCalled = true;
                    return Task.FromResult(0);
                },
                CancellationToken.None));

        Assert.False(nextCalled);
        Assert.Contains(
            exception.Errors.Values.SelectMany(messages => messages),
            message => message == "Invalid request.");
    }

    [Fact]
    public async Task ValidationBehaviour_ValidRequest_CallsNextAndReturnsResult()
    {
        var behaviour = new ValidationBehaviour<TestRequest, int>(
            Array.Empty<IRequestValidator<TestRequest>>());
        var nextCalled = false;

        var result = await behaviour.Handle(
            new TestRequest(),
            _ =>
            {
                nextCalled = true;
                return Task.FromResult(42);
            },
            CancellationToken.None);

        Assert.True(nextCalled);
        Assert.Equal(42, result);
    }

    private sealed record TestRequest : IRequest<int>;

    private sealed class FailingValidator : IRequestValidator<TestRequest>
    {
        public IEnumerable<ValidationError> Validate(TestRequest request)
        {
            return new[] { new ValidationError(nameof(TestRequest), "Invalid request.") };
        }
    }
}
