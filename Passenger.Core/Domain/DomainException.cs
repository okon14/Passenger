using System;

namespace Passenger.Core.Domain
{
    // klasa specjalozowanego wyjątku w naszej domenie
    public class DomainException : PassengerException
    {
        protected DomainException()
        { 
        }

        public DomainException(string code) : base(code)
        { 
        }

        public DomainException(string message, params object[] args) : base(string.Empty, message, args) 
        { 
        }

        public DomainException(string code, string message, params object[] args) : base(null, code, message, args) //słowo kluczowe odwołuje się do wywołania innego konstruktora tej samej klasy
        { 
        }

        public DomainException(Exception InnerException, string message, params object[] args) 
            : base(InnerException, string.Empty, message, args) // 
        { 
        }

        public DomainException(Exception InnerException, string code, string message, params object[] args) 
            : base(code, string.Format(message, args), InnerException)
        { 
        }
    }
}