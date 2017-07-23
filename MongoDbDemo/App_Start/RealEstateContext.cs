using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDbDemo.Models;
using MongoDbDemo.Properties;
using MongoDB.Driver;

namespace MongoDbDemo.App_Start
{
    public class RealEstateContext
    {
        public IMongoDatabase MongoDatabase { get; set; }
        public RealEstateContext()
        {
            //creation of the mongodb client
            var mongoClient = new MongoClient(Settings.Default.RealEstateConnectionString);
            //get mongo database
            MongoDatabase = mongoClient.GetDatabase(Settings.Default.RealEstateDatabaseName);

        }

        public IMongoCollection<Rental> Rentals
        {
            get { return MongoDatabase.GetCollection<Rental>("rentals"); }
        }

    }
}