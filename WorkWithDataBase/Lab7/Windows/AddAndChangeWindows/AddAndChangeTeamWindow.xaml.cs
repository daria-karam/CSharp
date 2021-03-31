using System;
using System.Windows;

namespace Lab7
{
    /// <summary>
    /// Логика взаимодействия для AddAndChangeTeamWindow.xaml
    /// </summary>
    public partial class AddAndChangeTeamWindow : Window
    {
        public AddAndChangeTeamWindow()
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
            int commandID, sportsmanCount;
            Team myCommand;
            if (tbCommandID.Text != null && Int32.TryParse(tbCommandID.Text, out commandID) &&
                !tbCommandName.Text.Equals("") && tbPlayerCount.Text != null &&
                Int32.TryParse(tbPlayerCount.Text, out sportsmanCount))
            {
                myCommand = new Team(commandID, tbCommandName.Text, tbCity.Text, sportsmanCount, tbTrainer.Text);
                switch (MainWindow.currentOperationType)
                {
                    case "Добавление":
                        ex = MainWindow.data.AddTeamToDB(myCommand);
                        break;
                    case "Изменение":
                        ex = MainWindow.data.UpdateTeamInDB(myCommand);
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
            int commandID;
            if (tbCommandID.Text != null && Int32.TryParse(tbCommandID.Text, out commandID))
            {
                Team team = MainWindow.data.GetTeamFromDB(commandID);
                if (team != null)
                {
                    tbCommandName.Text = team.TeamName.ToString();
                    tbCity.Text = team.City.ToString();
                    tbPlayerCount.Text = team.SportsmanCount.ToString();
                    tbTrainer.Text = team.Trainer.ToString();
                }
            }
        }

        private void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MainWindow.isAdditionalWindowOpened = false;
        }
    }
}
