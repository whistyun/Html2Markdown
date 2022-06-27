﻿using System;
using System.Collections.Generic;
using System.Text;
using Html2Markdown.Utils;

namespace Html2Markdown.MdElements.Blocks
{
    public class CodeBlock : IMdBlock
    {
        public string Lang { get; }
        public string Code { get; }

        public CodeBlock(string lang, string code)
        {
            Lang = lang;

            var buff = new StringBuilder(code.Length);

            int idx = (code.Length > 0 && code[0] == '\n') ? 1 : 0;
            int length = (code.Length > 0 && code[code.Length - 1] == '\n') ? code.Length - 1 : code.Length;


            for (; idx < length; ++idx)
            {
                var c = code[idx];

                if (c == '&' && code.TryDecode(ref idx, out var decoded))
                {
                    buff.Append(decoded);
                }
                else buff.Append(c);
            }

            Code = buff.ToString();
        }


        public IEnumerable<string> ToMarkdown()
        {
            yield return "```" + Lang;

            foreach (var codeline in Code.SplitLine())
                yield return codeline;

            yield return "```";
        }
    }
}
