using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using ReverseMarkdown.Converters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;
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
                logger.WriteLine("# Benchmarking" + summary.Title);
                logger.WriteLine();

                logger.WriteLine("## Environments");

                logger.WriteLine("```");
                foreach (string infoLine in summary.HostEnvironmentInfo.ToFormattedString())
                {
                    logger.WriteLineInfo(infoLine);
                }
                logger.WriteLineInfo(summary.AllRuntimes);
                logger.WriteLine("```");
                logger.WriteLine();

                logger.WriteLine("## Results");
                var table = summary.Table;
                var records = table.FullContent
                                   .Select(line => line.Select((element, index) => (element, index))
                                                       .ToDictionary(
                                                            entry => table.FullHeader[entry.index],
                                                            entry => entry.element))
                                   .ToList();

                var span = table.FullHeader
                                .Select((element, index) => (element, index))
                                .ToDictionary(
                                   entry => entry.element,
                                   entry => table.FullContent.Max(line => line[entry.index].Length + 2));
                span["Name"] = Math.Max(" Number of tags ".Length, span["Name"]);


                var names = records.Select(dic => dic["Name"]).Distinct().ToArray();
                var methods = records.Select(dic => dic["Method"]).Where(m => m != "MarkdownFromHtml").Distinct().ToArray();

                logger.Write("|" + Rpad(" Number of tags ", span["Name"]) + "|");
                logger.Write(Rpad(" Method ", span["Method"]));
                logger.Write("|");
                foreach (var col in new[] { "Mean", "Error", "StdDev" })
                {
                    logger.Write(Rpad($" {col} ", span[col]));
                    logger.Write("|");
                }
                logger.WriteLine();

                logger.WriteLine($"|--|--|-:|-:|-:|");

                foreach (var name in names)
                {
                    Log(name, "MarkdownFromHtml", true);
                    foreach (var method in methods)
                    {
                        Log(name, method, false);
                    }
                }

                logger.WriteLine();


                void Log(string name, string method, bool logName)
                {
                    if (logName)
                    {
                        logger.Write($"|{Rpad(" " + name + " ", span["Name"])}|{Rpad(" " + method + " ", span["Method"])}|");
                    }
                    else
                    {
                        logger.Write($"|{Rpad("", span["Name"])}|{Rpad(" " + method + " ", span["Method"])}|");
                    }

                    var myrec = records.First(rec => rec["Name"] == name && rec["Method"] == method);

                    foreach (var col in new[] { "Mean", "Error", "StdDev" })
                    {
                        logger.Write(Lpad(myrec[col], span[col]));
                        logger.Write("|");
                    }
                    logger.WriteLine();
                }

                logger.WriteLine();
                logger.WriteLine();
                exporter.ExportToLog(summary, logger);
                logger.WriteLine();
                logger.WriteLine();
            }


            File.WriteAllText("summary.md", logger.ToString());

            static string Lpad(string text, int len)
            {
                if (text.Length < len)
                {
                    return (new string(' ', len - text.Length)) + text;
                }
                return text;
            }
            static string Rpad(string text, int len)
            {
                if (text.Length < len)
                {
                    return text + (new string(' ', len - text.Length));
                }
                return text;
            }
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

    [SimpleJob(RuntimeMoniker.Net70)]
    public class Convert
    {
        [Params("About-10-tags", "About-100-tags", "About-500-tags")]
        public string Name
        {
            get => _name;
            set
            {
                _name = value;

                var path = $"MarkdownFromHtml.Benchmark.inputs.{value}.html";
                using (var stream = typeof(Program).Assembly.GetManifestResourceStream(path))
                using (var reader = new StreamReader(stream))
                    Html = reader.ReadToEnd();
            }
        }

        public string Html { set; get; }

        private string _name;
        private readonly H2MConverter _h2mconverter;
        private readonly MyConverter _myconverter;
        private readonly ReMdConverter _remdconverter;

        public Convert()
        {
            _h2mconverter = new H2MConverter();
            _myconverter = new MyConverter();
            _remdconverter = new ReMdConverter();
        }

        [Benchmark]
        public string Html2Markdown()
        {
            return _h2mconverter.Convert(Html);
        }

        [Benchmark]
        public string MarkdownFromHtml()
        {
            return _myconverter.Convert(Html);
        }

        [Benchmark]
        public string ReverseMarkdown()
        {
            return _remdconverter.Convert(Html);
        }
    }
}
