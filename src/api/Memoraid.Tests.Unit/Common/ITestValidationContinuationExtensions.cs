using FluentValidation.TestHelper;

namespace Memoraid.Tests.Unit.Common
{
    internal static class ITestValidationContinuationExtensions
    {
        public static ITestValidationWith WithPropertyName(this ITestValidationContinuation failures, string propertyName)
        {
            return failures.When(failure => failure.PropertyName == propertyName);
        }
    }
}
