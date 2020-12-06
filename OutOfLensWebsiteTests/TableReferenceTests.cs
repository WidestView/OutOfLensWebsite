using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OutOfLensWebsite.Models;

namespace OutOfLensWebsiteTests
{
    [TestClass]
    public class TableReferenceTests
    {
        [TestMethod]
        public void TestReference()
        {
            TableReference<string> subject = new TableReference<string>(1, "beep");

            Assert.AreEqual(subject.Reference, "beep");
            Assert.AreEqual(subject.Identifier, 1);
        }

        [TestMethod]
        public void TestExceptions()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new TableReference<string>(2, null));


            string Builder(int x, DatabaseConnection connection)
            {
                return "boop";
            } 
            
            var database = new DatabaseConnection();

            Assert.ThrowsException<ArgumentNullException>(
                () => new TableReference<string>(null, 5, null)
            );

            Assert.ThrowsException<ArgumentNullException>(
                // ReSharper disable once AccessToDisposedClosure
                () => new TableReference<string>(Builder, 5, null)
            );
            
            Assert.ThrowsException<ArgumentNullException>(
                
                // ReSharper disable once AccessToDisposedClosure
                () => new TableReference<string>(null, 5, database)
            );

            Assert.ThrowsException<ArgumentNullException>(
                () => new TableReference<string>(5, null)
            );
            
            var reference = new TableReference<string>((id, connection) => null, 4, database);

            Assert.AreEqual(reference.Reference, null);
            
            
            database.Dispose();
        }

        [TestMethod]
        public void TestBuilder()
        {
            int callCount = 0;
            
            string Builder(int id, DatabaseConnection connection)
            {
                callCount++;
                return "beep";
            }
            
            DatabaseConnection database = new DatabaseConnection();
            
            var reference = new TableReference<string>(Builder, 4, database);

            // Creating a TableReference shall *NOT* execute the builder
            Assert.AreEqual(callCount, 0);
            
            Assert.AreEqual(reference.Identifier, 4);

            string text = reference.Reference;
            
            // Calling the Reference shall execute the builder
            Assert.AreEqual(text, "beep");
            Assert.AreEqual(callCount, 1);
            
            // The builder shall be executed only once no matter how many times we call the reference
            Assert.AreSame(reference.Reference, text);
            Assert.AreEqual(callCount, 1);
            
            
            database.Dispose();
            
            
        }
    }
}