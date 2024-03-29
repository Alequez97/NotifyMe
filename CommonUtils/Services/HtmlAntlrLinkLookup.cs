﻿using Antlr4.Runtime;
using CommonUtils.Interfaces;
using CommonUtils.Models;
using CommonUtils.Services.AntlrSources;
using System.Collections.Generic;

namespace CommonUtils.Services
{
    /// <summary>
    /// Class that works only with html string. It can lookup for all links in html.
    /// In all other cases it will returns empty list
    /// </summary>
    public class HtmlAntlrLinkLookup : ILinkLookup
    {
        public List<Url> GetAllLinks(string htmlText)
        {
            var inputStream = new AntlrInputStream(htmlText);
            var linkLexer = new LinkLookupInHtmlLexer(inputStream);
            var commonTokenStream = new CommonTokenStream(linkLexer);
            var linkParser = new LinkLookupInHtmlParser(commonTokenStream);
            var context = linkParser.html();
            var visitor = new LinkLookupInHtmlVisitor();
            var result = visitor.Visit(context);

            return result;
        }
    }
}
