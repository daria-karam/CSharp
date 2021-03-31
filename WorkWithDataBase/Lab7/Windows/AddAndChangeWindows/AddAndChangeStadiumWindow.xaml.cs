using System;
using System.Windows;

namespace Lab7
{
    /// <summary>
    /// Логика взаимодействия для AddAndChangeStadiumWindow.xaml
    /// </summary>
    public partial class AddAndChangeStadiumWindow : Window
    {
        public AddAndChangeStadiumWindow()
        {
            InitializeComponent();
            this.Height = 400;
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
            int stadiumID, stadiumCapacity;
            Stadium myStadium;
            if (tbStadiumID.Text != null && Int32.TryParse(tbStadiumID.Text, out stadiumID) &&
                tbCapacity.Text != null && Int32.TryParse(tbCapacity.Text, out stadiumCapacity) &&
                !tbAdress.Text.Equals("") && !tbStadiumName.Text.Equals(""))
            {
                myStadium = new Stadium(stadiumID, tbStadiumName.Text, tbAdress.Text, stadiumCapacity);
                switch (MainWindow.currentOperationType)
                {
                    case "Добавление":
                        ex = MainWindow.data.AddStadiumToDB(myStadium);
                        break;
                    case "Изменение":
                        ex = MainWindow.data.UpdateStadiumInDB(myStadium);
                        break;
                }
                if (ex == null)
                {
                    this.Close();
                }
                else
                {
                    this.Height = 500;
                    UncorrectMessage.Text = ex;
                    UncorrectMessage.Visibility = Visibility.Visible;
                }
            }
            else
            {
                this.Height = 460;
                UncorrectMessage.Text = "Введены некорректные данные либо не все обязательные поля заполнены.\n" +
                    "Исправьте данные и повторите попытку.";
                UncorrectMessage.Visibility = Visibility.Visible;
            }
        }

        public void ClickSearch(object sender, RoutedEventArgs e)
        {
            int stadiumID;
            if (tbStadiumID.Text != null && Int32.TryParse(tbStadiumID.Text, out stadiumID))
            {
                Stadium stadium = MainWindow.data.GetStadiumFromDB(stadiumID);
                if (stadium != null)
                {
                    tbStadiumName.Text = stadium.StadiumName.ToString();
                    tbAdress.Text = stadium.StadiumAddress.ToString();
                    tbCapacity.Text = stadium.StadiumCapacity.ToString();
                }
            }
        }
        private void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MainWindow.isAdditionalWindowOpened = false;
        }
    }
}
