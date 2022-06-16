using CBASLanguageInterpreter.Helpers;
using CBASLanguageInterpreter.Interpretation;
using InterpretorUI.Commands;
using InterpretorUI.Helpers;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace InterpretorUI.ViewModels
{
    class MainWindowViewModel: INotifyPropertyChanged
    {
        #region binding data for text fields

        private string _codeText;
        private string _outputText;

        public string CodeText
        {
            get => _codeText;
            set
            {
                _codeText = value;
                OnPropertyChanged("CodeText");
            }
        }

        public string OutputText
        {
            get => _outputText;
            set
            {
                _outputText = value;
                OnPropertyChanged("OutputText");
            }
        }

        #endregion

        #region ICommand

        private ICommand _openFileCommand;
        private ICommand _saveFileCommand;
        private ICommand _executeCodeCommand;

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand OpenFileCommand { get => _openFileCommand;}
        public ICommand SaveFileCommand { get => _saveFileCommand;}
        public ICommand ExecuteCodeCommand { get => _executeCodeCommand;}

        #endregion

        public MainWindowViewModel()
        {
            _openFileCommand = new DelegateCommand(LoadCodeFromFile);
            _saveFileCommand = new DelegateCommand(SaveCodeToFile);
            _executeCodeCommand = new DelegateCommand(ExecuteCode);
        }

        #region methods for DelegateCommand

        public void LoadCodeFromFile()
        {
            FileDialog fileDialog = new OpenFileDialog();

            if (fileDialog.ShowDialog() != true)
            {
                return;
            }

            CodeText = File.ReadAllText(fileDialog.FileName);
        }

        public void SaveCodeToFile()
        {
            FileDialog fileDialog = new OpenFileDialog();

            if (fileDialog.ShowDialog() != true)
            {
                return;
            }

            File.WriteAllText(fileDialog.FileName, _codeText);
        }

        public void ExecuteCode()
        {
            SyntaxAnalyzer SA = new SyntaxAnalyzer();

            var (information, isCorrect) = SA.Analyse(out Stack<int> output, _codeText);

            OutputText = information;

            if (isCorrect)
            {
                ConsoleHelper.AllocConsole();

                var stack = output.Reverse();
                ConsoleHelper.WriteToConsole("---------------------------------------------------");

                Interpreter.Interpret(stack, true);

                Console.ReadKey();

                ConsoleHelper.FreeConsole();
            }
        }

        #endregion

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
