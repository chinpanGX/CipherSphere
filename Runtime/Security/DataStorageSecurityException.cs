using System;

namespace CipherSphere.Runtime.Security
{
    public class DataStorageSecurityException : ArgumentException
    {
        public DataStorageSecurityException(string message, string paramName) : base(message, paramName) { }
    }
}