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
    /// Логика взаимодействия для AddEditQuast.xaml
    /// </summary>
    public partial class AddEditQuast : Page
    {
        //расположение бд в папке debug(корень приложения)
        private SQLiteConnection connect = new SQLiteConnection(@"data source = DB.db");
        private bool updateType = false;
        private DataRowView application;

        public AddEditQuast()
        {
            InitializeComponent();
        }
        public AddEditQuast(DataRowView application)
        {
            InitializeComponent();          
            priceBox.Text = application.Row["Price"].ToString();
            dateStart.SelectedDate = Convert.ToDateTime(application.Row["DatePayment"].ToString());
            dateEnd.SelectedDate = Convert.ToDateTime(application.Row["DateCompletion"].ToString());
            updateType = true;
            this.application = application;
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            DownloadTypes();
            //заполнения комбобоксов
            if (updateType)
            {
                userBox.SelectedValue =
                    Convert.ToString(application.Row["UserID"].ToString());
                servisBox.SelectedValue =
                    Convert.ToString(application.Row["ServiceID"].ToString());
                statusBox.SelectedValue =
                    Convert.ToString(application.Row["StatusID"].ToString());
            }

        }

        //Добавление названий пользователей, услуг и статуса
        private DataTable users = new DataTable();
        private DataTable services = new DataTable();
        private DataTable status = new DataTable();
        public void DownloadTypes()
        {
            connect.Open();
            List<string> tabelsName = new List<string>() {"Users", "Statuses", "Services" };
            List<DataTable> tables = new List<DataTable> {users, services, status };
            List<ComboBox> comboBox = new List<ComboBox>() {userBox,statusBox,servisBox };
            for (int i = 0; i < tabelsName.Count; i++)
            {
                string sql = "SELECT * from "+tabelsName[i];
                SQLiteCommand command = new SQLiteCommand(sql, connect);
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(command);
                adapter.Fill(tables[i]);
                comboBox[i].ItemsSource = tables[i].DefaultView;
            }
            connect.Close();

        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            updateBD();
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
                command.CommandText = @"UPDATE 'main'.'Applications' SET " +
                                  "'UserID' = " + "'" + userBox.SelectedValue + "'" +
                                  ",'ServiceID' = " + servisBox.SelectedValue +
                                  ",'StatusID' = " + statusBox.SelectedValue +
                                  ",'DatePayment' = " + dateStart.SelectedDate +
                                  ",'DateCompletion' = " + dateEnd.SelectedDate +
                                  ",'Price' = " + priceBox.Text +
                                  " WHERE 'Applications'.'ID' =" + application.Row["ID"];
            else
                command.CommandText = @"INSERT INTO  'main'.'Applications' " +
                                "('UserID' ,'ServiceID', 'StatusID', 'DatePayment', 'DateCompletion', 'Price')" +
                                "VALUES " +
                                "("+userBox.SelectedValue+"," +servisBox.SelectedValue+","+ statusBox.SelectedValue+ ",'"+
                                dateStart.SelectedDate+"',"+dateEnd.SelectedDate+",'"+priceBox.Text+"' )";
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


    }
}
