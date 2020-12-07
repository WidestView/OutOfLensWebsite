using System;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OutOfLensWebsite.Models;
using OutOfLensWebsite.Models.Data;

namespace OutOfLensWebsiteTests
{
    [TestClass]
    public class ModelTests
    {
        [TestMethod]
        public void TestEmployeeInsertion()
        {
            var employee = new Employee
            {
                Cellphone = "119400248922",
                Cpf = "11111111111",
                Email = "bob@bob.com",
                Gender = "M",
                Name = "bob",
                Password = "bobb",
                Phone = "40028922q,",
                BirthDate = new DateTime(1970, 1, 1),
                Rfid = "abc",
                Rg = "12345",
                AccessLevel = 10,
                IsActive = true,
                SocialName = "bob"
            };


            using var database = new DatabaseConnection();

            var reference = employee.Register(database);

            reference = new ImmutableTableReference<Employee>(Employee.From, reference.Identifier, database);

            Assert.AreEqual(reference.Identifier, employee.Id);

            foreach (PropertyInfo info in typeof(Employee).GetProperties())
            {
                Assert.AreEqual(info.GetValue(employee), info.GetValue(reference.Reference));
            }
        }

        [TestMethod]
        public void TestPackageTypeInsertion()
        {
            PackageType type = new PackageType
            {
                Name = "type first",
                Description = "description"
            };
            
            using var connection = new DatabaseConnection();
            
            type.Insert(connection);
        }

        [TestMethod]
        public void TestPackageInsertion()
        {
            DatabaseConnection connection = new DatabaseConnection();

            Package package = new Package
            {
                Description = "a package",
                Observation = "boop",
                PhotoHeight = 10,
                PhotoWidth = 10,
                Price = "10",
                Quality = "good",
                Quantity = 10,
                Type = 1
            };
            
            package.Insert(connection);
        }

        [TestMethod]
        public void TestOrderInsertion()
        {
            var order = new Order
            {
                Customer = new TableReference<Customer>
                {
                    Identifier = 1
                },
                
                Date = new DateTime(1970, 1, 1),
                
                Done = false,
                
                Package = new TableReference<Package>
                {
                    Identifier = 1
                },
                
            };
            
            var connection = new DatabaseConnection();
            
            order.Insert(connection);
        }
        
        [TestMethod]
        public void TestSessionInsertion()
        {
            var session = new Session
            {
                Address = "rua beep boop",
                Description =  "aaaaa",
                Order = new TableReference<Order>
                {
                    Identifier = 2
                },
                StartTime = new DateTime(1970, 1, 1),
                EndTime =  new DateTime(1970, 1, 1),
            };
            
            var connection = new DatabaseConnection();
            
            session.Insert(connection);
        }

        public void TestReportInsertion()
        {
            
        }
    }
}