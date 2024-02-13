using MarkdownFromHtml.Parsers.MarkdigExtensions;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using VerifyNUnit;

namespace MarkdownFromHtml.Test
{
    public class UnknownTagTests
    {
        private string _testPath;

        public UnknownTagTests()
        {
            _testPath = Path.GetDirectoryName(Assembly.GetCallingAssembly().Location);
        }

        [Test]
        public Task Test1_NoExt_PassThrough()
        {
            var htmltxt = ReadText("Test1.html");

            var manager = new ReplaceManager();
            var converter = new Converter(manager);

            var result = converter.Convert(htmltxt);

            return Verifier.Verify(result);
        }

        [Test]
        public Task Test1_OnlyPipeTable_PassThrough()
        {
            var htmltxt = ReadText("Test1.html");

            var manager = new ReplaceManager();
            manager.Register(new PipeTableParser());
            var converter = new Converter(manager);

            var result = converter.Convert(htmltxt);

            return Verifier.Verify(result);
        }

        [Test]
        public Task Test1_PipeAndGrid_PassThrough()
        {
            var htmltxt = ReadText("Test1.html");

            var manager = new ReplaceManager();
            manager.Register(new GridTableParser());
            manager.Register(new PipeTableParser());
            var converter = new Converter(manager);

            var result = converter.Convert(htmltxt);

            return Verifier.Verify(result);
        }

        [Test]
        public Task Test1_NoExt_Drop()
        {
            var htmltxt = ReadText("Test1.html");

            var manager = new ReplaceManager();
            manager.UnknownTags = UnknownTagsOption.Drop;
            var converter = new Converter(manager);

            var result = converter.Convert(htmltxt);

            return Verifier.Verify(result);
        }

        [Test]
        public Task Test1_OnlyPipeTable_Drop()
        {
            var htmltxt = ReadText("Test1.html");

            var manager = new ReplaceManager();
            manager.UnknownTags = UnknownTagsOption.Drop;
            manager.Register(new PipeTableParser());
            var converter = new Converter(manager);

            var result = converter.Convert(htmltxt);

            return Verifier.Verify(result);
        }

        [Test]
        public Task Test1_PipeAndGrid_Drop()
        {
            var htmltxt = ReadText("Test1.html");

            var manager = new ReplaceManager();
            manager.UnknownTags = UnknownTagsOption.Drop;
            manager.Register(new GridTableParser());
            manager.Register(new PipeTableParser());
            var converter = new Converter(manager);

            var result = converter.Convert(htmltxt);

            return Verifier.Verify(result);
        }


        [Test]
        public void Test1_PipeAndGrid_Raise_Error()
        {
            var htmltxt = ReadText("Test1.html");

            var manager = new ReplaceManager();
            manager.UnknownTags = UnknownTagsOption.Raise;
            manager.Register(new GridTableParser());
            manager.Register(new PipeTableParser());
            var converter = new Converter(manager);

            try
            {
                var result = converter.Convert(htmltxt);
                Assert.Fail("no exception is thrown");
            }
            catch (UnknownTagException e)
            {
                ClassicAssert.AreEqual("center", e.TagName);
            }
        }

        [Test]
        public Task Test1_PipeAndGrid_Raise_Pass()
        {
            var htmltxt = ReadText("Test2.html");

            var manager = new ReplaceManager();
            manager.UnknownTags = UnknownTagsOption.Raise;
            manager.Register(new GridTableParser());
            manager.Register(new PipeTableParser());
            var converter = new Converter(manager);

            var result = converter.Convert(htmltxt);

            return Verifier.Verify(result);
        }


        private string ReadText(string fileName)
        {
            var fullpath = Path.Combine(_testPath, @"..\..\..\UnknownTagTests", fileName);
            return File.ReadAllText(fullpath);
        }
    }
}
