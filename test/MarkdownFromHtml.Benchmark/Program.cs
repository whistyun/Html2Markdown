using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System;
using System.IO;

using H2MConverter = Html2Markdown.Converter;
using MyConverter = MarkdownFromHtml.Converter;
using ReMdConverter = ReverseMarkdown.Converter;

namespace MarkdownFromHtml.Benchmark
{
    class Program
    {
        public static void Main(string[] args)
        {
            var exec = new Executor();

            File.WriteAllText("H2M.md", exec.ConvertByHtml2Markdown());
            File.WriteAllText("ReM.md", exec.ConvertByReverseMarkdown());

            var summary = BenchmarkRunner.Run(typeof(Program).Assembly);
        }
    }

    public class Executor
    {
        private H2MConverter _h2mconverter;
        private MyConverter _myconverter;
        private ReMdConverter _remdconverter;
        private string _markdown;

        public Executor()
        {
            _h2mconverter = new H2MConverter();
            _myconverter = new MyConverter();
            _remdconverter = new ReMdConverter();

            using (var stream = typeof(Program).Assembly.GetManifestResourceStream("MarkdownFromHtml.Benchmark.TargetText.html"))
            using (var reader = new StreamReader(stream))
                _markdown = reader.ReadToEnd();
        }


        [Benchmark]
        public string ConvertByHtml2Markdown()
        {
            return _h2mconverter.Convert(_markdown);
        }

        [Benchmark]
        public string ConvertByMarkdownFromHtml()
        {
            return _myconverter.Convert(_markdown);
        }

        [Benchmark]
        public string ConvertByReverseMarkdown()
        {
            return _remdconverter.Convert(_markdown);
        }
    }
}
