using System.Diagnostics;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace TracerCore
{

    [Serializable]
    public class ThreadTrace
    {
        [XmlAttribute] public int Id { get; set; } = -999;
        [XmlAttribute] public string ThreadName { get; set; } = "unknown";
        
        [XmlIgnore]
        [JsonIgnore]
        public Stopwatch Duration { get; } = new();

        [XmlElement] public String Time { get; set; } = "-999ms";
        public List<MethodTrace> MethodsTraces { get; } = new();

        [XmlIgnore]
        [JsonIgnore]
         public Stack<MethodTrace> RunningMethods { get; } = new();

        public ThreadTrace(int id, string threadName)
        {
            Id = id;
            ThreadName = threadName == null ? "thread#"+id : threadName;
        }
        
        public ThreadTrace()
        {
            Id = -999;
            ThreadName = "unknown";
            Time = "-999ms";
        }
        
    }
    
}