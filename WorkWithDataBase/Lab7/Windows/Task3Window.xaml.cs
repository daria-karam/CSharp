using System;
using System.Windows;

namespace Lab7
{
    /// <summary>
    /// Логика взаимодействия для Task3Window.xaml
    /// </summary>
    public partial class Task3Window : Window
    {
        public MyData data = new MyData();

        public Task3Window()
        {
            InitializeComponent();
        }

        public void ClickButton1(object sender, RoutedEventArgs e)
        {
            data.Request1();
            DataTable.AutoGenerateColumns = true;
            DataTable.ItemsSource = data.results;
            DataTable.Items.Refresh();
        }

        public void ClickButton2(object sender, RoutedEventArgs e)
        {
            data.Request2();
            DataTable.AutoGenerateColumns = true;
            DataTable.ItemsSource = data.contests;
            DataTable.Items.Refresh();
        }

        public void ClickButton3(object sender, RoutedEventArgs e)
        {
            data.Request3();
            DataTable.AutoGenerateColumns = true;
            DataTable.ItemsSource = data.teams;
            DataTable.Items.Refresh();
        }

        public void ClickButton4(object sender, RoutedEventArgs e)
        {
            int ID;
            if (tb4.Text != null && Int32.TryParse(tb4.Text, out ID))
            {
                data.Request4(ID);
                DataTable.AutoGenerateColumns = true;
                DataTable.ItemsSource = data.sportsmans;
                DataTable.Items.Refresh();
            }
        }

        public void ClickButton5(object sender, RoutedEventArgs e)
        {
            if (tb5.Text != null)
            {
                data.Request5(tb5.Text);
                DataTable.AutoGenerateColumns = true;
                DataTable.ItemsSource = data.teams;
                DataTable.Items.Refresh();
            }
        }

        public void ClickButton6(object sender, RoutedEventArgs e)
        {
            int capacity;
            if (tb6.Text != null && Int32.TryParse(tb6.Text, out capacity))
            {
                data.Request6(capacity);
                DataTable.AutoGenerateColumns = true;
                DataTable.ItemsSource = data.stadiums;
                DataTable.Items.Refresh();
            }
        }

        public void ClickButton7(object sender, RoutedEventArgs e)
        {
            data.Request7();
            data.GetSportsFromDB();
            DataTable.AutoGenerateColumns = true;
            DataTable.ItemsSource = data.sports;
            DataTable.Items.Refresh();
        }

        public void ClickButton8(object sender, RoutedEventArgs e)
        {
            data.Request8();
            data.GetSportsFromDB();
            DataTable.AutoGenerateColumns = true;
            DataTable.ItemsSource = data.sports;
            DataTable.Items.Refresh();
        }

        public void ClickButton9(object sender, RoutedEventArgs e)
        {
            data.Request9();
            data.GetSportsFromDB();
            DataTable.AutoGenerateColumns = true;
            DataTable.ItemsSource = data.sports;
            DataTable.Items.Refresh();
        }

        private void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MainWindow.isTask3WindowOpened = false;
        }
    }
}
