using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoDbDemo.Models
{
    public class Rental
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Description { get; set; }
        public int NumberOfRooms { get; set; }
        public List<string> Address  = new List<string>();
        [BsonRepresentation(BsonType.Double)]
        public decimal Price { get; set; }
        public string ImageId { get; set; }

        public List<PriceAdjustment> Adjustments = new List<PriceAdjustment>();


        public Rental()
        {
            
        }

        public Rental(PostRental postRental)
        {
            Description = postRental.Description;
            NumberOfRooms = postRental.NumberOfRooms;
            Price = postRental.Price;
            Address = (postRental.Address ?? String.Empty).Split('\n').ToList();
        }

        public void AdjustPrice(AdjustPrice adjustPrice)
        {
            var adjusment = new PriceAdjustment(adjustPrice, Price);
            Adjustments.Add(adjusment);
            Price = adjustPrice.NewPrice;
        }

        public bool HasImage()
        {
            return !String.IsNullOrWhiteSpace(ImageId);
        }


    }
}