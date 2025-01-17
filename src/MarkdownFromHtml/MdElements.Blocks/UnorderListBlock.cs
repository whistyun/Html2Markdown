﻿using System.Collections.Generic;
using MarkdownFromHtml.Utils;

namespace MarkdownFromHtml.MdElements.Blocks
{
    public class UnorderListBlock : IMdBlock
    {
        private const string Symbol = "* ";
        private const string Indent = "  ";


        public List<IEnumerable<IMdBlock>> ListItems { get; }


        public UnorderListBlock()
        {
            ListItems = new List<IEnumerable<IMdBlock>>();
        }


        public IEnumerable<string> ToMarkdown()
        {
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

                    foreach (var multiline in blockInItem.ToMarkdown())
                        foreach (var line in multiline.SplitLine())
                        {
                            yield return (isFirst ? Symbol : Indent) + line;

                            isFirst = false;
                        }

                    isRepeated = true;
                }
            }
        }
    }
}
