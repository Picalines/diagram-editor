using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;

namespace DiagramEditor.Extensions;

public static class MaybeExtensions
{
    public static IActionResult MatchAction<T>(
        this Maybe<T> maybe,
        Func<T, IActionResult> someAction,
        Func<IActionResult> noneAction
    )
    {
        return maybe.Match(someAction, noneAction);
    }
}
