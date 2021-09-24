using LinkLookup.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkLookup.UnitTests
{
    public class UrlModelTests
    {
        private Url url1;
        private Url url2;
        private Url url3;
        private Url url4;

        [SetUp]
        public void Setup()
        {
            url1 = new Url("https://www.microsoft.com");
            url2 = new Url("some/relative/path");
            url3 = new Url("http://habr.com");
            url4 = new Url("https://microsoft.com/some/relative/path");
        }

        [Test]
        public void UrlClassTest()
        {
            Assert.AreEqual("https", url1.Scheme);
            Assert.AreEqual("microsoft.com", url1.Host);
            Assert.IsTrue(url1.IsAbsoluteUrl);

            Assert.AreEqual(string.Empty, url2.Scheme);
            Assert.AreEqual(string.Empty, url2.Host);
            Assert.IsFalse(url2.IsAbsoluteUrl);

            Assert.AreEqual("http", url3.Scheme);
            Assert.AreEqual("habr.com", url3.Host);
            Assert.IsTrue(url3.IsAbsoluteUrl);

            Assert.IsTrue(url1.Host.Equals(url4.Host), $"{url1} host is equals with {url4} host");
        }
    }
}
