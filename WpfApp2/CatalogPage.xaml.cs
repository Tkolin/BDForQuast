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
    /// Логика взаимодействия для CatalogPage.xaml
    /// </summary>
    public partial class CatalogPage : Page
    {
        public CatalogPage()
        {
            InitializeComponent();
        }
        private SQLiteConnection connect = new SQLiteConnection(@"data source = DB.db");//расположение бд в папке debug(корень приложения)
        private DataTable Catalog = new DataTable();
        //Загрузка информации о пользователях
        public void UpdateConnectDB()
        {


            Catalog.Clear();
            
            try
            {
                connect.Open();
                string sql = "SELECT * from Services";
                SQLiteCommand command = new SQLiteCommand(sql, connect);
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(command);
                adapter.Fill(Catalog);
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
        //Удоление строки
        public void DeleteBD(string id)
        {
            try
            {
                connect.Open();
                SQLiteCommand command = new SQLiteCommand();
                command.Connection = connect;
                command.CommandText = @"DELETE FROM 'main'.'Services'" +
      "WHERE " + "'Services'.'ID' = " + id;
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(command);
                SQLiteCommandBuilder sqlBuilder = new SQLiteCommandBuilder(adapter);
                int number = command.ExecuteNonQuery();

                MessageBox.Show($"Удалено обьектов: {number}");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (connect != null)
                    connect.Close();
            }
        }


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
            try { 
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
            NavigationService.Navigate(new AddEditCatalog());
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            if (grid.SelectedItems.Count == 0)
            {
                return;
            }
            try { 
            NavigationService.Navigate(new AddEditCatalog((DataRowView)grid.SelectedItem));

            }
            catch
            {
                MessageBox.Show("Услуга не выделена");
            }

        }



        private void Page_Loaded_1(object sender, RoutedEventArgs e)
        {
            UpdateConnectDB();

            grid.ItemsSource = Catalog.DefaultView;
        }
    }
}
