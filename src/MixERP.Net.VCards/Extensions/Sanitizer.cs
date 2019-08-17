using System;
using System.Collections.Generic;
using System.Linq;

namespace MixERP.Net.VCards.Extensions
{
    public static class Sanitizer
    {
        private static readonly IEnumerable<(string rawToken, string escapeSequence)> EspaceTokens = new List<(string, string)>
        {
            (@"\", @"\\"), // slash must be escaped first
            (Environment.NewLine, @"\n"),
            ("\n", @"\n"),
            (",", @"\,"),
            (";", @"\;"),
            (":", @"\:")
        };

        public static string Escape(this string value)
        {
            return string.IsNullOrWhiteSpace(value) ? string.Empty : EspaceTokens.Aggregate(value, (current, token) => current.Replace(token.rawToken, token.escapeSequence));
        }

        public static string UnEscape(this string value)
        {
            return EspaceTokens.Reverse().Aggregate(value, (current, token) => current.Replace(token.escapeSequence, token.rawToken));
        }
    }
}
