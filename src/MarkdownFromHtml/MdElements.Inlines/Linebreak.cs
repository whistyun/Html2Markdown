using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkdownFromHtml.MdElements.Inlines
{
    public class Linebreak : IMdInline
    {
        public void TrimStart() { }

        public void TrimEnd() { }

        public bool EndsWithSpace() => false;

        public string ToMarkdown() => "  \n";
    }
}
