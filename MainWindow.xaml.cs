using System.Windows;
using PassengerCarCompany.Windows;

namespace PassengerCarCompany
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Загрузка окна

        public MainWindow()
        {
            InitializeComponent();
        }

        #endregion

        #region Клики

        private void btnBusTable_Click(object sender, RoutedEventArgs e)
        {
            BusWindow window = new BusWindow()
            {
                Owner = this
            };
            window.ShowDialog();
        }

        private void btnBusStopTable_Click(object sender, RoutedEventArgs e)
        {
            BusStopWindow window = new BusStopWindow()
            {
                Owner = this
            };
            window.ShowDialog();
        }


        private void btnDriverTable_Click(object sender, RoutedEventArgs e)
        {
            DriverWindow window = new DriverWindow()
            {
                Owner = this
            };
            window.ShowDialog();
        }

        private void btnRouteTable_Click(object sender, RoutedEventArgs e)
        {
            RouteWindow window = new RouteWindow()
            {
                Owner = this
            };
            window.ShowDialog();
        }

        private void btnRouteSheetTable_Click(object sender, RoutedEventArgs e)
        {
            RouteSheetWindow window = new RouteSheetWindow()
            {
                Owner = this
            };
            window.ShowDialog();
        }

        private void btnStopsOnTheRouteTable_Click(object sender, RoutedEventArgs e)
        {
            StopsOnTheRouteWindow window = new StopsOnTheRouteWindow()
            {
                Owner = this
            };
            window.ShowDialog();
        }

        #endregion
    }
}
