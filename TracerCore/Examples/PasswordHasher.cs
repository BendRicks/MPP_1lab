using System.Security.Cryptography;

namespace TracerCore.Examples
{
    public class PasswordHasher
    {
        private ITracer _tracer;
        
        private const int SaltSize = 16;
        private const int HashSize = 20;

        public PasswordHasher(ITracer tracer)
        {
            _tracer = tracer;
        }
        
        public string Hash(string password, int iterations)
        {
            _tracer.StartTrace();
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[SaltSize]);
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations);
            var hash = pbkdf2.GetBytes(HashSize);
            var hashBytes = new byte[SaltSize + HashSize];
            Array.Copy(salt, 0, hashBytes, 0, SaltSize);
            Array.Copy(hash, 0, hashBytes, SaltSize, HashSize);
            var base64Hash = Convert.ToBase64String(hashBytes);
            string strToReturn = string.Format("$MYHASH$V1${0}${1}", iterations, base64Hash);
            _tracer.StopTrace();
            return strToReturn;
        }
        
        public string Hash(string password)
        {
            _tracer.StartTrace();
            string hash = Hash(password, 10000);
            _tracer.StopTrace();
            return hash;
        }
        
        public bool IsHashSupported(string hashString)
        {
            _tracer.StartTrace();
            bool isHashSupported = hashString.Contains("$MYHASH$V1$");
            _tracer.StopTrace();
            return isHashSupported;
        }
        
        public bool Verify(string password, string hashedPassword)
        {
            _tracer.StartTrace();
            if (!IsHashSupported(hashedPassword))
            {
                throw new NotSupportedException("The hashtype is not supported");
            }
            var splittedHashString = hashedPassword.Replace("$MYHASH$V1$", "").Split('$');
            var iterations = int.Parse(splittedHashString[0]);
            var base64Hash = splittedHashString[1];
            var hashBytes = Convert.FromBase64String(base64Hash);
            var salt = new byte[SaltSize];
            Array.Copy(hashBytes, 0, salt, 0, SaltSize);
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations);
            byte[] hash = pbkdf2.GetBytes(HashSize);
            for (var i = 0; i < HashSize; i++)
            {
                if (hashBytes[i + SaltSize] != hash[i])
                {
                    _tracer.StopTrace();
                    return false;
                }
            }
            _tracer.StopTrace();
            return true;
        }
    }
}