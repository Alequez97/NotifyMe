using System;
using System.Collections.Generic;
using Antlr4;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using System.Linq;

namespace LinkLookup.AntlrSources
{
    public sealed class LinkLookupInHtmlVisitor : LinkLookupInHtmlBaseVisitor<List<string>>
    {
        public override List<string> VisitHtml([NotNull] LinkLookupInHtmlParser.HtmlContext context)
        {
            var links = context._links.Select(wordToken => wordToken.Text).ToList();
            return links;
        }
    }
}