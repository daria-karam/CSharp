using System;
using System.Windows;

namespace Lab7
{
    /// <summary>
    /// Логика взаимодействия для AddAndChangeWindow.xaml
    /// </summary>
    public partial class AddAndChangeContestWindow : Window
    {
        public AddAndChangeContestWindow()
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
            int contestID, sportID, stadiumID;
            Contest myContest;
            if (tbContestID.Text != null && Int32.TryParse(tbContestID.Text, out contestID) &&
                tbSportID.Text != null && Int32.TryParse(tbSportID.Text, out sportID) &&
                tbStadiumID.Text != null && Int32.TryParse(tbStadiumID.Text, out stadiumID) &&
                !tbBeginDate.Text.Equals("") && !tbEndDate.Text.Equals(""))
            {
                myContest = new Contest(contestID, sportID, stadiumID, tbBeginDate.Text, tbEndDate.Text);
                switch (MainWindow.currentOperationType)
                {
                    case "Добавление":
                        ex = MainWindow.data.AddContestToDB(myContest);
                        break;
                    case "Изменение":
                        ex = MainWindow.data.UpdateContestInDB(myContest);
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
            int contestID;
            if (tbContestID.Text != null && Int32.TryParse(tbContestID.Text, out contestID))
            {
                Contest contest = MainWindow.data.GetContestFromDB(contestID);
                if (contest != null)
                {
                    tbSportID.Text = contest.SportID.ToString();
                    tbStadiumID.Text = contest.StadiumID.ToString();
                    tbBeginDate.Text = contest.BeginDate.ToString();
                    tbEndDate.Text = contest.EndDate.ToString();
                }
            }
        }

        private void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MainWindow.isAdditionalWindowOpened = false;
        }
    }
}
