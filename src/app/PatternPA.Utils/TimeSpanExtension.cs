using System;

namespace PatternPA.Utils
{
    public static class TimeSpanExtension
    {
        public static string ToReadableString(this TimeSpan span)
        {
            string formatted = string.Format("{0}{1}{2}{3}{4}",
                span.Days > 0 ? string.Format("{0:0}d-", span.Days) : string.Empty,
                span.Hours > 0 ? string.Format("{0:0}h-", span.Hours) : string.Empty,
                span.Minutes > 0 ? string.Format("{0:0}m-", span.Minutes) : string.Empty,
                span.Seconds > 0 ? string.Format("{0:0}s-", span.Seconds) : string.Empty,
                span.Milliseconds > 0 ? string.Format("{0:0}ms", span.Milliseconds) : string.Empty);

            if (formatted.EndsWith("-")) formatted = formatted.Substring(0, formatted.Length - 1);

            return formatted;
        }
    }
}
