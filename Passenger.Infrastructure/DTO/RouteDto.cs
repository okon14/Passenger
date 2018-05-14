using System;

namespace Passenger.Infrastructure.DTO
{
    public class RouteDto
    {
        public string Name { get; set; }
        public NodeDto Start { get; set; }
        public NodeDto End { get; set; } 
        public double Length { get; set; }  
    }
}