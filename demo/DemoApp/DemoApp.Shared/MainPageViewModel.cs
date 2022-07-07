using Markdig;
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

        public MainPageViewModel()
        {
            TargetName = "html";
            ConvertedName = "markdown";
            TargetContent = "";
            ConvertedContent = "";

            ConvertCommand = new ActCommand(Convert);
            RotateCommand = new ActCommand(Rotate);
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
                var converter = new Html2Markdown.Converter();
                ConvertedContent = converter.Convert(TargetContent);
            }
            if (TargetName == "markdown")
            {
                ConvertedContent = Markdown.ToHtml(TargetContent);
            }
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
