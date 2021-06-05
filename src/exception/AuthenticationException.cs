using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.okitoki.kraken.exception
{
    public class AuthenticationException : Exception
    {
        public AuthenticationException() : base() { }

        public AuthenticationException(string message) : base(message) { }
    }
}
