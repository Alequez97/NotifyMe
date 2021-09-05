using Antlr4.Runtime;
using LinkLookup.AntlrSources;
using System;
using System.Collections.Generic;
using System.Text;

namespace LinkLookup
{
    public class AntlrLinkLookup : ILinkLookup
    {
        public AntlrLinkLookup()
        {
            
        }

        public List<string> GetAllLinks(string text)
        {
            var inputStream = new AntlrInputStream(text);
            var lexer = new HtmlLinkExtractorLexer(inputStream);
            //lexer.RemoveErrorListeners();
            //lexer.AddErrorListener(new CustomErrorListener());
            var tokens = new CommonTokenStream(lexer);
            var parser = new HtmlLinkExtractorParser(tokens);

            var visitor = new HtmlLinkVisitor();
            var context = parser.html();

            return visitor.Visit(context);
        }
    }
}
