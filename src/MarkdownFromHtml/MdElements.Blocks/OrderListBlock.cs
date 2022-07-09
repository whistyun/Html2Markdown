using System;
using System.Collections.Generic;
using System.Linq;
using MarkdownFromHtml.Utils;

namespace MarkdownFromHtml.MdElements.Blocks
{
    public class OrderListBlock : IMdBlock
    {
        private const string Indent = "   ";


        public int Start { get; set; }
        public List<IEnumerable<IMdBlock>> ListItems { get; }


        public OrderListBlock()
        {
            Start = 1;
            ListItems = new List<IEnumerable<IMdBlock>>();
        }


        public IEnumerable<string> ToMarkdown()
        {
            int counter = Start;

            foreach (var item in ListItems)
            {
                bool isFirst = true;
                bool isRepeated = false;

                foreach (var blockInItem in item)
                {
                    if (isRepeated)
                    {
                        // insert empty line
                        yield return "";
                    }

                    foreach (var line in blockInItem.ToMarkdown().SelectMany(ln => ln.SplitLine()))
                    {
                        yield return (isFirst ? $"{counter++}. " : Indent) + line;

                        isFirst = false;
                    }

                    isRepeated = true;
                }
            }
        }
    }
}
