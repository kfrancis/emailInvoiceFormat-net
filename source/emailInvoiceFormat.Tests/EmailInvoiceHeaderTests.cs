using System;
using System.Text.RegularExpressions;
using System.Web;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace EmailInvoiceFormat.Tests
{
    [TestClass]
    public class EmailInvoiceHeaderTests
    {
        [TestMethod]
        public void Does_Respect_Serialization_Settings()
        {
            // Arrange
            InvoiceEmailHeader header = new InvoiceEmailHeader() {
                Issuer = "Service Provider",
                Filename = "bill.pdf",
                InvoiceId = "XF4321-89",
                Paid = true,
                InvoiceDate = DateTime.Parse("2015-01-10T00:00:00+01:00"),
                DueDate = DateTime.Parse("2015-01-30T12:00:00+01:00"),
                PaidDate = DateTime.Parse("2015-01-10T08:35:12+01:00"),
                Amount = 39.90m,
                Currency = "USD"
            };
            var jsonSerializerSettings = new Newtonsoft.Json.JsonSerializerSettings { StringEscapeHandling = Newtonsoft.Json.StringEscapeHandling.EscapeHtml, Formatting = Newtonsoft.Json.Formatting.None };

            // Act
            var result = header.AsJSON(false, jsonSerializerSettings);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(string.IsNullOrWhiteSpace(result));
        }
        [TestMethod]
        public void Can_Serialize_Correctly()
        {
            // Arrange
            InvoiceEmailHeader header = new InvoiceEmailHeader() {
                Issuer = "Service Provider",
                Filename = "bill.pdf",
                InvoiceId = "XF4321-89",
                Paid = true,
                InvoiceDate = DateTime.Parse("2015-01-10T00:00:00+01:00"),
                DueDate = DateTime.Parse("2015-01-30T12:00:00+01:00"),
                PaidDate = DateTime.Parse("2015-01-10T08:35:12+01:00"),
                Amount = 39.90m,
                Currency = "USD"
            };

            // Act
            var headerValue = header.AsJSON();
            var result = headerValue.FromJSON();

            // Assert
            Assert.IsNotNull(headerValue);
            Assert.IsFalse(string.IsNullOrWhiteSpace(headerValue));
            Assert.IsNotNull(result);
            Assert.AreEqual(InvoiceEmailHeader.Version, InvoiceEmailHeader.Version);
            Assert.AreEqual(header.Issuer, result.Issuer);
            Assert.AreEqual(header.Filename, result.Filename);
            Assert.AreEqual(header.InvoiceId, result.InvoiceId);
            Assert.AreEqual(header.Paid, result.Paid);
            Assert.AreEqual(header.InvoiceDate, result.InvoiceDate);
            Assert.AreEqual(header.DueDate, result.DueDate);
            Assert.AreEqual(header.Amount, result.Amount);
            Assert.AreEqual(header.Currency, result.Currency);
        }
    }
}
