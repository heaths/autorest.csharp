// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Text.Json;
using Azure.Core;

namespace CognitiveSearch.Models
{
    public partial class KeywordMarkerTokenFilter : IUtf8JsonSerializable
    {
        void IUtf8JsonSerializable.Write(Utf8JsonWriter writer)
        {
            writer.WriteStartObject();
            writer.WritePropertyName("keywords");
            writer.WriteStartArray();
            foreach (var item in Keywords)
            {
                writer.WriteStringValue(item);
            }
            writer.WriteEndArray();
            if (IgnoreCase != null)
            {
                writer.WritePropertyName("ignoreCase");
                writer.WriteBooleanValue(IgnoreCase.Value);
            }
            writer.WritePropertyName("@odata.type");
            writer.WriteStringValue(OdataType);
            writer.WritePropertyName("name");
            writer.WriteStringValue(Name);
            writer.WriteEndObject();
        }
        internal static KeywordMarkerTokenFilter DeserializeKeywordMarkerTokenFilter(JsonElement element)
        {
            KeywordMarkerTokenFilter result = new KeywordMarkerTokenFilter();
            foreach (var property in element.EnumerateObject())
            {
                if (property.NameEquals("keywords"))
                {
                    foreach (var item in property.Value.EnumerateArray())
                    {
                        result.Keywords.Add(item.GetString());
                    }
                    continue;
                }
                if (property.NameEquals("ignoreCase"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        continue;
                    }
                    result.IgnoreCase = property.Value.GetBoolean();
                    continue;
                }
                if (property.NameEquals("@odata.type"))
                {
                    result.OdataType = property.Value.GetString();
                    continue;
                }
                if (property.NameEquals("name"))
                {
                    result.Name = property.Value.GetString();
                    continue;
                }
            }
            return result;
        }
    }
}
