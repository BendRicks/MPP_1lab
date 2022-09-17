using System.Collections.Concurrent;

namespace TracerCore.Examples
{

    public class AccountManager
    {
        private ITracer _tracer;
        private PasswordHasher _passwordHasher;
        private Dictionary<string, string> _accounts = new();

        public AccountManager(ITracer tracer)
        {
            _tracer = tracer;
            _passwordHasher = new(tracer);
        }

        public void SignUp(string username, string password)
        {
            _tracer.StartTrace();
            _accounts.Add(username, _passwordHasher.Hash(password));
            _tracer.StopTrace();
        }

        public bool SignIn(string username, string password)
        {
            _tracer.StartTrace();
            _accounts.TryGetValue(username, out var pass);
            bool loggedIn = _passwordHasher.Verify(password, pass!);
            _tracer.StopTrace();
            return loggedIn;
        }
    }
    
}