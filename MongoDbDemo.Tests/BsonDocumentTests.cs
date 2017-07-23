using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;

namespace MongoDbDemo.Tests
{
    [TestClass]
    public class BsonDocumentTests
    {
        public BsonDocumentTests()
        {
            JsonWriterSettings.Defaults.Indent = true;
        }

        [TestMethod]
        public void EmptyDocument()
        {
            var document = new BsonDocument();
            Console.WriteLine(document);
        }


        [TestMethod]
        public void AddElements()
        {
            var person = new BsonDocument
            {
                {"age", new BsonInt32(54) }, //Collection initializer
                {"IsAlive", true } //implict convert the .NET boolean type to BsonBoolean Type (not all .net types are converted automatically)
            };
            person.Add("FirstName", new BsonString("Bob"));

            Console.WriteLine(person);
        }

        [TestMethod]
        public void AddingArrays()
        {
            var person = new BsonDocument();
            person.Add("address", new BsonArray(new[] {"101 Some Road", "Unit 1"}));

            Console.WriteLine(person);
        }

        [TestMethod]
        public void EmbeddedDocument()
        {
            var person = new BsonDocument
            {
                {"contact", new BsonDocument
                            {
                                {"phone","123-456-789"},
                                { "email", "bla@bla.com" }
                            }
                }
            };

            Console.WriteLine(person);
        }

        [TestMethod]
        public void BsonValueConversions()
        {
            var person = new BsonDocument
            {
                {"age", 54 }, 
            };
            

            Console.WriteLine(person["age"].AsInt32 + 10);
            Console.WriteLine(person["age"].IsInt32);
            Console.WriteLine(person["age"].IsString);
        }

        [TestMethod]
        public void ToBson()
        {
            var person = new BsonDocument
            {
                {"FirstName", "Bob" },
            };

            var bson = person.ToBson(); //bson is in binary format (this is essentially how the driver send documents from client to server and viceversa)

            Console.WriteLine(BitConverter.ToString(bson));

            var deserializedPerson = BsonSerializer.Deserialize<BsonDocument>(bson);
            Console.WriteLine(deserializedPerson);
        }




    }
}
