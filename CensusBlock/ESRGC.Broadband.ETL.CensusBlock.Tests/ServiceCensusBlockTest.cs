using ESRGC.Broadband.ETL.CensusBlock.Domain.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ESRGC.Broadband.ETL.CensusBlock.Tests
{
    
    
    /// <summary>
    ///This is a test class for ServiceCensusBlockTest and is intended
    ///to contain all ServiceCensusBlockTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ServiceCensusBlockTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext {
            get {
                return testContextInstance;
            }
            set {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for ServiceCensusBlock Constructor
        ///</summary>
        [TestMethod()]
        public void ServiceCensusBlockConstructorTest() {
            ServiceCensusBlock target = new ServiceCensusBlock();
            Assert.Inconclusive("TODO: Implement code to verify target");
        }

        /// <summary>
        ///A test for BLOCKID
        ///</summary>
        [TestMethod()]
        public void BLOCKIDTest() {
            ServiceCensusBlock target = new ServiceCensusBlock(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.BLOCKID = expected;
            actual = target.BLOCKID;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for COUNTYFIPS
        ///</summary>
        [TestMethod()]
        public void COUNTYFIPSTest() {
            ServiceCensusBlock target = new ServiceCensusBlock(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.COUNTYFIPS = expected;
            actual = target.COUNTYFIPS;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for DBANAME
        ///</summary>
        [TestMethod()]
        public void DBANAMETest() {
            ServiceCensusBlock target = new ServiceCensusBlock(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.DBANAME = expected;
            actual = target.DBANAME;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for FRN
        ///</summary>
        [TestMethod()]
        public void FRNTest() {
            ServiceCensusBlock target = new ServiceCensusBlock(); // TODO: Initialize to an appropriate value
            string expected = "0001234567"; // TODO: Initialize to an appropriate value
            string actual = "0001234567";
            target.FRN = "1234567";
            actual = target.FRN;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for FULLFIPSID
        ///</summary>
        [TestMethod()]
        public void FULLFIPSIDTest() {
            ServiceCensusBlock target = new ServiceCensusBlock(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.FULLFIPSID = expected;
            actual = target.FULLFIPSID;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for MAXADDOWN
        ///</summary>
        [TestMethod()]
        public void MAXADDOWNTest() {
            ServiceCensusBlock target = new ServiceCensusBlock(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.MAXADDOWN = expected;
            actual = target.MAXADDOWN;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for MAXADUP
        ///</summary>
        [TestMethod()]
        public void MAXADUPTest() {
            ServiceCensusBlock target = new ServiceCensusBlock(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.MAXADUP = expected;
            actual = target.MAXADUP;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for PROVNAME
        ///</summary>
        [TestMethod()]
        public void PROVNAMETest() {
            ServiceCensusBlock target = new ServiceCensusBlock(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.PROVNAME = expected;
            actual = target.PROVNAME;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for Provider_Type
        ///</summary>
        [TestMethod()]
        public void Provider_TypeTest() {
            ServiceCensusBlock target = new ServiceCensusBlock(); // TODO: Initialize to an appropriate value
            short expected = 0; // TODO: Initialize to an appropriate value
            short actual;
            target.Provider_Type = expected;
            actual = target.Provider_Type;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for STATEFIPS
        ///</summary>
        [TestMethod()]
        public void STATEFIPSTest() {
            ServiceCensusBlock target = new ServiceCensusBlock(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.STATEFIPS = expected;
            actual = target.STATEFIPS;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for ServiceCensusBlockID
        ///</summary>
        [TestMethod()]
        public void ServiceCensusBlockIDTest() {
            ServiceCensusBlock target = new ServiceCensusBlock(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.ServiceCensusBlockID = expected;
            actual = target.ServiceCensusBlockID;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for Submission
        ///</summary>
        [TestMethod()]
        public void SubmissionTest() {
            ServiceCensusBlock target = new ServiceCensusBlock(); // TODO: Initialize to an appropriate value
            Submission expected = null; // TODO: Initialize to an appropriate value
            Submission actual;
            target.Submission = expected;
            actual = target.Submission;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for SubmissionID
        ///</summary>
        [TestMethod()]
        public void SubmissionIDTest() {
            ServiceCensusBlock target = new ServiceCensusBlock(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.SubmissionID = expected;
            actual = target.SubmissionID;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for TRACT
        ///</summary>
        [TestMethod()]
        public void TRACTTest() {
            ServiceCensusBlock target = new ServiceCensusBlock(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.TRACT = expected;
            actual = target.TRACT;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for TRANSTECH
        ///</summary>
        [TestMethod()]
        public void TRANSTECHTest() {
            ServiceCensusBlock target = new ServiceCensusBlock(); // TODO: Initialize to an appropriate value
            short expected = 0; // TODO: Initialize to an appropriate value
            short actual;
            target.TRANSTECH = expected;
            actual = target.TRANSTECH;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for TYPICDOWN
        ///</summary>
        [TestMethod()]
        public void TYPICDOWNTest() {
            ServiceCensusBlock target = new ServiceCensusBlock(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.TYPICDOWN = expected;
            actual = target.TYPICDOWN;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for TYPICUP
        ///</summary>
        [TestMethod()]
        public void TYPICUPTest() {
            ServiceCensusBlock target = new ServiceCensusBlock(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.TYPICUP = expected;
            actual = target.TYPICUP;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}
