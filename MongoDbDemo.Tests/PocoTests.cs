using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoDbDemo.Tests
{
    [TestClass]
    public class PocoTests
    {
        public PocoTests()
        {
            JsonWriterSettings.Defaults.Indent = true;
        }

        public class Person
        {
            public int Id { get; set; }
            public string FirstName { get; set; }
            public int Age { get; set; }
            public List<string> Address = new List<string>();
            public Contact Contact = new Contact();
            [BsonIgnore]
            public string IgnoreMe { get; set; }
            [BsonElement("New")] //change the name of the field when converting to bsondocument
            public string Old { get; set; }
            [BsonElement]
            private string PrivateField; //private fields are not included by default

            [BsonRepresentation(BsonType.Double)]
            public decimal DecimalNumber = 100.5m; // by default the decimal type is serialize as string to the bsonDoc, to change the serialization we use the attribute
        }

        public class Contact
        {
            public string Email { get; set; }
            public string Phone { get; set; }
        }

        [TestMethod]
        public void Automatic()
        {
            var person = new Person()
            {
                Age = 54,
                FirstName = "Bob"
            };

            person.Address.Add("101 Some Road");
            person.Address.Add("Unit 500");
            person.Contact.Email = "email@email.com";
            person.Contact.Phone = "555-555-555";

            Console.WriteLine(person.ToJson());
        }

        [TestMethod]
        public void SerializationAttributes()
        {
            var person = new Person();

            Console.WriteLine(person.ToJson());
        }


    }
}
