using System.Windows;
namespace Lab7
{
    /// <summary>
    /// Логика взаимодействия для Task2Window.xaml
    /// </summary>
    public partial class Task2Window : Window
    {
        public static MyData data = new MyData();
        public Task2Window()
        {
            InitializeComponent();
            
        }

        public void ClickMSysObjects(object sender, RoutedEventArgs e)
        {
            Title.Content = "MSysObjects";
            data.MSysObjectsRequest("SELECT * FROM MSysObjects");
            MSysObjectsDataTable.AutoGenerateColumns = true;
            MSysObjectsDataTable.ItemsSource = data.MSysObjectsData.DefaultView;
        }

        public void ClickMSysACEs(object sender, RoutedEventArgs e)
        {
            Title.Content = "MSysACEs";
            data.MSysObjectsRequest("SELECT * FROM MSysACEs");
            MSysObjectsDataTable.AutoGenerateColumns = true;
            MSysObjectsDataTable.ItemsSource = data.MSysObjectsData.DefaultView;
        }

        public void ClickMSysQueries(object sender, RoutedEventArgs e)
        {
            Title.Content = "MSysQueries";
            data.MSysObjectsRequest("SELECT * FROM MSysQueries");
            MSysObjectsDataTable.AutoGenerateColumns = true;
            MSysObjectsDataTable.ItemsSource = data.MSysObjectsData.DefaultView;
        }

        public void ClickMSysRelationships(object sender, RoutedEventArgs e)
        {
            Title.Content = "MSysRelationships";
            data.MSysObjectsRequest("SELECT * FROM MSysRelationships");
            MSysObjectsDataTable.AutoGenerateColumns = true;
            MSysObjectsDataTable.ItemsSource = data.MSysObjectsData.DefaultView;
        }

        public void ClickMSysComplexColumns(object sender, RoutedEventArgs e)
        {
            Title.Content = "MSysComplexColumns";
            data.MSysObjectsRequest("SELECT * FROM MSysComplexColumns");
            MSysObjectsDataTable.AutoGenerateColumns = true;
            MSysObjectsDataTable.ItemsSource = data.MSysObjectsData.DefaultView;
        }

        public void ClickMSysAccessStorage(object sender, RoutedEventArgs e)
        {
            Title.Content = "MSysAccessStorage";
            data.MSysObjectsRequest("SELECT * FROM MSysAccessStorage");
            MSysObjectsDataTable.AutoGenerateColumns = true;
            MSysObjectsDataTable.ItemsSource = data.MSysObjectsData.DefaultView;
        }

        private void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MainWindow.isTask2WindowOpened = false;
        }
    }
}
