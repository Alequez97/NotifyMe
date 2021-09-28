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
        private List<Url> _userSubscribedOnlinks;

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
            _userSubscribedOnlinks = new List<Url>()
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
            foreach (var link in _userSubscribedOnlinks)
            {
                downloadedLinks = _urlService.RemoveAlienLinks(_mockDownloadedLinks, link);
                downloadedLinks = _urlService.ConcatenateRelativeLinksWithHost(downloadedLinks, link);
            }

            Assert.AreEqual(expectedList, downloadedLinks);
        }

        [Test]
        public void UrlServiceRemoveBySubstringTest1()
        {
            var expectedList = new List<Url>()
            {
                new Url("https://translate.yandex.ru"),
                new Url("/relative/path")
            };

            var actualList = _urlService.RemoveLinksContainingSubstrings(_mockDownloadedLinks, new List<string> { "com" });

            Assert.AreEqual(expectedList, actualList);
        }

        [Test]
        public void UrlServiceRemoveBySubstringTest2()
        {
            var expectedList = new List<Url>()
            {
                new Url("https://translate.yandex.ru"),
            };

            var actualList = _urlService.RemoveLinksContainingSubstrings(_mockDownloadedLinks, new List<string> { "com", "path" });

            Assert.AreEqual(expectedList, actualList);
        }
    }
}
