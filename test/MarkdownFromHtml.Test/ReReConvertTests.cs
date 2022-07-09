using Markdig;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.DocAsCode.MarkdigEngine.Extensions;
using System.Reflection;
using HtmlAgilityPack;
using System.Text.RegularExpressions;
using MarkdownFromHtml.Parsers;
using MarkdownFromHtml.Parsers.MarkdigExtensions;
using VerifyNUnit;
using System.Runtime.CompilerServices;

namespace MarkdownFromHtml.Test
{
    public class ReReConvertTests
    {
        private MarkdownPipeline _pipe;
        private string _testPath;

        public ReReConvertTests()
        {
            var builder = new MarkdownPipelineBuilder();
            builder.UseAdvancedExtensions();

            _pipe = builder.Build();
            _testPath = Path.GetDirectoryName(Assembly.GetCallingAssembly().Location);
        }


        [Test]
        public void List()
        {
            var mdtxt = ReadText();
            var htmltxt = Markdown.ToHtml(mdtxt);

            var converter = new Converter();
            var reMdtxt = converter.Convert(htmltxt);

            var reHtmltxt = Markdown.ToHtml(reMdtxt);
            Assert.AreEqual(Normalize(htmltxt), Normalize(reHtmltxt));
        }

        [Test]
        public void Code()
        {
            var mdtxt = ReadText();
            var htmltxt = MarkdownToHtml(mdtxt);

            var converter = new Converter();
            var reMdtxt = converter.Convert(htmltxt);

            var reHtmltxt = MarkdownToHtml(reMdtxt);
            Assert.AreEqual(Normalize(htmltxt), Normalize(reHtmltxt));
        }

        [Test]
        public void PipeTable()
        {
            var mdtxt = ReadText();
            var htmltxt = MarkdownToHtml(mdtxt);

            var manager = new ReplaceManager();
            manager.Register(new CiteParser());
            manager.Register(new DeletedParser());
            manager.Register(new FigureParser());
            manager.Register(new GridTableParser());
            manager.Register(new InsertedParser());
            manager.Register(new MarkedParser());
            manager.Register(new PipeTableParser());
            manager.Register(new SubscriptParser());
            manager.Register(new SuperscriptParser());
            var converter = new Converter(manager);

            var reMdtxt = converter.Convert(htmltxt);

            WriteText("PipeTable.temp.md", reMdtxt);

            var reHtmltxt = MarkdownToHtml(reMdtxt);

            Assert.AreEqual(Normalize(htmltxt), Normalize(reHtmltxt));
        }

        [Test]
        public void GridTable()
        {
            var mdtxt = ReadText();

            var htmltxt = MarkdownToHtml(mdtxt);

            var manager = new ReplaceManager();
            manager.Register(new GridTableParser());
            var converter = new Converter(manager);

            var reMdtxt = converter.Convert(htmltxt);

            WriteText("GridTable.temp.md", reMdtxt);

            var reHtmltxt = MarkdownToHtml(reMdtxt);

            Assert.AreEqual(Normalize(htmltxt), Normalize(reHtmltxt));
        }

        [Test]
        public void EmphasisExtra()
        {
            var mdtxt = ReadText();

            var htmltxt = MarkdownToHtml(mdtxt);

            var manager = new ReplaceManager();
            manager.Register(new CiteParser());
            manager.Register(new DeletedParser());
            manager.Register(new FigureParser());
            manager.Register(new GridTableParser());
            manager.Register(new InsertedParser());
            manager.Register(new MarkedParser());
            manager.Register(new PipeTableParser());
            manager.Register(new SubscriptParser());
            manager.Register(new SuperscriptParser());
            var converter = new Converter(manager);

            var reMdtxt = converter.Convert(htmltxt);

            WriteText("EmphasisExtra.temp.md", reMdtxt);

            var reHtmltxt = MarkdownToHtml(reMdtxt);

            Assert.AreEqual(Normalize(htmltxt), Normalize(reHtmltxt));
        }

        [Test]
        public void FigureFooterAndCite()
        {
            var mdtxt = ReadText();

            var htmltxt = MarkdownToHtml(mdtxt);

            var manager = new ReplaceManager();
            manager.Register(new CiteParser());
            manager.Register(new DeletedParser());
            manager.Register(new FigureParser());
            manager.Register(new FooterParser());
            manager.Register(new GridTableParser());
            manager.Register(new InsertedParser());
            manager.Register(new MarkedParser());
            manager.Register(new PipeTableParser());
            manager.Register(new SubscriptParser());
            manager.Register(new SuperscriptParser());
            var converter = new Converter(manager);

            var reMdtxt = converter.Convert(htmltxt);

            WriteText("FigureFooterAndCite.temp.md", reMdtxt);

            var reHtmltxt = MarkdownToHtml(reMdtxt);

            Assert.AreEqual(Normalize(htmltxt), Normalize(reHtmltxt));
        }



        private string ReadText([CallerMemberName] string fileName = null)
        {
            var fullpath = Path.Combine(_testPath, @"..\..\..\ReReConvertTests", fileName + ".md");
            return File.ReadAllText(fullpath);
        }

        private void WriteText(string fileName, string content)
        {
            var fullpath = Path.Combine(_testPath, @"..\..\..\ReReConvertTests", fileName);
            File.WriteAllText(fullpath, content);
        }

        private string MarkdownToHtml(string md)
        {
            return Markdown.ToHtml(md, _pipe);
        }

        private string Normalize(string text)
        {
            var buff = new StringBuilder();

            var spcPtn = new Regex(@"[ \r\n\t]+", RegexOptions.Compiled);
            var prePtn = new Regex(@"<pre([ \t][^>]+)?>.*?</pre>", RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.RightToLeft);

            int idx = 0;
            for (; ; )
            {
                var match = prePtn.Match(text, idx);
                if (!match.Success)
                {
                    var preTxt = text.Substring(idx);
                    buff.Append(spcPtn.Replace(preTxt, " "));

                    break;
                }
                else
                {
                    var preTxt = text.Substring(idx, match.Index - idx);
                    buff.Append(spcPtn.Replace(preTxt, " "));

                    buff.Append(match.Value);

                    idx = match.Index + match.Length;
                }
            }

            return buff.ToString();
        }
    }
}
