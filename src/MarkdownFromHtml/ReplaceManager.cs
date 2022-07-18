using MarkdownFromHtml.MdElements;
using MarkdownFromHtml.MdElements.Blocks;
using MarkdownFromHtml.MdElements.Inlines;
using MarkdownFromHtml.Parsers;
using MarkdownFromHtml.Utils;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;

namespace MarkdownFromHtml
{
    public class ReplaceManager
    {
        private Dictionary<string, List<ISimpleTagParser>> _bindParsers;
        private List<ITagParser> _parsers;
        private EscapeManager _escape;

        public ReplaceManager()
        {
            _bindParsers = new();
            _parsers = new();
            _escape = new();

            UnknownTags = UnknownTagsOption.PassThrough;

            Register(new TagIgnoreParser());
            Register(new CommentParsre());

            Register(new BoldParser());
            Register(new ItalicParser());
            Register(new HyperlinkParser());
            Register(new ImageParser());
            Register(new CodeSpanParser());

            Register(new HeadingParser());
            Register(new OrderListParser());
            Register(new UnorderListParser());
            Register(new ParagraphParser());
            Register(new TextNodeParser());
            Register(new LineBreakParser());
            Register(new BlockquoteParser());
            Register(new CodeBlockParser());
            Register(new HorizontalRuleParser());
        }

        public UnknownTagsOption UnknownTags { get; set; }

        public EscapeManager Escape { get; }

        public void Register(ISimpleTagParser parser)
        {
            foreach (var tag in parser.SupportTag)
            {
                if (!_bindParsers.TryGetValue(tag.ToLower(), out var list))
                {
                    list = new();
                    _bindParsers.Add(tag.ToLower(), list);
                }

                InsertWithPriority(list, parser);
            }

            if (parser is IRequestEscapeCharacter escapeReqChr)
                RegisterRequestEscapeCharacter(escapeReqChr);

            if (parser is IRequestEscapeString escapeReqStr)
                RegisterRequestEscapeString(escapeReqStr);
        }

        public void Register(ITagParser parser)
        {
            if (parser is ISimpleTagParser simpleParser)
                Register(simpleParser);

            else
                InsertWithPriority(_parsers, parser);

            if (parser is IRequestEscapeCharacter escapeReq)
                RegisterRequestEscapeCharacter(escapeReq);
        }

        private void InsertWithPriority<T>(List<T> list, T parser) where T : ITagParser
        {
            int parserPriority = parser.GetPriority();

            int count = list.Count;
            for (int i = 0; i < count; ++i)
            {
                var elmnt = list[i];

                if (parserPriority <= elmnt.GetPriority())
                {
                    list.Insert(i, parser);
                    return;
                }
            }
            list.Add(parser);
        }

        private void RegisterRequestEscapeCharacter(IRequestEscapeCharacter req)
        {
            foreach (var ch in req.EscapeCharTarget)
                _escape.Register(ch);
        }

        private void RegisterRequestEscapeString(IRequestEscapeString req)
        {
            foreach (var ch in req.EscapeStringTarget)
                _escape.Register(ch);
        }

        /// <summary>
        /// Convert a html tag list to an element of markdown.
        /// </summary>
        public IEnumerable<IMdBlock> Parse(string htmldoc)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(htmldoc);

            return Parse(doc);
        }

        /// <summary>
        /// Convert a html tag list to an element of markdown.
        /// </summary>
        public IEnumerable<IMdBlock> Parse(HtmlDocument doc)
        {
            var contents = new List<HtmlNode>();

            var head = PickBodyOrHead(doc.DocumentNode, "head");
            if (head is not null)
                contents.AddRange(head.ChildNodes.SkipComment());

            var body = PickBodyOrHead(doc.DocumentNode, "body");
            if (body is not null)
                contents.AddRange(body.ChildNodes.SkipComment());

            if (contents.Count == 0)
            {
                var root = doc.DocumentNode.ChildNodes.SkipComment();

                if (root.Count == 1 && string.Equals(root[0].Name, "html", StringComparison.OrdinalIgnoreCase))
                    contents.AddRange(root[0].ChildNodes.SkipComment());
                else
                    contents.AddRange(root);
            }

            var jaggingResult = ParseJagging(contents);

            return Grouping(jaggingResult);
        }

        /// <summary>
        /// Convert a html tag list to an element of markdown.
        /// Inline elements are aggreated into paragraph.
        /// </summary>
        public IEnumerable<IMdBlock> ParseAndGroup(HtmlNodeCollection nodes)
        {
            var jaggingResult = ParseJagging(nodes);

            return Grouping(jaggingResult);
        }

        /// <summary>
        /// Convert a html tag to an element of markdown.
        /// this result contains a block element and an inline element.
        /// </summary>
        public IEnumerable<IMdElement> ParseJagging(IEnumerable<HtmlNode> nodes)
        {
            bool isPrevBlock = true;
            IMdElement? lastElement = EmptyBlock.Instance;

            foreach (var node in nodes)
            {
                if (node.IsComment())
                    continue;

                // remove blank text between the blocks.
                if (isPrevBlock
                    && node is HtmlTextNode txt
                    && String.IsNullOrWhiteSpace(txt.Text))
                    continue;

                foreach (var element in ParseIt(node))
                {
                    lastElement = element;
                    yield return element;
                }

                isPrevBlock = lastElement is IMdBlock;
            }
        }

        /// <summary>
        /// Convert a html tag to an element of markdown.
        /// Only tag node and text node are accepted.
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private IEnumerable<IMdElement> ParseIt(HtmlNode node)
        {
            if (_bindParsers.TryGetValue(node.Name.ToLower(), out var binds))
            {
                foreach (var bind in binds)
                {
                    if (bind.TryReplace(node, this, out var parsed))
                    {
                        return parsed;
                    }
                }
            }

            foreach (var parser in _parsers)
            {
                if (parser.TryReplace(node, this, out var parsed))
                {
                    return parsed;
                }
            }


            switch (UnknownTags)
            {
                case UnknownTagsOption.PassThrough:
                    return HtmlUtils.IsBlockTag(node.Name) ?
                        new[] { new HtmlBlock(node.OuterHtml) } :
                        new[] { new HtmlInline(node.OuterHtml) };

                case UnknownTagsOption.Drop:
                    return Array.Empty<IMdElement>();

                case UnknownTagsOption.Bypass:
                    return ParseJagging(node.ChildNodes);

                case UnknownTagsOption.Raise:
                default:
                    throw new UnknownTagException(node);
            }
        }

        /// <summary>
        /// Convert IMdElement to IMdBlock.
        /// Inline elements are aggreated into paragraph.
        /// </summary>
        public IEnumerable<IMdBlock> Grouping(IEnumerable<IMdElement> elements)
        {
            bool Group(IList<IMdInline> inlines)
            {
                // trim whiltepace plain

                while (inlines.Count > 0)
                {
                    if (inlines[0] is Plain plain
                        && String.IsNullOrWhiteSpace(plain.Content))
                    {
                        inlines.RemoveAt(0);
                    }
                    else break;
                }

                while (inlines.Count > 0)
                {
                    if (inlines[inlines.Count - 1] is Plain plain
                        && String.IsNullOrWhiteSpace(plain.Content))
                    {
                        inlines.RemoveAt(inlines.Count - 1);
                    }
                    else break;
                }

                using (var list = inlines.GetEnumerator())
                {
                    IMdInline? prev = null;

                    if (list.MoveNext())
                    {
                        prev = list.Current;
                        prev.TrimStart();

                        while (list.MoveNext())
                        {
                            var now = list.Current;

                            if (now is Linebreak)
                            {
                                prev.TrimEnd();

                                if (list.MoveNext())
                                {
                                    now = list.Current;
                                    now.TrimStart();
                                }
                            }
                            else if (prev.EndsWithSpace())
                            {
                                now.TrimStart();
                            }

                            prev = now;
                        }
                    }

                    prev?.TrimEnd();
                }
                return inlines.Count > 0;
            }

            List<IMdInline> stored = new();
            foreach (var e in elements)
            {
                if (e is IMdInline inline)
                {
                    stored.Add(inline);
                    continue;
                }

                // grouping inlines
                if (stored.Count != 0)
                {
                    if (Group(stored))
                        yield return new Paragraph(stored.ToArray());

                    stored.Clear();
                }

                yield return (IMdBlock)e;
            }

            if (stored.Count != 0)
                if (Group(stored))
                    yield return new Paragraph(stored.ToArray());
        }

        private HtmlNode? PickBodyOrHead(HtmlNode documentNode, string headOrBody)
        {
            // html?
            foreach (var child in documentNode.ChildNodes)
            {
                if (child.Name == HtmlTextNode.HtmlNodeTypeNameText
                    || child.Name == HtmlTextNode.HtmlNodeTypeNameComment)
                    continue;

                switch (child.Name.ToLower())
                {
                    case "html":
                        // body? head?
                        foreach (var descendants in child.ChildNodes)
                        {
                            if (descendants.Name == HtmlTextNode.HtmlNodeTypeNameText
                                || descendants.Name == HtmlTextNode.HtmlNodeTypeNameComment)
                                continue;
                            switch (descendants.Name.ToLower())
                            {
                                case "head":
                                    if (headOrBody == "head")
                                        return descendants;
                                    break;

                                case "body":
                                    if (headOrBody == "body")
                                        return descendants;
                                    break;

                                default:
                                    return null;
                            }
                        }
                        break;

                    case "head":
                        if (headOrBody == "head")
                            return child;
                        break;

                    case "body":
                        if (headOrBody == "body")
                            return child;
                        break;

                    default:
                        return null;
                }
            }
            return null;
        }
    }
}
