using System;
using System.Collections.Generic;
using System.Data;
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
    /// Логика взаимодействия для Print.xaml
    /// </summary>
    public partial class Print : Page
    {
        
        public Print(string startDate, string endDate, DataTable tb)
        {
            InitializeComponent();
            dataLable.Text += "от: " + startDate + "до: " + endDate;
            grid.ItemsSource = tb.DefaultView;                      
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //Скрытие
            btns.Visibility = Visibility.Collapsed;
            //Вывод окна печати (текущей формы)
            PrintDialog printDialog = new PrintDialog();
            printDialog.PrintVisual(this, "Печать");
            //Возвращеие отображения
            btns.Visibility = Visibility.Visible;
        }
    }
}
