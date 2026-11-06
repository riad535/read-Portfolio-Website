using System.Globalization;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using PortfolioWebsite.Application.Common.Exceptions;

namespace PortfolioWebsite.Web.Areas.Admin.Models;

public static class AdminFormHelpers
{
    public static IReadOnlyList<string> SplitLines(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return Array.Empty<string>();
        }

        return value
            .Replace("\r\n", "\n")
            .Replace('\r', '\n')
            .Split('\n')
            .Select(line => line.Trim())
            .Where(line => line.Length > 0)
            .ToArray();
    }

    public static string? NormalizeOptional(string? value)
    {
        return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
    }

    public static bool TryParseMonth(string? value, out DateOnly month)
    {
        month = default;

        return !string.IsNullOrWhiteSpace(value) &&
               DateOnly.TryParseExact(
                   value.Trim(),
                   "yyyy-MM",
                   CultureInfo.InvariantCulture,
                   DateTimeStyles.None,
                   out month);
    }

    public static string FormatMonth(DateOnly value)
    {
        return value.ToString("yyyy-MM", CultureInfo.InvariantCulture);
    }

    public static void AddValidationErrors<TModel>(
        ModelStateDictionary modelState,
        ValidationException exception)
    {
        var fieldNames = typeof(TModel)
            .GetProperties()
            .Select(property => property.Name)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        foreach (var (key, messages) in exception.Errors)
        {
            var target = fieldNames.Contains(key) ? key : string.Empty;

            foreach (var message in messages)
            {
                modelState.AddModelError(target, message);
            }
        }
    }

    public static string GetFirstErrorMessage(ValidationException exception)
    {
        return exception.Errors.Values
            .SelectMany(messages => messages)
            .FirstOrDefault() ?? "The request was invalid.";
    }
}
