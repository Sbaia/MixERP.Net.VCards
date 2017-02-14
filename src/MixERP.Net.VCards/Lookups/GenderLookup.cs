﻿using System;
using System.Collections.Generic;
using System.Linq;
using MixERP.Net.VCards.Types;

namespace MixERP.Net.VCards.Lookups
{
    internal static class GenderLookup
    {
        private static readonly Dictionary<Gender, string> Lookup = new Dictionary<Gender, string>
        {
            {Gender.Female, "F"},
            {Gender.Male, "M"},
            {Gender.NotApplicable, "N"},
            {Gender.Other, "O"},
            {Gender.Unknown, "U"}
        };

        internal static string ToVCardString(this Gender type)
        {
            return Lookup[type];
        }

        internal static Gender Parse(string type)
        {
            if (string.IsNullOrWhiteSpace(type))
            {
                return Gender.Unknown;
            }

            return Lookup.FirstOrDefault(x => string.Equals(x.Value, type, StringComparison.OrdinalIgnoreCase)).Key;
        }
    }
}