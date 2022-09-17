using System.Text.Json;
using Serializer.Abstraction;
using TracerCore;

namespace Serializer;

public class SerializerJson : ISerializer
{
    public string Serialize(TraceResult result)
    {
        var serializeOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };
        return JsonSerializer.Serialize(result, serializeOptions);
    }
}