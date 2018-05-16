using System;
using System.Collections.Generic;
using System.Linq;

namespace Passenger.Core.Domain
{
    public class Driver
    {
        private ISet<Route> _routes = new HashSet<Route>();
        private ISet<DailyRoute> _dailyRoutes = new HashSet<DailyRoute>();
        //Ogólnie przykład AggregateRoot bo ma własne niepowtarzalne ID
        public Guid Id { get; protected set; }
        public Guid UserId { get; protected set; }
        public string Name { get; protected set; }
        public Vehicle Vehicle  { get; protected set; }
        public DateTime UpdatedAt { get; protected set; }

        public IEnumerable<Route> Routes
        {
            get { return _routes; }
            set { _routes = new HashSet<Route>(value); }
        }
        public IEnumerable<DailyRoute> DailyRoutes
        {
            get { return _dailyRoutes; }
            set { _dailyRoutes = new HashSet<DailyRoute>(value); }
        }

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

        public void SetVehicle(Vehicle vehicle)
        {
            Vehicle = vehicle;
            UpdatedAt = DateTime.UtcNow;
        }

        public void AddRoute(string name, Node start, Node end, double distance)
        {
            var route = _routes.SingleOrDefault(x => x.Name == name);
            if(route != null)
            {
                throw new Exception($"Route with name {name} already exists for user {this.Name}");
            }
            if(distance < 0)
            {
                throw new Exception($"Route with name {name} can not have negative distance");
            }
            _routes.Add(Route.Create(name, start, end, distance));
            UpdatedAt = DateTime.Now;
        }

        public void DeleteRoute(string name)
        {
            var route = _routes.SingleOrDefault(x => x.Name == name);
            if(route == null)
            {
                throw new Exception($"Route with name {name} not exists for user {this.Name}");
            }
            _routes.Remove(route);
            UpdatedAt = DateTime.Now;
        }

    }
}