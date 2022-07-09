using Html2Markdown.Parsers;
using Html2Markdown.Parsers.MarkdigExtensions;
using Markdig;
using Markdig.Extensions.EmphasisExtras;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using TsOpUndo;

namespace DemoApp
{
    public class MainPageViewModel : GenericNotifyPropertyChanged2
    {
        public ICommand ConvertCommand { get; set; }
        public ICommand RotateCommand { get; set; }

        public string TargetName { set => SetValue(value); get => GetValue<string>(); }
        public string TargetContent { set => SetValue(value); get => GetValue<string>(); }

        public string ConvertedName { set => SetValue(value); get => GetValue<string>(); }
        public string ConvertedContent { set => SetValue(value); get => GetValue<string>(); }

        public bool IsEnabledEmphasisExtra { set => SetValue(value); get => GetValue<bool>(); }
        public bool IsEnabledEmphasisDeleted { set => SetValue(value); get => GetValue<bool>(); }
        public bool IsEnabledEmphasisInserted { set => SetValue(value); get => GetValue<bool>(); }
        public bool IsEnabledEmphasisMarked { set => SetValue(value); get => GetValue<bool>(); }
        public bool IsEnabledEmphasisSubscript { set => SetValue(value); get => GetValue<bool>(); }
        public bool IsEnabledEmphasisSuperscript { set => SetValue(value); get => GetValue<bool>(); }

        public bool IsEnabledPipeTable { set => SetValue(value); get => GetValue<bool>(); }
        public bool IsEnabledGridTable { set => SetValue(value); get => GetValue<bool>(); }

        public bool IsEnabledFigureFooterAndCite { set => SetValue(value); get => GetValue<bool>(); }
        public bool IsEnabledFigure { set => SetValue(value); get => GetValue<bool>(); }
        public bool IsEnabledFooter { set => SetValue(value); get => GetValue<bool>(); }
        public bool IsEnabledCite { set => SetValue(value); get => GetValue<bool>(); }


        public MainPageViewModel()
        {
            TargetName = "html";
            ConvertedName = "markdown";
            TargetContent = "<html>\n  <body>\n    <div>\n      <h1>Hellow Html to Markdown</h1>\n    </div>\n\n    <div>\n      <h2>What's this?</h2>\n      <p>This application convert html to markdown.</p>\n    </div>\n\n    <div>\n      <h2>Support</h2>\n      <ul>\n        <li><code>&lt;a&gt;</code></li>\n        <li><code>&lt;strong&gt;</code></li>\n        <li><code>&lt;b&gt;</code></li>\n        <li><code>&lt;em&gt;</code></li>\n        <li><code>&lt;i&gt;</code></li>\n        <li><code>&lt;br&gt;</code></li>\n        <li><code>&lt;code&gt;</code></li>\n        <li><code>&lt;h1&gt;</code></li>\n        <li><code>&lt;h2&gt;</code></li>\n        <li><code>&lt;h3&gt;</code></li>\n        <li><code>&lt;h4&gt;</code></li>\n        <li><code>&lt;h5&gt;</code></li>\n        <li><code>&lt;h6&gt;</code></li>\n        <li><code>&lt;blockquote&gt;</code></li>\n        <li><code>&lt;img&gt;</code></li>\n        <li><code>&lt;hr&gt;</code></li>\n        <li><code>&lt;p&gt;</code></li>\n        <li><code>&lt;pre&gt;</code></li>\n        <li><code>&lt;ul&gt;</code></li>\n        <li><code>&lt;ol&gt;</code></li>\n      </ul>\n    </div>\n  </body>\n</html>\n";
            ConvertedContent = "";

            IsEnabledEmphasisDeleted = true;
            IsEnabledEmphasisInserted = true;
            IsEnabledEmphasisMarked = true;
            IsEnabledEmphasisSubscript = true;
            IsEnabledEmphasisSuperscript = true;

            IsEnabledFigure = true;
            IsEnabledFooter = true;
            IsEnabledCite = true;

            ConvertCommand = new ActCommand(Convert);
            RotateCommand = new ActCommand(Rotate);

            Convert();
        }


        public void Rotate()
        {
            var name = TargetName;
            TargetName = ConvertedName;
            ConvertedName = name;

            TargetContent = ConvertedContent;

            Convert();
        }

        public void Convert()
        {
            if (TargetName == "html")
            {
                var converter = CreateConverter();
                ConvertedContent = converter.Convert(TargetContent);
            }
            if (TargetName == "markdown")
            {
                var pipe = CreatePipeline();
                ConvertedContent = Markdown.ToHtml(TargetContent, pipe);
            }
        }

        private Html2Markdown.Converter CreateConverter()
        {
            var manager = new Html2Markdown.ReplaceManager();

            if (IsEnabledGridTable)
                manager.Register(new GridTableParser());

            if (IsEnabledPipeTable)
                manager.Register(new PipeTableParser());

            if (IsEnabledEmphasisExtra)
            {
                if (IsEnabledEmphasisDeleted)
                    manager.Register(new DeletedParser());

                if (IsEnabledEmphasisInserted)
                    manager.Register(new InsertedParser());

                if (IsEnabledEmphasisMarked)
                    manager.Register(new MarkedParser());

                if (IsEnabledEmphasisSubscript)
                    manager.Register(new SubscriptParser());

                if (IsEnabledEmphasisSuperscript)
                    manager.Register(new SuperscriptParser());
            }

            if (IsEnabledFigureFooterAndCite)
            {
                if (IsEnabledFigure)
                    manager.Register(new FigureParser());

                if (IsEnabledFooter)
                    manager.Register(new FooterParser());

                if (IsEnabledCite)
                    manager.Register(new CiteParser());
            }

            return new Html2Markdown.Converter(manager);
        }

        private MarkdownPipeline CreatePipeline()
        {
            var builder = new MarkdownPipelineBuilder();

            if (IsEnabledPipeTable)
                builder.UsePipeTables();

            if (IsEnabledGridTable)
                builder.UseGridTables();

            if (IsEnabledEmphasisExtra)
            {
                builder.UseEmphasisExtras(
                     (IsEnabledEmphasisDeleted ? EmphasisExtraOptions.Strikethrough : 0) |
                     (IsEnabledEmphasisInserted ? EmphasisExtraOptions.Inserted : 0) |
                     (IsEnabledEmphasisMarked ? EmphasisExtraOptions.Marked : 0) |
                     (IsEnabledEmphasisSubscript ? EmphasisExtraOptions.Subscript : 0) |
                     (IsEnabledEmphasisSuperscript ? EmphasisExtraOptions.Superscript : 0)
               );
            }

            if (IsEnabledFigureFooterAndCite)
            {
                if (IsEnabledFigure) builder.UseFigures();
                if (IsEnabledFooter) builder.UseFooters();
                if (IsEnabledCite) builder.UseCitations();
            }

            return builder.Build();
        }

    }

    class ActCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        Action _action;

        public ActCommand(Action act)
        {
            _action = act;
        }


        public bool CanExecute(object parameter) => true;

        public void Execute(object parameter) => _action.Invoke();
    }
}
