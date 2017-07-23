using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;

namespace MongoDbDemoConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                //we need a MongoDB client to interact with the server. 
                MongoClient dbClient = new MongoClient("mongodb://localhost");

                //List of Databases  
                var dbList = dbClient.ListDatabases().ToList();

                Console.WriteLine("The list of databases are :");
                foreach (var item in dbList)
                {
                    Console.WriteLine(item);
                }

                Console.WriteLine("\n\n");

                //Get Database and Collection  
                IMongoDatabase mongoDatabase = dbClient.GetDatabase("Demo");
                var collList = mongoDatabase.ListCollections().ToList();

                Console.WriteLine("The list of collections are :");
                foreach (var item in collList)
                {
                    Console.WriteLine(item);
                }

                var people = mongoDatabase.GetCollection<BsonDocument>("People");

                //CREATE  
                BsonElement firstNameElement = new BsonElement("FirstName", "Gabriel");

                BsonDocument personDoc = new BsonDocument();
                personDoc.Add(firstNameElement);
                personDoc.Add(new BsonElement("Age", 50));

                people.InsertOne(personDoc);

                //UPDATE  
                BsonElement updateFirstNameElement = new BsonElement("FirstName", "Pedro");

                BsonDocument updatePersonDoc = new BsonDocument();
                updatePersonDoc.Add(updateFirstNameElement);
                updatePersonDoc.Add(new BsonElement("Age", 80));

                BsonDocument findPersonDoc = new BsonDocument(new BsonElement("FirstName", "Gabriel"));

                var updateDoc = people.FindOneAndReplace(findPersonDoc, updatePersonDoc);

                Console.WriteLine(updateDoc);

                //DELETE  
                BsonDocument findAnotherPersonDoc = new BsonDocument(new BsonElement("FirstName", "Pedro"));
                people.FindOneAndDelete(findAnotherPersonDoc);

                //READ  
                var resultDoc = people.Find(new BsonDocument()).ToList();
                foreach (var item in resultDoc)
                {
                    Console.WriteLine(item.ToString());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.ReadKey();
        }
    }
}
