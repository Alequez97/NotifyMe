using LinkLookup.Models;
using LinkLookup.Services;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkLookup.UnitTests
{
    public class UrlServiceTests
    {
        private class MockLinkLookup : ILinkLookup
        {
            public List<Url> GetAllLinks(string text)
            {
                return new List<Url>();
            }
        }

        private List<Url> _mockDownloadedLinks;
        private UrlService _urlService;
        private List<Url> _links;

        [SetUp]
        public void Setup()
        {
            _mockDownloadedLinks = new List<Url>()
            {
                new Url("http://www.w3.com"),
                new Url("https://www.google.com/test"),
                new Url("https://translate.yandex.ru"),
                new Url("/relative/path")
            };
            _urlService = new UrlService(new MockLinkLookup());
            _links = new List<Url>()
            {
                new Url("https://www.google.com")
            };
        }

        [Test]
        public void UrlServiceTest()
        {
            var expectedList = new List<Url>()
            {
                new Url("https://www.google.com/test"),
                new Url("https://www.google.com/relative/path"),
            };

            List<Url> downloadedLinks = new List<Url>();
            foreach (var link in _links)
            {
                downloadedLinks = _urlService.RemoveAlienLinks(_mockDownloadedLinks, link);
                downloadedLinks = _urlService.ConcatenateRelativeLinksWithHost(downloadedLinks, link);
            }

            Assert.AreEqual(expectedList, downloadedLinks);
        }
    }
}
