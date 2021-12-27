using Microsoft.VisualStudio.TestTools.UnitTesting;
using SensibleProgramming.Data.SQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SensibleProgramming.Data.SQL.Tests
{
    [TestClass()]
    public class SQLManagerTests
    {
        [TestMethod()]
        public void GetStoredProcedureParametersTest()
        {
            SQLManager _sqlService = new SQLManager();
            var results = _sqlService.GetStoredProcedureParameters("MO1u1sqlddb06", "RiskRegister", "usp_GetAssessmentDetail");
            Assert.AreEqual(1, results.Count);
        }

        [TestMethod()]
        public void CanParseJson()
        {
            //var jsonString = @"{""Name"":""Rick"",""Company"":""West Wind"",
            //            ""Entered"":""2012-03-16T00:03:33.245-10:00""}";

            //dynamic json = JValue.Parse(jsonString);

            //// values require casting
            //string name = json.Name;
            //string company = json.Company;
            //DateTime entered = json.Entered;
        }

        [TestMethod()]
        public void GetClassFromSqlTest()
        {
            SQLManager _sqlService = new SQLManager();
            var results = _sqlService.GetDTOClassFromSql("MO1u1sqlddb06", "RiskRegister", "ScoreValue");
            Assert.AreEqual(1, results.Length);
        }

        [TestMethod()]
        public void GetJSONClassFromSqlTest()
        {
            SQLManager _sqlService = new SQLManager();
            var results = _sqlService.GetJSONClassFromSql("MO1u1sqlddb06", "RiskRegister", "ScoreValue");
            Assert.AreEqual(1, results.Length);
        }
    }

}