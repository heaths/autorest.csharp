// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Text.Json;
using Azure.Core;

namespace CognitiveSearch.Models
{
    public partial class IndexingSchedule : IUtf8JsonSerializable
    {
        void IUtf8JsonSerializable.Write(Utf8JsonWriter writer)
        {
            writer.WriteStartObject();
            writer.WritePropertyName("interval");
            writer.WriteStringValue(Interval, "P");
            if (StartTime != null)
            {
                writer.WritePropertyName("startTime");
                writer.WriteStringValue(StartTime.Value, "S");
            }
            writer.WriteEndObject();
        }
        internal static IndexingSchedule DeserializeIndexingSchedule(JsonElement element)
        {
            IndexingSchedule result = new IndexingSchedule();
            foreach (var property in element.EnumerateObject())
            {
                if (property.NameEquals("interval"))
                {
                    result.Interval = property.Value.GetTimeSpan("P");
                    continue;
                }
                if (property.NameEquals("startTime"))
                {
                    if (property.Value.ValueKind == JsonValueKind.Null)
                    {
                        continue;
                    }
                    result.StartTime = property.Value.GetDateTimeOffset("S");
                    continue;
                }
            }
            return result;
        }
    }
}
