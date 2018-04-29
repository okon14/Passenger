using System;

namespace Passenger.Core.Domain
{
    public class User
    {
        public Guid Id { get; protected set; }
        public string Email { get; protected set; }
        public string Role { get; protected set; }
        public string Password { get; protected set; }
        public string Salt { get; protected set; }
        public string UserName { get; protected set; }
        public string FullName { get; protected set; }
        public DateTime CreatedAt { get; protected set; }

        protected User()
        { 
        }

        public User(string email, string username,
            string password, string salt)
        {
            Id = Guid.NewGuid();
            Email = email.ToLowerInvariant();
            UserName = username;
            Password = password;
            Salt = salt;
            CreatedAt = DateTime.UtcNow;
            Role = "admin";
        }
    }
}