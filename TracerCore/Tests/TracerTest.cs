using NUnit.Framework;

namespace TracerCore.Tests
{

    public class TracerTest
    {
        private static ITracer _tracer;

        [SetUp]
        public void SetMock()
        {
            _tracer = new Tracer();
        }

        [Test]
        public void ThreadsTest()
        {
            _tracer.StartTrace();
            int currentThreadId = Thread.CurrentThread.ManagedThreadId;
            ((Tracer)_tracer).Threads.TryGetValue(currentThreadId, out var currentThreadTrace);
            Assert.That(currentThreadTrace != null);
            Assert.That(((Tracer)_tracer).Threads.Count == 1);
            _tracer.StopTrace();
            Assert.That(!currentThreadTrace.Time.Equals("-999ms"));
            Thread thread1 = new Thread(() =>
            {
                _tracer.StartTrace();
                ((Tracer)_tracer).Threads.TryGetValue(Thread.CurrentThread.ManagedThreadId, out var InnerCurrentThreadTrace);
                Assert.That(InnerCurrentThreadTrace != null);
                _tracer.StopTrace();
                Assert.That(!InnerCurrentThreadTrace!.Time.Equals("-999ms"));
            });
            thread1.Start();
            thread1.Join();
            Assert.That(((Tracer)_tracer).Threads.Count == 2);
            Thread thread2 = new Thread(() =>
            {
                _tracer.StartTrace();
                Assert.That(((Tracer)_tracer).Threads.Count == 3);
                ((Tracer)_tracer).Threads.TryGetValue(Thread.CurrentThread.ManagedThreadId, out var InnerCurrentThreadTrace);
                Assert.That(InnerCurrentThreadTrace != null);
                _tracer.StopTrace();
                Assert.That(!InnerCurrentThreadTrace!.Time.Equals("-999ms"));
            });
            thread2.Start();
            thread2.Join();
            Assert.That(!currentThreadTrace.Time.Equals("-999ms"));
        }

        [Test]
        public void MethodsTest()
        {
            _tracer.StartTrace();
            int currentThreadId = Thread.CurrentThread.ManagedThreadId;
            ((Tracer)_tracer).Threads.TryGetValue(currentThreadId, out var currentThreadTrace);
            Assert.That(currentThreadTrace != null);
            Assert.That(currentThreadTrace!.RunningMethods.Count == 1);
            Assert.That(currentThreadTrace!.MethodsTraces.Count == 1);
            _tracer.StartTrace();
            Assert.That(currentThreadTrace!.RunningMethods.Count == 2);
            Assert.That(currentThreadTrace!.MethodsTraces.Count == 1);
            Assert.That(currentThreadTrace!.MethodsTraces[0].InnerTraces.Count == 1);
            _tracer.StopTrace();
            Assert.That(currentThreadTrace!.RunningMethods.Count == 1);
            Assert.That(currentThreadTrace!.MethodsTraces.Count == 1);
            Assert.That(currentThreadTrace!.MethodsTraces[0].InnerTraces.Count == 1);
            _tracer.StopTrace();
            Assert.That(currentThreadTrace!.RunningMethods.Count == 0);
            Assert.That(currentThreadTrace!.MethodsTraces.Count == 1);
            Assert.That(currentThreadTrace!.MethodsTraces[0].InnerTraces.Count == 1);
        }
        
    }
}