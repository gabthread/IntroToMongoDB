using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MongoDbDemo.App_Start;
using MongoDbDemo.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;

namespace MongoDbDemo.Controllers
{
    public class RentalsController : Controller
    {
        public readonly RealEstateContext Context = new RealEstateContext();
        public ActionResult Post()
        {
            return View();
        }

        public ActionResult Index(RentalsFilter filters)
        {
            var rentals = FilterRentals(filters).SortBy(x => x.Price).ToList();

            var model = new RentalList
            {
                Rentals = rentals,
                Filters = filters
            };

            return View(model);
        }

        private IFindFluent<Rental, Rental> FilterRentals(RentalsFilter filters)
        {


            if (!filters.PriceLimit.HasValue && !filters.MinimumRooms.HasValue)
            {
                //We have to sort the collection before we enumerate it using ToList method 
                return Context.Rentals.Find(x => true);
            }
            else if (filters.PriceLimit.HasValue && !filters.MinimumRooms.HasValue)
            {

                return Context.Rentals.Find(Builders<Rental>.Filter.Lte(x => x.Price, filters.PriceLimit.Value));
                //Also can be used the fluent Mongo expression which is similar to Linq
                //rentals = Context.Rentals.Find(x => x.Price <= filters.PriceLimit.Value).ToList();
            }
            else if (filters.MinimumRooms.HasValue && !filters.PriceLimit.HasValue)
            {
                //Using LINQ
                //return Context.Rentals.AsQueryable().Where(x => x.NumberOfRooms >= filters.MinimumRooms);
                return Context.Rentals.Find(x => x.NumberOfRooms >= filters.MinimumRooms);
            }
            else
            {
                //Using LINQ
                //return Context.Rentals.AsQueryable().Where(x => x.NumberOfRooms >= filters.MinimumRooms.Value && x.Price >= filters.PriceLimit.Value);
                return Context.Rentals.Find(x => x.NumberOfRooms >= filters.MinimumRooms.Value && x.Price <= filters.PriceLimit.Value);
            }
        }

        [HttpPost]
        public ActionResult Post(PostRental postRental)
        {
            var rental = new Rental(postRental);
            Context.Rentals.InsertOne(rental);
            return RedirectToAction("Index");
        }

        public ActionResult AdjustPrice(string id)
        {
            var rental = Context.Rentals.Find(x => x.Id == id).FirstOrDefault();
            return View(rental);
        }

        [HttpPost]
        public ActionResult AdjustPrice(string id, AdjustPrice adjustPrice)
        {
            var rental = Context.Rentals.Find(x => x.Id == id).FirstOrDefault();

            //UPDATE - TYPE: REPLACE DOCUMENT
            //rental.AdjustPrice(adjustPrice);
            //Context.Rentals.ReplaceOne(x => x.Id == rental.Id, rental); //here I am replacing the whole document for a new version of the document.

            //UPDATE - TYPE: MODIFY DOCUMENT
            var adjustment = new PriceAdjustment(adjustPrice, rental.Price);
            //here we are adding the adjustment to the collection and updating the field Price to a new number
            var updateDefinition = new UpdateDefinitionBuilder<Rental>().Push(x => x.Adjustments, adjustment).Set(x => x.Price, adjustPrice.NewPrice);
            //here we actually execute the update.
            var updateResult = Context.Rentals.UpdateOne(x => x.Id == rental.Id, updateDefinition);

            //NOTE: GENERALY IN OOP IS USED REPLACEMENT INSTEAD OF MODIFICATION. ALTHOUGH MOFICATION HAS BETTER PERFORMANCE

            return RedirectToAction("Index");
        }

        public ActionResult Delete(string id)
        {
            Context.Rentals.DeleteOne(x => x.Id == id);
            return RedirectToAction("Index");
        }

        public ActionResult AttachImage(string id)
        {
            var rental = Context.Rentals.Find(x => x.Id == id).FirstOrDefault();
            return View(rental);
        }

        [HttpPost]
        public ActionResult AttachImage(string id, HttpPostedFileBase file)
        {
            var rental = Context.Rentals.Find(x => x.Id == id).FirstOrDefault();
            if (rental.HasImage())
            {
                DeleteImage(rental);
            }
            StoreImage(file, rental);
            return RedirectToAction("Index");
        }

        private void DeleteImage(Rental rental)
        {
            var gridFs = new GridFSBucket(Context.MongoDatabase);
            gridFs.Delete(new ObjectId(rental.ImageId));
            rental.ImageId = null;
            Context.Rentals.ReplaceOne(x => x.Id == rental.Id, rental);
        }

        private void StoreImage(HttpPostedFileBase file, Rental rental)
        {
            var imageId = ObjectId.GenerateNewId();
            rental.ImageId = imageId.ToString();
            Context.Rentals.ReplaceOne(x => x.Id == rental.Id, rental);

            var options = new GridFSUploadOptions()
            {
                Metadata = new BsonDocument
                        {
                            { "Id", imageId },
                            { "ContentType", file.ContentType },
                        }

            };

            var gridFs = new GridFSBucket(Context.MongoDatabase);
            gridFs.UploadFromStream(file.FileName, file.InputStream, options);
        }

        public ActionResult GetImage(string id)
        {
            var gridFs = new GridFSBucket(Context.MongoDatabase);
            //var filter = Builders<GridFSFileInfo>.Filter.Eq(x => x.Filename, "please-keep-your-voice-down.png");
            var filter = Builders<GridFSFileInfo>.Filter.Eq(x => x.Metadata["Id"], new ObjectId(id));

            var imageFileInfo = gridFs.Find(filter).FirstOrDefault();

            var imageBytes = gridFs.DownloadAsBytes(imageFileInfo.Id);
            
            if (imageBytes == null)
            {
                return HttpNotFound();
            }
            return File(imageBytes, imageFileInfo.Metadata["ContentType"].ToString());
        }


    }
}