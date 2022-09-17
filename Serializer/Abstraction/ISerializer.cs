using TracerCore;

namespace Serializer.Abstraction
{

    public interface ISerializer
    {
        string Serialize(TraceResult result);
    }
    
}