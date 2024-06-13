using System.Diagnostics;
using Microsoft.JSInterop;

namespace JsonEditor;

/// <summary>
/// <see cref="JSRuntime"/> extension methods.
/// </summary>
public static class JsRuntimeExtension
{
    /// <summary>
    /// Tries to call the specified JavaScript function repeatedly until success or timeout.
    /// </summary>
    /// <param name="jsRuntime">The <see cref="IJSRuntime"/>.</param>
    /// <param name="identifier">An identifier for the function to invoke.
    /// For example, the value <c>"someScope.someFunction"</c> will invoke the
    /// function <c>window.someScope.someFunction</c>.</param>
    /// <param name="millisecondsTimeout">The duration in milliseconds to try to invoke the function.</param>
    /// <returns>A <see cref="ValueTask"/> of type bool that represents the
    /// success of the asynchronous invocation within the given time.
    /// True if successful, otherwise false.</returns>
    public static async Task<bool> TryInvoke(this IJSRuntime jsRuntime, string identifier, int millisecondsTimeout = 5000)
    {
        var success = false;
        var stopwatch = new Stopwatch();
        stopwatch.Start();
        
        while (!success || stopwatch.ElapsedMilliseconds > millisecondsTimeout)
        {
            try
            {
                await jsRuntime.InvokeVoidAsync(identifier);
                success = true;
            }
            catch
            {
                // try again
            }
        }

        return success;
    }
}