using NUnit.Framework;

using MMABooksProps;
using MMABooksDB;

using DBCommand = MySql.Data.MySqlClient.MySqlCommand;
using System.Data;

using System.Collections.Generic;
using System;
using MySql.Data.MySqlClient;

namespace MMABooksTests
{
    [TestFixture]
    public class ProductDBTests
    {
        ProductDB db;

        [SetUp]
        public void ResetData()
        {
            db = new ProductDB();
            DBCommand command = new DBCommand();
            command.CommandText = "usp_testingResetProductData";
            command.CommandType = CommandType.StoredProcedure;
            db.RunNonQueryProcedure(command);
        }

        [Test]
        public void TestRetrieve()
        {
            ProductProps p = (ProductProps)db.Retrieve(1);
            Assert.AreEqual(1, p.ProductId);
            Assert.AreEqual("Murach's ASP.NET 4 Web Programming with C# 2010", p.Description);
        }

        [Test]
        public void TestRetrieveAll()
        {
            List<ProductProps> list = (List<ProductProps>)db.RetrieveAll();
            Assert.AreEqual(16, list.Count);
        }

        [Test]
        public void TestDelete()
        {
            ProductProps p = (ProductProps)db.Retrieve(1);
            Assert.True(db.Delete(p));
            Assert.Throws<Exception>(() => db.Retrieve(1));
        }



        [Test]
        public void TestUpdate()
        {
            ProductProps p = (ProductProps)db.Retrieve(1);
            p.Description = "Mickey Mouse";
            Assert.True(db.Update(p));
            p = (ProductProps)db.Retrieve(1);
            Assert.AreEqual("Mickey Mouse", p.Description);
        }


        [Test]
        public void TestCreate()
        {
            ProductProps p = new ProductProps();
            p.ProductCode = "OPDF";
            p.Description = "Only You Can Do This";
            p.UnitPrice = 39.9900M;
            p.OnHandQuantity = 5;
            
            db.Create(p);
            ProductProps p2 = (ProductProps)db.Retrieve(p.ProductId);
            Assert.AreEqual(p.GetState(), p2.GetState());
        }

   
    }
}

