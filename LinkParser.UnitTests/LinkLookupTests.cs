using LinkLookup;
using NUnit.Framework;
using System.Collections.Generic;

namespace LinkParser.UnitTests
{
    public class Tests
    {
        private ILinkLookup linkParser;

        [SetUp]
        public void Setup()
        {
            linkParser = new RegexLinkLookup();
        }

        [Test]
        public void AntlrHtmlParserTest()
        {
            var html = $"<!DOCTYPE html>" +
                            $"<html>" +
                                $"<body>" +
                                $"<h2> External Paths </h2>" +
                                    $"<p> This example links to a page located in the same folder as the current page:</p>" +
                                    $"<a href = \"http://www.w3.com\" > HTML tutorial </a>" +
                                    $"<a href = \"https://www.google.com\" > HTML tutorial </a>" +
                                    $"<a href = \"https://translate.yandex.ru\" > HTML tutorial </a>" +
                                $"</body>" +
                            $" </html>";

            var expectedLinks = new List<string>()
            {
                "http://www.w3.com",
                "https://www.google.com",
                "https://translate.yandex.ru"
            };
            
            var actualLinks = linkParser.GetAllLinks(html);
            Assert.AreEqual(expectedLinks, actualLinks);
        }
    }
}