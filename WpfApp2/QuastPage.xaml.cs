using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp2
{
    /// <summary>
    /// Логика взаимодействия для QuastPage.xaml
    /// </summary>
    public partial class QuastPage : Page
    {
        public QuastPage()
        {
            InitializeComponent();
        }
        private SQLiteConnection connect = new SQLiteConnection(@"data source = DB.db");//расположение бд в папке debug(корень приложения)
        private DataTable quast = new DataTable();
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (grid.SelectedItems.Count == 0)
            {
                return;
            }
            try
            {
                this.DeleteBD(((DataRowView)grid.SelectedItem).Row["ID"].ToString());
            }
            catch
            {
                MessageBox.Show("Ошибка удоления");
            }
            this.UpdateConnectDB();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddEditQuast());
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            if (grid.SelectedItems.Count == 0)
            {
                return;
            }
            try
            {
                NavigationService.Navigate(new AddEditQuast((DataRowView)grid.SelectedItem));

            }
            catch
            {
                MessageBox.Show("Услуга не выделена");
            }
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new QuastFilter(quast));
        }
        //удаление строки
        public void DeleteBD(string id)
        {
            try
            {
                connect.Open();
                SQLiteCommand command = new SQLiteCommand();
                command.Connection = connect;
                command.CommandText = @"DELETE FROM 'main'.'Applications'" +
      "WHERE " + "'Applications'.'ID' = " + id;
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(command);
                SQLiteCommandBuilder sqlBuilder = new SQLiteCommandBuilder(adapter);
                int number = command.ExecuteNonQuery();

                MessageBox.Show($"Удалено обьектов: {number}");
            }
            catch 
            {
                MessageBox.Show("Ошибка удаления");
            }
            finally
            {
                if (connect != null)
                    connect.Close();
            }
        }
        public void UpdateConnectDB()
        {


            quast.Clear();
            //выгрузка информации о пользователях
            try
            {
                connect.Open();
                string sql = "SELECT * from Applications";
                SQLiteCommand command = new SQLiteCommand(sql, connect);
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(command);
                adapter.Fill(quast);
               
            }
            catch 
            {
                MessageBox.Show("Данные не получены!", "Ошибка");
            }
            finally
            {
                if (connect != null)
                    connect.Close();
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateConnectDB();
            grid.ItemsSource = quast.DefaultView;
        }
    }
}
