using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDbDemo.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace MongoDbDemo
{
    public class QueryPriceDistribution
    {
        //public IEnumerable<BsonDocument> Run(MongoCollectionBase<Rental> rentals)
        //{
        //    var priceRange = new BsonDocument(
        //        "$subtract",
        //        new BsonArray
        //        {
        //            "$Price",
        //            new BsonDocument(
        //                "$mod",
        //                new BsonArray {"$Price",500})
        //        });


        //    var grouping = new BsonElement(
        //        "$group",
        //        new BsonDocument
        //        {
        //            {"_id", priceRange},
        //            {"count", new BsonDocument("$sum", 1)}
        //        });

        //    var sort = new BsonDocument("$sort",new BsonDocument("_id", 1));

            

        //}
    }
}