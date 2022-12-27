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
    /// Логика взаимодействия для QuastFilter.xaml
    /// </summary>
    public partial class QuastFilter : Page
    {
        private DataTable quast = new DataTable();

        public QuastFilter(DataTable quast)
        {
            InitializeComponent();
            this.quast = quast;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
        DataTable quastFilter;//Отсортированная таблица
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //Если временной интервал не определён прерываем выполнение
            if (startBox.SelectedDate == null || endBox.SelectedDate == null) { return; }
            //Переход на страницу печати (Даты обрезаються до 10ти знаков)
            NavigationService.Navigate (new Print(startBox.SelectedDate.ToString().Remove(10), 
                endBox.SelectedDate.ToString().Remove(10), quastFilter));
        }
        public void Filter(DateTime start, DateTime end)
        {
            quastFilter = quast.Copy();//Копирование таблицы заявок
            quastFilter.Rows.Clear();//Отчистка данных в новой таблице
            foreach (DataRow item in quast.Rows) //беребор всех заявок
            {
                //Если время соответствует промежутку
                if (Convert.ToDateTime(item["DatePayment"]) >= start && 
                    Convert.ToDateTime(item["DateCompletion"]) <= end)
                    quastFilter.ImportRow(item);//добавляем в новую пустую таблицу данные
            }
            grid.ItemsSource = quastFilter.DefaultView;//Выводим таблицу в DataGrid
        }

        private void filterButt_Click(object sender, RoutedEventArgs e)
        {
            //Если временной интервал не определён 
            if(startBox.SelectedDate == null || endBox.SelectedDate == null) { return; }

            Filter((DateTime)startBox.SelectedDate, (DateTime)endBox.SelectedDate);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
         }
    }
}
