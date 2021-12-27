using Microsoft.VisualStudio.TestTools.UnitTesting;
using SensibleProgramming.CodePro.Models.Oracle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SensibleProgramming.CodePro.Models.Oracle.Tests
{
    [TestClass()]
    public class OracleDataManagerTests
    {
        [TestMethod()]
        public void GetInstanceTest()
        {
            OracleDataManager _mgr = new OracleDataManager();
            _mgr.OnError = (ex) => {
                Assert.Fail(ex.Message);
            };

            var tbls = _mgr.GetInstance("mo1u1oraddb03","DSITE3");
            Assert.Fail();
        }
    }
}