using System.Text.Json;
using System.Text.Json.Serialization;

namespace Backend.Helpers
{
    public class DateOnlyJsonConverter : JsonConverter<DateOnly>
    {
        private readonly string _dateFormat;

        public DateOnlyJsonConverter(string dateFormat = DateFormats.DefaultDateFormat)
        {
            _dateFormat = dateFormat;
        }

        public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value = reader.GetString();
            return DateOnly.Parse(value!);
        }

        public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString(_dateFormat));
        }
    }
}
