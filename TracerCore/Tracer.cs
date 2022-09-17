using System.Collections.Concurrent;
using System.Diagnostics;
using System.Reflection;

namespace TracerCore
{
    public class Tracer : ITracer
    {
        
        private readonly ConcurrentDictionary<int, ThreadTrace> _threads = new();

        void ITracer.StartTrace()
        {
            MethodBase? method = new StackTrace().GetFrame(1)!.GetMethod();
            int threadId = Thread.CurrentThread.ManagedThreadId;
            ThreadTrace thread = _threads.GetOrAdd(threadId, id =>
            {
                ThreadTrace threadInfo = new ThreadTrace(id, Thread.CurrentThread.Name);
                threadInfo.Duration.Start();
                return threadInfo;
            });
            MethodTrace trace = new MethodTrace(method!.Name, method.DeclaringType!.Name);
            if (thread.RunningMethods.Count == 0)
            {
                thread.MethodsTraces.Add(trace);
            }
            else
            {
                thread.RunningMethods.Peek().InnerTraces.Add(trace);
            }
            thread.RunningMethods.Push(trace);
            trace.Duration.Start();
        }

        void ITracer.StopTrace()
        {
            int threadId = Thread.CurrentThread.ManagedThreadId;
            _threads.TryGetValue(threadId, out var thread);
            if (thread.RunningMethods.Count > 0)
            {
                MethodTrace methodTrace = thread.RunningMethods.Pop();
                methodTrace.Duration.Stop();
                methodTrace.Time = $"{methodTrace.Duration.Elapsed.TotalMilliseconds:f0}ms";
            }
            if (thread.RunningMethods.Count == 0)
            {
                thread.Duration.Stop();
                thread.Time = $"{thread.Duration.Elapsed.TotalMilliseconds:f0}ms";
            }
        }

        TraceResult ITracer.GetTraceResult()
        {
            return new TraceResult(_threads.Values);
        }

    }
}