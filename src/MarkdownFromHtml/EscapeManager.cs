using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkdownFromHtml
{
    public class EscapeManager
    {
        private HashSet<char> _EscapeCharacter = new();
        private Dictionary<char, List<Replace>> _EscapeText = new();

        public void Register(char c) => _EscapeCharacter.Add(c);

        public void Register(string text)
        {
            char first = text[0];
            if (!_EscapeText.TryGetValue(first, out var list))
            {
                list = new List<Replace>();
                _EscapeText.Add(first, list);
            }

            list.Add(new Replace(text));
        }

        public bool IsTarget(string text, ref int index, out string escaped)
        {
            char first = text[index];

            if (_EscapeCharacter.Contains(first))
            {
                escaped = "\\" + first;
                return true;
            }

            if (_EscapeText.TryGetValue(first, out var list))
            {
                foreach (var tgtRep in list)
                {
                    string tgt = tgtRep.Target;

                    if (index + tgt.Length > text.Length) continue;

                    for (int i = 0; i < tgt.Length; ++i)
                    {
                        if (tgt[i] != text[index + i])
                            goto unmatch;
                    }

                    // match
                    index += tgt.Length - 1;
                    escaped = tgtRep.Escaped;
                    return true;

                unmatch:
                    continue;
                }

            }

            escaped = "";
            return false;
        }

        class Replace
        {
            public string Target { get; }
            public string Escaped { get; }

            public Replace(string target)
            {
                Target = target;

                var buff = new StringBuilder(target.Length * 2);
                foreach (var c in Target)
                    buff.Append('\\').Append(c);

                Escaped = buff.ToString();
            }
        }
    }
}
