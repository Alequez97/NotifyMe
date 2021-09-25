using LinkLookup.Models;
using NUnit.Framework;
using System.Collections.Generic;

namespace LinkLookup.UnitTests
{
    public class LinkLookupTests
    {
        private ILinkLookup linkParser;

        [SetUp]
        public void Setup()
        {
            linkParser = new HtmlAntlrLinkLookup();
        }

        [Test]
        public void AntlrHtmlParserTest()
        {
            var html = $"<!DOCTYPE html>" +
                            $"<html>" +
                                $"<body>" +
                                $"<h2> External Paths </h2>" +
                                    $"<p> This example links to a page located in the same folder as the current page:</p>" +
                                    $"<a href=\"http://www.w3.com\" > HTML tutorial </a>" +
                                    $"<a href=\"https://www.google.com\" > HTML tutorial </a>" +
                                    $"<a href= \"https://translate.yandex.ru\" > HTML tutorial </a>" +
                                    $"<a href =\"/relative/path\" > HTML tutorial </a>" +
                                $"</body>" +
                            $" </html>";

            var html2 = "<a href=\"https://www.habr.com/sanja-test\">HTML tutorial</a>";

            var expectedLinks = new List<Url>()
            {
                new Url("http://www.w3.com"),
                new Url("https://www.google.com"),
                new Url("https://translate.yandex.ru"),
                new Url("/relative/path")
            };
            
            var actualLinks = linkParser.GetAllLinks(html);
            Assert.AreEqual(expectedLinks, actualLinks);

            Assert.AreEqual(1, linkParser.GetAllLinks(html2).Count);
            Assert.AreEqual("https://www.habr.com/sanja-test", linkParser.GetAllLinks(html2)[0].ToString());
        }
    }
}