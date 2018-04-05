namespace Passenger.Core.Domain
{
    public class Node
    {
        //Reprezetuje ValueObject
        public string Address { get; protected set; }
        public decimal Longitude { get; protected set; }
        public decimal Latitude { get; protected set; }
    }
}