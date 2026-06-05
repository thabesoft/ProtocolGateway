using Avalonia.Collections;

namespace ThabeSoft.ProtocolGateway.Extensions;


internal static class AvaloniaListExtensions
{
    extension<T>(IEnumerable<T> values)
    {
        public AvaloniaList<T> ToAvaloniaList()
        {
            return [.. values];
        }
    }
}
