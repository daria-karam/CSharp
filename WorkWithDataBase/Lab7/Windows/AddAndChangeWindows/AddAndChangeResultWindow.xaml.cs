using System;
using System.Windows;

namespace Lab7
{
    /// <summary>
    /// Логика взаимодействия для AddAndChangeResultWindow.xaml
    /// </summary>
    public partial class AddAndChangeResultWindow : Window
    {
        public AddAndChangeResultWindow()
        {
            InitializeComponent();
            this.Height = 450;
            UncorrectMessage.Visibility = Visibility.Hidden;
            switch (MainWindow.currentOperationType)
            {
                case "Добавление":
                    titleLabel.Content = "Добавление элемента в таблицу " + MainWindow.currentTableName;
                    acceptButton.Content = "Добавить";
                        SearchButton.Visibility = Visibility.Hidden;
                    break;
                case "Изменение":
                    titleLabel.Content = "Изменение элемента в таблице " + MainWindow.currentTableName;
                    acceptButton.Content = "Изменить";
                        SearchButton.Visibility = Visibility.Visible;
                    break;
            }
        }

        public void ClickAccept(object sender, RoutedEventArgs e)
        {
            string ex = null;
            int attemptNumber, contestID, commandID, attemptResult;
            Result myResult;
            if (tbAttemptNumber.Text != null && Int32.TryParse(tbAttemptNumber.Text, out attemptNumber) &&
                tbContestID.Text != null && Int32.TryParse(tbContestID.Text, out contestID) &&
                tbCommandID.Text != null && Int32.TryParse(tbCommandID.Text, out commandID) &&
                tbAttemptResult.Text != null && Int32.TryParse(tbAttemptResult.Text, out attemptResult) &&
                !tbDate.Text.Equals(""))
            {
                myResult = new Result(attemptNumber, contestID, commandID, tbDate.Text, attemptResult);
                switch (MainWindow.currentOperationType)
                {
                    case "Добавление":
                        ex = MainWindow.data.AddResultToDB(myResult);
                        break;
                    case "Изменение":
                        ex = MainWindow.data.UpdateResultInDB(myResult);
                        break;
                }
                if (ex == null)
                {
                    this.Close();
                }
                else
                {
                    this.Height = 560;
                    UncorrectMessage.Text = ex;
                    UncorrectMessage.Visibility = Visibility.Visible;
                }
            }
            else
            {
                this.Height = 510;
                UncorrectMessage.Text = "Введены некорректные данные либо не все обязательные поля заполнены.\n" +
                    "Исправьте данные и повторите попытку.";
                UncorrectMessage.Visibility = Visibility.Visible;
            }
        }

        public void ClickSearch(object sender, RoutedEventArgs e)
        {
            int attemptNumber;
            if (tbAttemptNumber.Text != null && Int32.TryParse(tbAttemptNumber.Text, out attemptNumber))
            {
                Result result = MainWindow.data.GetResultFromDB(attemptNumber);
                if (result != null)
                {
                    tbContestID.Text = result.ContestID.ToString();
                    tbCommandID.Text = result.TeamID.ToString();
                    tbAttemptResult.Text = result.AttemptResult.ToString();
                    tbDate.Text = result.Date.ToString();
                }
            }
        }

        private void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MainWindow.isAdditionalWindowOpened = false;
        }
    }
}
