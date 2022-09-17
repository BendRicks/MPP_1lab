using System.ComponentModel;
using System.Diagnostics;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace TracerCore
{
    public class MethodTrace
    {
        [XmlAttribute] public string ClassName { get; set; } = "unknown";
        [XmlAttribute] public string MethodName { get; set; } = "unknown";

        [XmlIgnore]
        [JsonIgnore]
        public Stopwatch Duration { get; } = new();

        [XmlElement] public String Time { get; set; } = "-999ms";
        public List<MethodTrace> InnerTraces { get; } = new();

        public MethodTrace(string methodName, string className)
        {
            MethodName = methodName;
            ClassName = className;
        }
        
        public MethodTrace() {}
        
    }
}