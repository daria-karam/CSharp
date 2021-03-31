using System;
using System.Windows;

namespace Lab7
{
    /// <summary>
    /// Логика взаимодействия для AddAndChangeSportWindow.xaml
    /// </summary>
    public partial class AddAndChangeSportWindow : Window
    {
        public AddAndChangeSportWindow()
        {
            InitializeComponent();
            this.Height = 250;
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
            int sportID;
            Sport mySport;
            if (tbSportID.Text != null && Int32.TryParse(tbSportID.Text, out sportID) && !tbSportName.Text.Equals(""))
            {
                mySport = new Sport(sportID, tbSportName.Text);
                switch (MainWindow.currentOperationType)
                {
                    case "Добавление":
                        ex = MainWindow.data.AddSportToDB(mySport);
                        break;
                    case "Изменение":
                        ex = MainWindow.data.UpdateSportInDB(mySport);
                        break;
                }
                if (ex == null)
                {
                    this.Close();
                }
                else
                {
                    this.Height = 350;
                    UncorrectMessage.Text = ex;
                    UncorrectMessage.Visibility = Visibility.Visible;
                }
            }
            else
            {
                this.Height = 320;
                UncorrectMessage.Text = "Введены некорректные данные либо не все обязательные поля заполнены.\n" +
                    "Исправьте данные и повторите попытку.";
                UncorrectMessage.Visibility = Visibility.Visible;
            }
        }

        public void ClickSearch(object sender, RoutedEventArgs e)
        {
            int sportID;
            if (tbSportID.Text != null && Int32.TryParse(tbSportID.Text, out sportID))
            {
                Sport sport = MainWindow.data.GetSportFromDB(sportID);
                if (sport != null)
                {
                    tbSportID.Text = sport.SportID.ToString();
                    tbSportName.Text = sport.SportName.ToString();
                }
            }
        }

        private void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MainWindow.isAdditionalWindowOpened = false;
        }
    }
}
