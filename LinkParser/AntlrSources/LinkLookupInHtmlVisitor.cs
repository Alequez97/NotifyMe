using System;
using System.Collections.Generic;
using Antlr4;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using System.Linq;
using LinkLookup.Models;

namespace LinkLookup.AntlrSources
{
    public sealed class LinkLookupInHtmlVisitor : LinkLookupInHtmlBaseVisitor<List<Url>>
    {
        public override List<Url> VisitHtml([NotNull] LinkLookupInHtmlParser.HtmlContext context)
        {
            var links = context._links.Select(wordToken => new Url(wordToken.Text)).ToList();
            return links;
        }
    }
}