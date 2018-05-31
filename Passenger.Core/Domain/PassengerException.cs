using System;

namespace Passenger.Core.Domain
{
    public abstract class PassengerException : Exception
    {
        public string Code { get; }

        protected PassengerException()
        { 
        }

        public PassengerException(string code)
        { 
            Code = code;
        }

        public PassengerException(string message, params object[] args) : this(string.Empty, message, args) 
        { 
        }

        public PassengerException(string code, string message, params object[] args) : this(null, code, message, args) //słowo kluczowe odwołuje się do wywołania innego konstruktora tej samej klasy
        { 
        }

        public PassengerException(Exception InnerException, string message, params object[] args) 
            : this(InnerException, string.Empty, message, args) // 
        { 
        }

        public PassengerException(Exception InnerException, string code, string message, params object[] args) 
            : base(string.Format(message, args), InnerException)
        { 
            Code = code;
        }
    }
}