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

    public partial class UserPage : Page
    {
        //расположение бд в папке debug(корень приложения)
        private SQLiteConnection connect = new SQLiteConnection(@"data source = DB.db");
        //Переменная для хранения таблицы
        private DataTable Users = new DataTable();

        //Загрузка информации о пользователях
        public void UpdateConnectDB()
        {
            Users.Clear();
            try
            {
                connect.Open();
                string sql = "SELECT * from Users";//Запрос
                SQLiteCommand command = new SQLiteCommand(sql, connect);//Создание запроса
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(command);//Создание адаптера с указанием запроса
                adapter.Fill(Users);//Заполнение таблицы
            }
            catch (Exception ex)
            {
                MessageBox.Show("Данные не получены!","Ошибка");
            }
            finally
            {
                if (connect != null)
                    connect.Close();
            }
        }
        public UserPage()
        {
            InitializeComponent();
        }
        //Нажатие на кнопку удаления
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (grid.SelectedItems.Count == 0)
                return;//Есле не выделен
            try
            {
               //Вызов метода с указанием ID выделеной строки
               DeleteBD(((DataRowView)grid.SelectedItem).Row["ID"].ToString());
            }
            catch
            {
                MessageBox.Show("Ошибка удоления, стока не выбрана","Ошибка");
            }
            UpdateConnectDB();//Отоброжение результата
        }
        //Удаление строки
        public void DeleteBD(string id)
        { 
            try
            {
                connect.Open();
                //запрос на удаление
                string sql = @"DELETE FROM 'main'.'Users'" +
                    "WHERE " + "'Users'.'ID' = " + id;     
                SQLiteCommand command = new SQLiteCommand(sql,connect);
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(command);
                SQLiteCommandBuilder sqlBuilder = new SQLiteCommandBuilder(adapter);
                int number = command.ExecuteNonQuery();//Выполнение запроса и получение кол-ва затронутых строк

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



        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new addEditUsers());
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            try
            {
                NavigationService.Navigate(new addEditUsers((DataRowView)grid.SelectedItem));

            }
            catch
            {
                MessageBox.Show("Пользователь не выделен");
            }

        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateConnectDB();
            grid.ItemsSource = Users.DefaultView;
        }
    }
}
