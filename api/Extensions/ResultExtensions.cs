using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;

namespace DiagramEditor.Extensions;

public static class ResultExtensions
{
    public static IActionResult MatchAction<T>(
        this Result<T> result,
        Func<T, IActionResult> successAction,
        Func<string, IActionResult> failureAction
    )
    {
        return result.Match(successAction, failureAction);
    }

    public static IActionResult MatchAction<T, E>(
        this Result<T, E> result,
        Func<T, IActionResult> successAction,
        Func<E, IActionResult> failureAction
    )
    {
        return result.Match(successAction, failureAction);
    }
}
