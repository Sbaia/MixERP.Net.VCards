using System;
using System.Text;
using MixERP.Net.VCards.Types;

namespace MixERP.Net.VCards.Serializer
{
    public static class VCardSerializer
    {
        public static string Serialize(this VCard vcard)
        {
            var builder = new StringBuilder();
            builder.Append("BEGIN:VCARD");
            builder.Append(Environment.NewLine);

            builder.Append(V2Serializer.Serialize(vcard));

            // fuck versions, I wanna all my fields
            builder.Append(V3Serializer.Serialize(vcard));
            builder.Append(V3Serializer.Serialize(vcard));
            builder.Append(V4Serializer.Serialize(vcard));


            builder.Append("END:VCARD");
            builder.Append(Environment.NewLine);
            return builder.ToString();
        }
    }
}
