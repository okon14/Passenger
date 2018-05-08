using System;
using System.Collections.Generic;

namespace Passenger.Core.Domain
{
    public class Driver
    {
        //Ogólnie przykład AggregateRoot
        public Guid Id { get; protected set; }
        public Guid UserId { get; protected set; }
        public Vehicle Vehicle  { get; protected set; }
        public IEnumerable<Route> Routes { get; protected set; }
        public IEnumerable<DailyRoute> DailyRoutes { get; protected set; }

        protected Driver() 
        {
        }

        public Driver (Guid userId)
        {
            Id = Guid.NewGuid();
            UserId = userId;
        }

        public Driver (Guid userId, string vehicleBrand, string vehicleName, int vehicleSeats)
        {
            Id = Guid.NewGuid();
            UserId = userId;
            Vehicle = Vehicle.Create(vehicleBrand, vehicleName, vehicleSeats);
        }

    }
}