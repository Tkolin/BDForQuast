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
    /// Логика взаимодействия для AddEditCatalog.xaml
    /// </summary>
    public partial class AddEditCatalog : Page
    {
        private SQLiteConnection connect
            = new SQLiteConnection(@"data source = DB.db");
        bool updateType = false;
        DataRowView catalog;


        public AddEditCatalog()
        {
            InitializeComponent();
        }

        public AddEditCatalog(DataRowView catalog)
        {
            InitializeComponent();

            nameBox.Text = catalog.Row["Name"].ToString();
            priceBox.Text = catalog.Row["Price"].ToString();
            updateType = true;

            this.catalog = catalog;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            DownloadTypes();
        }




        //Добавление название услуг
        DataTable types = new DataTable();
        public void DownloadTypes()
        {
            connect.Open();
            string sql = "SELECT * from ServiceTypes";
            SQLiteCommand command = new SQLiteCommand(sql, connect);
            SQLiteDataAdapter adapter = new SQLiteDataAdapter(command);
            adapter.Fill(types);
            typeBox.ItemsSource = types.DefaultView;
            connect.Close();
     
        }
        //Сохронение базы
        public void updateBD()
        {
            connect.Open();
            SQLiteCommand command = new SQLiteCommand();
            command.Connection = connect;

            if (updateType)
                command.CommandText = @"UPDATE 'main'.'Services' SET " +

                              "'Name' = " + "'" + nameBox.Text + "'" +
                              ",'TypeID' = " + typeBox.SelectedValue+
                              ",'Price' = "  + priceBox.Text +
                              " WHERE 'Services'.'ID' =" + catalog.Row["ID"];
            else
                command.CommandText = @"INSERT INTO  'main'.'Services' " +
                            "('Name' ,'TypeID', 'Price')" +
                            "VALUES ('" + nameBox.Text + "'," + 
                            typeBox.SelectedValue+ "," + priceBox.Text  + ")";
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
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}

