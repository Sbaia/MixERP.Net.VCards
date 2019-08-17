using System.Text;
using MixERP.Net.VCards.Models;
using MixERP.Net.VCards.Serializer;
using System.Collections.Generic;
using System.Linq;
using MixERP.Net.VCards.Extensions;

namespace MixERP.Net.VCards.Processors
{
    public static class ExtensionsProcessor
    {
        public static string Serialize(VCard vcard)
        {
            if (vcard.CustomExtensions == null || vcard.CustomExtensions.Count() == 0)
            {
                return string.Empty;
            }

            var builder = new StringBuilder();
            foreach (var extension in vcard.CustomExtensions)
            {
                var key = extension.Key;
                foreach (var value in extension.Values)
                {
                    if (string.IsNullOrWhiteSpace(value))
                    {
                        continue;
                    }

                    var vcardString = DefaultSerializer.GetVCardString(key, value, extension.EscapeContent ?? true, vcard.Version);
                    builder.Append(vcardString);
                }
            }

            return builder.ToString();
        }

        public static void Parse(Token token, ref VCard vcard)
        {
            var canEscape =         string.Join(";", token.Values.Select(v => v.Escape())) == token.RawValue;
            var canIgnoreEscaping = string.Join(";", token.Values) == token.RawValue;

            var key = token.Key;
            var extensions = (List<CustomExtension>)vcard.CustomExtensions ?? new List<CustomExtension>();

            var entry = extensions.FirstOrDefault(x => x.Key.Equals(key, System.StringComparison.OrdinalIgnoreCase));
            var escape = entry?.EscapeContent;
            if (!canEscape)
                escape = false;
            else if (!canIgnoreEscaping)
                escape = true;

            if (entry != null)
            {
                entry.Values = entry.Values.Union(token.Values);
            }
            else
            {
                extensions.Add(new CustomExtension
                {
                    Key = token.Key,
                    Values = token.Values,
                    EscapeContent = escape
                });
            }

            vcard.CustomExtensions = extensions;
        }
    }
}
