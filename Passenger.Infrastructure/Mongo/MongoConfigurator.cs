using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;

namespace Passenger.Infrastructure.Mongo
{
    public static class MongoConfigurator
    {
        private static bool _initialize; // czy było już zaincjalizowane
        public static void Initialize()
        {
            if(_initialize)
            {
                return;
            }
            RegisterConventions();
        }

        private static void RegisterConventions()
        {
            ConventionRegistry.Register("PassengerConventions", new MongoConventions(), x => true);
            _initialize = true;
        }

        private class MongoConventions : IConventionPack
        {
            public IEnumerable<IConvention> Conventions
                => new List<IConvention>
                {
                    new IgnoreExtraElementsConvention(true),
                    new EnumRepresentationConvention(BsonType.String),
                    new CamelCaseElementNameConvention()
                };
        }
    }
}