using System;
using System.Collections.Generic;
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
using System.Data;
using System.Data.SQLite;

namespace WpfApp2
{
    /// <summary>
    /// Логика взаимодействия для addEditUsers.xaml
    /// </summary>
    public partial class addEditUsers : Page
    {
        //расположение бд в папке debug(корень приложения)
        private SQLiteConnection connect = new SQLiteConnection(@"data source = DB.db");
        bool updateType = false;
        DataRowView user;
        public addEditUsers(DataRowView user)
        {
            InitializeComponent();
           
            nameBox.Text = user.Row["Name"].ToString();
            surnameBox.Text = user.Row["Surname"].ToString();
            patronymicBox.Text = user.Row["Patronymic"].ToString();
            phoneBox.Text = user.Row["PhoneNumber"].ToString();
            emailBox.Text = user.Row["Email"].ToString();
            updateType = true;

            this.user = user;
        }
        public addEditUsers()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
        //Сохронение базы
        public void updateBD()
        {
            connect.Open();
            SQLiteCommand command = new SQLiteCommand();
            command.Connection = connect;

            if (updateType)
                command.CommandText = @"UPDATE 'main'.'Users' SET "+                             
                              "'Name' = "+"'"+ nameBox.Text + "'"+
                              ",'Surname' = " + "'"+ surnameBox.Text+"'" +
                              ",'Patronymic' = " + "'"+patronymicBox.Text+"'"+
                              ",'PhoneNumber' = " + "'"+phoneBox.Text+"'"+
                              ",'Email' = " + "'"+emailBox.Text+"'" +
                              "WHERE 'Users'.'ID' =" + user.Row["ID"];
            else
                command.CommandText = @"INSERT INTO  'main'.'Users' " +
                "('Name' ,'Surname','Patronymic' ,'PhoneNumber' ,'Email')" +
                "VALUES ('" + nameBox.Text + "','" + surnameBox.Text + "','" + patronymicBox.Text + "','" +
                phoneBox.Text + "','" + emailBox.Text + "')";
            try
            {
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(command);
                SQLiteCommandBuilder sqlBuilder = new SQLiteCommandBuilder(adapter);
                int number = command.ExecuteNonQuery();
                if (number == 0)
                    MessageBox.Show("Команда не выполнена");
                else
                    MessageBox.Show("Команда выполнена");
            }
            catch 
            {
                MessageBox.Show("Ошибка сохранения, проверьте заполнены ли поля");
            }
            finally
            {
                if (connect != null)
                    connect.Close();
            }
            NavigationService.GoBack();
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            updateBD();
        }
    }
}
