namespace TracerCore;

public class TraceResult
{
    public List<ThreadTrace> ThreadTraces { get; }

    public TraceResult(ICollection<ThreadTrace> threads)
    {
        ThreadTraces = new List<ThreadTrace>(threads);
    }
    
    public TraceResult()
    {
        ThreadTraces = new List<ThreadTrace>();
    }
    
}