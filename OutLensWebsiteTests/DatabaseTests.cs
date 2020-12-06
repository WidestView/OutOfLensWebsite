using Microsoft.VisualStudio.TestTools.UnitTesting;
using OutOfLens_ASP.Models;

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
