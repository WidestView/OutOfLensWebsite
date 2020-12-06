using Microsoft.VisualStudio.TestTools.UnitTesting;
using OutOfLensWebsite.Models;

namespace OutLensWebsiteTests
{
    [TestClass]
    public class DatabaseTests
    {
        [TestMethod]
        public void TestConnection()
        {
            new DatabaseConnection().Run("select CÓDIGO from FUNCIONÁRIO LIMIT 1");
        }

    }
}
