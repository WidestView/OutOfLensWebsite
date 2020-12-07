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

            reference = new TableReference<Employee>(Employee.From, reference.Identifier, database);

            Assert.AreEqual(reference.Identifier, employee.Id);

            foreach (PropertyInfo info in typeof(Employee).GetProperties())
            {
                Assert.AreEqual(info.GetValue(employee), info.GetValue(reference.Reference));
            }
        }

        public void TestPackageInsertion()
        {
            
        }
    
        
    }
}