using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;

namespace DiagramEditor.Web.API.Extensions;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

internal static class MaybeExtensions
{
    public static ActionResult<T> ToActionResult<T>(
        this Maybe<T> maybe,
        Func<T, ActionResult<T>> someAction,
        Func<ActionResult<T>> noneAction
    )
    {
        return maybe.Match(someAction, noneAction);
    }

    public static Results<RS, RN> ToTypedResult<T, RS, RN>(
        this Maybe<T> maybe,
        Func<T, RS> someResult,
        Func<RN> noneResult
    )
        where RS : IResult
        where RN : IResult
    {
        return maybe.TryGetValue(out var value) ? someResult(value) : noneResult();
    }
}
