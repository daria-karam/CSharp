using System;
using System.Windows;

namespace Lab7
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MyData data = new MyData();
        public static string currentTableName;
        public static string currentOperationType;
        public static bool isAdditionalWindowOpened = false;
        public static bool isTask2WindowOpened = false;
        public static bool isTask3WindowOpened = false;

        public MainWindow()
        {
            InitializeComponent();
        }

        public void ClickUpdate(object sender, RoutedEventArgs e)
        {
            if (tableName.Text != null)
            {
                currentTableName = tableName.Text;
                ShowAllDataFromTable();
            }
        }

        public void ClickAdd(object sender, RoutedEventArgs e)
        {
            if (tableName.Text != null && !isAdditionalWindowOpened)
            {
                currentTableName = tableName.Text;
                currentOperationType = "Добавление";
                switch (tableName.Text)
                {
                    case "Соревнование":
                        new AddAndChangeContestWindow().Show();
                        isAdditionalWindowOpened = true;
                        break;
                    case "Спортсмен":
                        new AddAndChangeSportsmanWindow().Show();
                        isAdditionalWindowOpened = true;
                        break;
                    case "Команда":
                        new AddAndChangeTeamWindow().Show();
                        isAdditionalWindowOpened = true;
                        break;
                    case "Результат":
                        new AddAndChangeResultWindow().Show();
                        isAdditionalWindowOpened = true;
                        break;
                    case "Стадион":
                        new AddAndChangeStadiumWindow().Show();
                        isAdditionalWindowOpened = true;
                        break;
                    case "Вид спорта":
                        new AddAndChangeSportWindow().Show();
                        isAdditionalWindowOpened = true;
                        break;
                }
            }
        }

        public void ClickChange(object sender, RoutedEventArgs e)
        {
            if (tableName.Text != null && !isAdditionalWindowOpened)
            {
                currentTableName = tableName.Text;
                currentOperationType = "Изменение";
                switch (tableName.Text)
                {
                    case "Соревнование":
                        new AddAndChangeContestWindow().Show();
                        isAdditionalWindowOpened = true;
                        break;
                    case "Спортсмен":
                        new AddAndChangeSportsmanWindow().Show();
                        isAdditionalWindowOpened = true;
                        break;
                    case "Команда":
                        new AddAndChangeTeamWindow().Show();
                        isAdditionalWindowOpened = true;
                        break;
                    case "Результат":
                        new AddAndChangeResultWindow().Show();
                        isAdditionalWindowOpened = true;
                        break;
                    case "Стадион":
                        new AddAndChangeStadiumWindow().Show();
                        isAdditionalWindowOpened = true;
                        break;
                    case "Вид спорта":
                        new AddAndChangeSportWindow().Show();
                        isAdditionalWindowOpened = true;
                        break;
                }
            }
        }

        public void ClickDelete(object sender, RoutedEventArgs e)
        {
            if (tableName.Text != null && !isAdditionalWindowOpened && (tableName.Text == "Соревнование"
                || tableName.Text == "Спортсмен" || tableName.Text == "Команда" || tableName.Text == "Результат" ||
                tableName.Text == "Стадион" || tableName.Text == "Вид спорта"))
            {
                currentTableName = tableName.Text;
                new DeleteWindow().Show();
                isAdditionalWindowOpened = true;
            }
        }
    
        public void ShowAllDataFromTableClick(object sender, RoutedEventArgs e)
        {
            string[] temp = tableName.SelectedItem.ToString().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            currentTableName = temp[temp.Length - 1];
            if (currentTableName.Equals("спорта"))
            {
                currentTableName = "Вид спорта";
            }
            ShowAllDataFromTable();
        }

        public void ShowAllDataFromTable()
        {
            dgTableData.AutoGenerateColumns = true;
            switch (currentTableName)
            {
                case "Соревнование":
                    //show table
                    dgTableData.ItemsSource = data.contests;
                    data.GetContestsFromDB();
                    dgTableData.Items.Refresh();
                    break;
                case "Спортсмен":
                    //show table
                    dgTableData.ItemsSource = data.sportsmans;
                    data.GetSportsmansFromDB();
                    dgTableData.Items.Refresh();
                    break;
                case "Команда":
                    //show table
                    dgTableData.ItemsSource = data.teams;
                    data.GetTeamsFromDB();
                    dgTableData.Items.Refresh();
                    break;
                case "Результат":
                    //show table
                    dgTableData.ItemsSource = data.results;
                    data.GetResultsFromDB();
                    dgTableData.Items.Refresh();
                    break;
                case "Стадион":
                    //show table
                    dgTableData.ItemsSource = data.stadiums;
                    data.GetStadiumsFromDB();
                    dgTableData.Items.Refresh();
                    break;
                case "Вид спорта":
                    //show table
                    dgTableData.ItemsSource = data.sports;
                    data.GetSportsFromDB();
                    dgTableData.Items.Refresh();
                    break;
                default:
                    //do nothing;
                    break;
            }
        }
    
        public void OpenTask3(object sender, RoutedEventArgs e)
        {
            if (!isTask3WindowOpened)
            {
                isTask3WindowOpened = true;
                new Task3Window().Show();
            }
        }

        public void OpenTask2(object sender, RoutedEventArgs e)
        {
            if (!isTask2WindowOpened)
            {
                isTask2WindowOpened = true;
                new Task2Window().Show();
            }
        }
    
        /*
        public void ClickLINQ(object sender, RoutedEventArgs e)
        {
            string connString = "Provider=Microsoft.ACE.OLEDB.12.0; Data Source=C:\\Users\\Daria\\Desktop\\БД\\Lab7\\Lab7_DB.accdb";

            string query = "SELECT * FROM Спортсмен";
            OleDbDataAdapter dAdapter = new OleDbDataAdapter(query, connString);
            OleDbCommandBuilder cBuilder = new OleDbCommandBuilder(dAdapter);
            DataTable dTable = new DataTable();
            dAdapter.Fill(dTable);

            IEnumerable<DataRow> results = from myRow in dTable.AsEnumerable()
                          where myRow.Field<string>("Имя") == "Геральт"
                          select myRow;
            DataTable boundTable = results.CopyToDataTable<DataRow>();
            dgTableData.ItemsSource = results;
            dgTableData.Items.Refresh();
        }
        */
    }
}
