using Serializer;
using TracerCore;
using TracerCore.Examples;

public class Program
{
    private static string SO_PATH = "C:/Users/bendr/Documents/SO/";
    private static ITracer _tracer = new Tracer();

    static void Main(string[] args)
    {
        _tracer.StartTrace();
        AccountManager accountManager = new AccountManager(_tracer);
        var thread1 = new Thread(() =>
        {
            _tracer.StartTrace();
            accountManager.SignUp("bendricks", "testpassword");
            _tracer.StopTrace();
        });
        thread1.Start();
        thread1.Join();
        var thread2 = new Thread(() =>
        {
            _tracer.StartTrace();
            if (accountManager.SignIn("bendricks", "testpassword"))
            {
                Console.WriteLine("Validated");
            }
            _tracer.StopTrace();
        });
        thread2.Start();
        thread2.Join();
        _tracer.StopTrace();
        string json = new SerializerJson().Serialize(_tracer.GetTraceResult());
        string xml =new SerializerXml().Serialize(_tracer.GetTraceResult());
        Console.WriteLine(json);
        var sw = new StreamWriter(SO_PATH+"SO.json", false);
        sw.WriteLine(json);
        sw.Close();
        Console.WriteLine(xml);
        sw = new StreamWriter(SO_PATH+"SO.xml", false);
        sw.WriteLine(xml);
        sw.Close();
    }
    
}