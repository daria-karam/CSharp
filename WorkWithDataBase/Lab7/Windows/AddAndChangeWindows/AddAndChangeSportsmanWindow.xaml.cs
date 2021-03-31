using System;
using System.Windows;

namespace Lab7
{
    /// <summary>
    /// Логика взаимодействия для AddAndChangeSportsmanWindow.xaml
    /// </summary>
    public partial class AddAndChangeSportsmanWindow : Window
    {
        public AddAndChangeSportsmanWindow()
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
            int sportsmanID, commandID;
            Sportsman mySportsman;
            if (tbSportsmanNumber.Text != null && Int32.TryParse(tbSportsmanNumber.Text, out sportsmanID) &&
                tbCommandID.Text != null && Int32.TryParse(tbCommandID.Text, out commandID) &&
                !tbName.Text.Equals(""))
            {
                mySportsman = new Sportsman(sportsmanID, commandID, tbSurname.Text, tbName.Text, tbPatronymic.Text);
                switch (MainWindow.currentOperationType)
                {
                    case "Добавление":
                        ex = MainWindow.data.AddSportsmanToDB(mySportsman);
                        break;
                    case "Изменение":
                        ex = MainWindow.data.UpdateSportsmanInDB(mySportsman);
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
            int sportsmanID;
            if (tbSportsmanNumber.Text != null && Int32.TryParse(tbSportsmanNumber.Text, out sportsmanID))
            {
                Sportsman sportsman = MainWindow.data.GetSportsmanFromDB(sportsmanID);
                if (sportsman != null)
                {
                    tbCommandID.Text = sportsman.TeamID.ToString();
                    tbSurname.Text = sportsman.Surname.ToString();
                    tbName.Text = sportsman.Name.ToString();
                    tbPatronymic.Text = sportsman.Patronymic.ToString();
                }
            }
        }

        private void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MainWindow.isAdditionalWindowOpened = false;
        }
    }
}
