using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Running;
using System;
using System.IO;
using System.Text;
using H2MConverter = Html2Markdown.Converter;
using MyConverter = MarkdownFromHtml.Converter;
using ReMdConverter = ReverseMarkdown.Converter;

namespace MarkdownFromHtml.Benchmark
{
    class Program
    {
        public static void Main(string[] args)
        {
            var summaries = BenchmarkRunner.Run(typeof(Program).Assembly);
            var exporter = AsciiDocExporter.Default;

            var logger = new StringLogger();

            foreach (var summary in summaries)
            {
                logger.WriteLine("# " + summary.Title);
                logger.WriteLine();
                exporter.ExportToLog(summary, logger);
                logger.WriteLine();
                logger.WriteLine();
            }

            File.WriteAllText("summary.md", logger.ToString());
        }
    }

    class StringLogger : ILogger
    {
        private StringBuilder _builder = new StringBuilder();

        public string Id => "StringLogger";
        public int Priority => 0;

        public StringLogger() { }

        public void Flush()
        {
        }

        public void Write(LogKind logKind, string text)
        {
            _builder.Append(text);
        }

        public void WriteLine()
        {
            _builder.AppendLine();
        }

        public void WriteLine(LogKind logKind, string text)
        {
            _builder.AppendLine(text);
        }

        public string ToString() => _builder.ToString();
    }


    [SimpleJob(RuntimeMoniker.Net472)]
    [SimpleJob(RuntimeMoniker.NetCoreApp21)]
    [SimpleJob(RuntimeMoniker.NetCoreApp30)]
    [SimpleJob(RuntimeMoniker.Net50)]
    public class InitializeAndConvert
    {
        private string _html;

        public InitializeAndConvert()
        {
            using (var stream = typeof(Program).Assembly.GetManifestResourceStream("MarkdownFromHtml.Benchmark.TargetText.html"))
            using (var reader = new StreamReader(stream))
                _html = reader.ReadToEnd();
        }

        [Benchmark]
        public string Html2Markdown()
        {
            var cv = new H2MConverter();
            return cv.Convert(_html);
        }

        [Benchmark]
        public string MarkdownFromHtml()
        {
            var cv = new MyConverter();
            return cv.Convert(_html);
        }

        [Benchmark]
        public string ReverseMarkdown()
        {
            var cv = new ReMdConverter();
            return cv.Convert(_html);
        }
    }

    [SimpleJob(RuntimeMoniker.Net472)]
    [SimpleJob(RuntimeMoniker.NetCoreApp21)]
    [SimpleJob(RuntimeMoniker.NetCoreApp30)]
    [SimpleJob(RuntimeMoniker.Net50)]
    public class Convert
    {
        private H2MConverter _h2mconverter;
        private MyConverter _myconverter;
        private ReMdConverter _remdconverter;
        private string _html;

        public Convert()
        {
            _h2mconverter = new H2MConverter();
            _myconverter = new MyConverter();
            _remdconverter = new ReMdConverter();

            using (var stream = typeof(Program).Assembly.GetManifestResourceStream("MarkdownFromHtml.Benchmark.TargetText.html"))
            using (var reader = new StreamReader(stream))
                _html = reader.ReadToEnd();
        }

        [Benchmark]
        public string Html2Markdown()
        {
            return _h2mconverter.Convert(_html);
        }

        [Benchmark]
        public string MarkdownFromHtml()
        {
            return _myconverter.Convert(_html);
        }

        [Benchmark]
        public string ReverseMarkdown()
        {
            return _remdconverter.Convert(_html);
        }
    }
}
