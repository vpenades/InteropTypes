using System;
using System.Collections.Generic;
using System.Text;

// https://docs.microsoft.com/es-es/dotnet/api/system.runtime.serialization.iserializable?view=net-6.0

using JSON = System.Text.Json;

namespace InteropTypes.Graphics.Drawing
{
    [System.Text.Json.Serialization.JsonConverter(typeof(Point2.JsonConverter))]
    partial struct Point2
    {
        /// <inheritdoc/>
        public override string ToString() { return XY.ToString(); }

        /// <inheritdoc/>
        public string ToString(string format, IFormatProvider formatProvider) { return XY.ToString(format, formatProvider); }

        public sealed class JsonConverter : Single2Converter<Point2>
        {
            protected override Point2 Create(float x, float y) { return new Point2(x, y); }
            protected override (float x, float y) Read(in Point2 value) { return (value.X, value.Y); }
        }

        public abstract class Single2Converter<T> : JSON.Serialization.JsonConverter<T>
        {
            public override T Read(ref JSON.Utf8JsonReader reader, Type typeToConvert, JSON.JsonSerializerOptions options)
            {
                _ReadToken(ref reader, JSON.JsonTokenType.StartArray);
                float x = _ReadSingle(ref reader);
                float y = _ReadSingle(ref reader);                
                _ReadToken(ref reader, JSON.JsonTokenType.EndArray);

                return Create(x, y);
            }

            private static void _ReadToken(ref JSON.Utf8JsonReader reader, JSON.JsonTokenType token)
            {
                reader.Read();
                if (reader.TokenType != token)
                {
                    throw new System.Text.Json.JsonException();
                }
            }

            private static float _ReadSingle(ref JSON.Utf8JsonReader reader)
            {
                // Read the x component
                reader.Read();
                if (reader.TokenType != JSON.JsonTokenType.Number) throw new JSON.JsonException();
                float x = reader.GetSingle();
                return x;
            }

            public override void Write(JSON.Utf8JsonWriter writer, T value, JSON.JsonSerializerOptions options)
            {
                var (x, y) = Read(value);

                writer.WriteStartArray();
                writer.WriteNumberValue(x);
                writer.WriteNumberValue(y);                
                writer.WriteEndArray();
            }

            protected abstract T Create(float x, float y);

            protected abstract (float x, float y) Read(in T value);        
        }
    }
}
