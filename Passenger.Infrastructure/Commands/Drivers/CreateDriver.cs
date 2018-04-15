using System;

namespace Passenger.Infrastructure.Commands.Drivers
{
    public class CreateDriver : ICommand
    {
        public Guid UserId { get; set; }
        public string VehicleBrand { get; set; }
        public string VehicleName { get; set; }
        public int VehicleSeats { get; set; }
        
    }
}