﻿using HtmlAgilityPack;
using System;
using System.Collections.Generic;

namespace Html2Markdown.Utils
{
    static class NodeCollectionExt
    {
        public static List<HtmlNode> SkipComment(this HtmlNodeCollection list)
        {
            var count = list.Count;

            var store = new List<HtmlNode>(count);

            for (var i = 0; i < count; ++i)
            {
                var e = list[i];
                if (e.IsComment()) continue;

                store.Add(e);
            }

            return store;
        }

        public static bool IsComment(this HtmlNode node) => node is HtmlCommentNode;

        public static List<HtmlNode> CollectTag(this HtmlNodeCollection list)
        {
            var count = list.Count;

            var store = new List<HtmlNode>(count);

            for (var i = 0; i < count; ++i)
            {
                var e = list[i];
                if (e.NodeType != HtmlNodeType.Element) continue;

                store.Add(e);
            }

            return store;
        }

        public static List<HtmlNode> CollectTag(this HtmlNodeCollection list, string tagName)
        {
            var count = list.Count;

            var store = new List<HtmlNode>(count);

            for (var i = 0; i < count; ++i)
            {
                var e = list[i];
                if (e.NodeType != HtmlNodeType.Element) continue;
                if (!string.Equals(e.Name, tagName, StringComparison.OrdinalIgnoreCase)) continue;

                store.Add(e);
            }

            return store;
        }

        public static bool TryCastTextNode(this HtmlNodeCollection list, out List<HtmlTextNode> texts)
        {
            var count = list.Count;

            texts = new List<HtmlTextNode>(count);

            for (var i = 0; i < count; ++i)
            {
                var e = list[i];

                if (e is HtmlTextNode txtNd)
                {
                    texts.Add(txtNd);
                    continue;
                }

                if (e.IsComment())
                {
                    continue;
                }

                return false;
            }

            return true;
        }
    }
}
