using System;
using System.Collections.Generic;

namespace Passenger.Core.Domain
{
    public class Driver
    {
        //Ogólnie przykład AggregateRoot
        public Guid Id { get; protected set; }
        public Guid UserId { get; protected set; }
        public string Name { get; protected set; }
        public Vehicle Vehicle  { get; protected set; }
        public IEnumerable<Route> Routes { get; protected set; }
        public IEnumerable<DailyRoute> DailyRoutes { get; protected set; }
        public DateTime UpdatedAt { get; protected set; }

        protected Driver() 
        {
        }

        public Driver (Guid userId)
        {
            Id = Guid.NewGuid();
            UserId = userId;
        }

        public Driver (User user)
        {
            UserId = user.Id;
            Name = user.UserName;
        }

        public Driver (Guid userId, string vehicleBrand, string vehicleName, int vehicleSeats)
        {
            Id = Guid.NewGuid();
            UserId = userId;
            SetVehicle(vehicleBrand, vehicleName, vehicleSeats);
        }

        public void SetVehicle(string brand, string name, int seats)
        {
            Vehicle = Vehicle.Create(brand, name, seats);
            UpdatedAt = DateTime.UtcNow;
        }

    }
}