using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Serializer.Abstraction;
using TracerCore;

namespace Serializer;

public class SerializerXml : ISerializer
{
    public string Serialize(TraceResult result)
    {
        XmlSerializer xmlSerializer = new XmlSerializer(typeof(TraceResult));
        new XmlWriterSettings();
        var sB = new StringBuilder();
        XmlWriter xmlWriter = XmlWriter.Create(sB, new XmlWriterSettings()
        {
            
            Indent = true
        });
        xmlSerializer.Serialize(xmlWriter, result);
        return sB.ToString();
    }
}