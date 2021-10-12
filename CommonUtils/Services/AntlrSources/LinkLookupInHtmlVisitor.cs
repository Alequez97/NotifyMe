using Antlr4.Runtime.Misc;
using CommonUtils.Models;
using System.Collections.Generic;
using System.Linq;

namespace CommonUtils.Services.AntlrSources
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