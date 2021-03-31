using System;
using System.Windows;

namespace Lab7
{
    /// <summary>
    /// Логика взаимодействия для DeleteWindow.xaml
    /// </summary>
    public partial class DeleteWindow : Window
    {
        public DeleteWindow()
        {
            InitializeComponent();
            this.Height = 250;
            titleLabel.Content = "Удаление элемента из таблицы " + MainWindow.currentTableName;
        }

        public void ClickAcceptDelete(object sender, RoutedEventArgs e)
        {
            string ex;
            int ID;
            if (delObjID.Text != null && Int32.TryParse(delObjID.Text, out ID))
            {
                ex = MainWindow.data.DeleteFromTableByID(ID);
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
                this.Height = 300;
                UncorrectMessage.Text = "Введены некорректные данные либо не все обязательные поля заполнены.\n" +
                    "Исправьте данные и повторите попытку.";
                UncorrectMessage.Visibility = Visibility.Visible;
            }
        }

        private void WindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MainWindow.isAdditionalWindowOpened = false;
        }
    }
}
