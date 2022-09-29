using TracerCore;

namespace Serializer.Abstraction
{

    public interface ISerializer
    {
        string Format();
        string Serialize(TraceResult result);
    }
    
}