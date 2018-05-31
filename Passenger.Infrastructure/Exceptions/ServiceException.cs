using System;
using Passenger.Core.Domain;

namespace Passenger.Infrastructure.Exceptions
{
    // specjalizowany wyjątek dla service'ów
    public class ServiceException : PassengerException
    {
        protected ServiceException()
        { 
        }

        public ServiceException(string code) : base(code)
        { 
        }

        public ServiceException(string message, params object[] args) : base(string.Empty, message, args) 
        { 
        }

        public ServiceException(string code, string message, params object[] args) : base(null, code, message, args) //słowo kluczowe odwołuje się do wywołania innego konstruktora tej samej klasy
        { 
        }

        public ServiceException(Exception InnerException, string message, params object[] args) 
            : base(InnerException, string.Empty, message, args) // 
        { 
        }

        public ServiceException(Exception InnerException, string code, string message, params object[] args) 
            : base(code, string.Format(message, args), InnerException)
        { 
        }
    }
}