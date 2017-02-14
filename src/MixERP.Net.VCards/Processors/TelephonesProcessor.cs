﻿using System.Collections.Generic;
using System.Linq;
using System.Text;
using MixERP.Net.VCards.Extensions;
using MixERP.Net.VCards.Lookups;
using MixERP.Net.VCards.Models;
using MixERP.Net.VCards.Types;

namespace MixERP.Net.VCards.Processors
{
    internal static class TelephonesProcessor
    {
        internal static string ToVCardToken(this IEnumerable<Telephone> value, VCardVersion version)
        {
            if (value == null)
            {
                return string.Empty;
            }

            var builder = new StringBuilder();

            foreach (var phone in value)
            {
                string type = phone.Type.ToVCardString(version);

                string key = "TEL";

                if (version == VCardVersion.V4)
                {
                    if (phone.Preference > 0)
                    {
                        key = key + ";PREF=" + phone.Preference;
                    }
                }

                builder.Append(GroupProcessor.Serialize(key, version, type, true, phone.Number));
            }

            return builder.ToString();
        }

        internal static void Parse(Token token, ref VCard vcard)
        {
            var telephone = new Telephone();
            var preference = token.AdditionalKeyMembers.FirstOrDefault(x => x.Key == "PREF");
            var type = token.AdditionalKeyMembers.FirstOrDefault(x => x.Key == "TYPE");

            telephone.Preference = preference.Value.ConvertTo<int>();
            telephone.Type = TelephoneTypeLookup.Parse(type.Value);
            telephone.Number = token.Values[0];

            var telephones = (List<Telephone>) vcard.Telephones ?? new List<Telephone>();
            telephones.Add(telephone);
            vcard.Telephones = telephones;
        }
    }
}