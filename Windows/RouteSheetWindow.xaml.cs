using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace PassengerCarCompany.Windows
{
    public partial class RouteSheetWindow : Window
    {
        RouteSheet selectedRouteSheet;
        ObservableCollection<RouteSheet> lstRouteSheets;

        #region Загрузка окна

        public RouteSheetWindow()
        {
            InitializeComponent();

            // Контекст данных для блока доп. информации.
            selectedRouteSheet = new RouteSheet();
            gboxCurrRouteSheet.DataContext = selectedRouteSheet;

            // Выбрать первую строку таблицы.
            dgridRouteSheet.SelectedIndex = 0;
        }

        #endregion

        #region Работа со списком данных

        // Заполнение списка.
        private void UpdateData(object sender, RoutedEventArgs e)
        {
            lstRouteSheets = new ObservableCollection<RouteSheet>(RouteSheet.Get());
            dgridRouteSheet.ItemsSource = lstRouteSheets;
            lstRouteSheets.CollectionChanged += lstRouteSheets_CollectionChanged;
        }

        // Событие изменения списка данных.
        private void lstRouteSheets_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            // При удалении элемента.
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                (e.OldItems[0] as RouteSheet).Delete();
            }
            // При добавлении элемента.
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                (e.NewItems[0] as RouteSheet).Insert();
            }
            // При изменении элемента.
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Replace)
            {
                (e.OldItems[0] as RouteSheet).Update(e.NewItems[0] as RouteSheet);
            }
        }

        #endregion

        #region Подробная информация

        // При выборе строки вывести подробную информацию.
        private void dgridRouteSheet_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count <= 0 || dgridRouteSheet.SelectedItems.Count > 1 || dgridRouteSheet.Items.IndexOf(e.AddedItems[0]) >= lstRouteSheets.Count)
                return;

            //selectedRouteSheet.Id            = ((RouteSheet)e.AddedItems[0]).Id;
            selectedRouteSheet.Number        = ((RouteSheet)e.AddedItems[0]).Number;
            selectedRouteSheet.Date          = ((RouteSheet)e.AddedItems[0]).Date;
            selectedRouteSheet.DepartureTime = ((RouteSheet)e.AddedItems[0]).DepartureTime;
            selectedRouteSheet.ArrivalTime   = ((RouteSheet)e.AddedItems[0]).ArrivalTime;
            selectedRouteSheet.PlannedProfit = ((RouteSheet)e.AddedItems[0]).PlannedProfit;
            selectedRouteSheet.RealProfit    = ((RouteSheet)e.AddedItems[0]).RealProfit;
            selectedRouteSheet.DriverNumber  = ((RouteSheet)e.AddedItems[0]).DriverNumber;
            selectedRouteSheet.RouteNumber   = ((RouteSheet)e.AddedItems[0]).RouteNumber;
            selectedRouteSheet.BusNumber     = ((RouteSheet)e.AddedItems[0]).BusNumber;
        }

        private void ValidationError(object sender, ValidationErrorEventArgs e)
        {
            int selected = dgridRouteSheet.SelectedIndex;

            tbErrorInfo.Text = string.Empty;
            foreach (object child in LogicalTreeHelper.GetChildren(gridInfo))
            {
                // Для всех TextBox из потомков gridInfo.
                TextBox element = child as TextBox;
                if (element == null)
                    continue;

                // Если есть ошибки.
                if (Validation.GetHasError(element))
                {
                    // Вывести все ошибки.
                    foreach (var error in Validation.GetErrors(element))
                    {
                        tbErrorInfo.Text += $"{error.ErrorContent.ToString()}\n";
                    }
                }
            }
        }

        #endregion

        #region Клики

        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
            // Индекс первого выбранного элемента.
            int selected = dgridRouteSheet.SelectedIndex;

            if (selected != -1)
            {
                // Количество выбранных элементов.
                int count = dgridRouteSheet.SelectedItems.Count;
                if (count > lstRouteSheets.Count)
                    count = lstRouteSheets.Count;

                for (int i = 0; i < count; i++)
                {
                    // Удаляем из теблицы.
                    lstRouteSheets.Remove((RouteSheet)dgridRouteSheet.SelectedItem);
                }
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (tbErrorInfo.Text != string.Empty)
                return;

            if (RouteSheet.Get(selectedRouteSheet.Number, selectedRouteSheet.Date) != null)
            {
                MessageBox.Show("Маршрутный лист с таким номером и датой уже есть в БД", "Ошибка");
                return;
            }

            if (Driver.Get(selectedRouteSheet.DriverNumber) == null)
            {
                MessageBox.Show("Водителя с таким номером не существует в БД", "Ошибка");
                return;
            }
            if (Route.Get(selectedRouteSheet.RouteNumber) == null)
            {
                MessageBox.Show("Маршрута с таким номером не существует в БД", "Ошибка");
                return;
            }
            if (Bus.Get(selectedRouteSheet.BusNumber) == null)
            {
                MessageBox.Show("Автобуса с таким номером не существует в БД", "Ошибка");
                return;
            }

            lstRouteSheets.Add((RouteSheet)selectedRouteSheet.Clone());
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            //if (RouteSheet.Get(selectedRouteSheet.Number, selectedRouteSheet.Date) != null)
            //{
            //    MessageBox.Show("Маршрутный лист с таким номером и датой уже есть в БД", "Ошибка");
            //    return;
            //}

            if (Driver.Get(selectedRouteSheet.DriverNumber) == null)
            {
                MessageBox.Show("Водителя с таким номером не существует в БД", "Ошибка");
                return;
            }
            if (Route.Get(selectedRouteSheet.RouteNumber) == null)
            {
                MessageBox.Show("Маршрута с таким номером не существует в БД", "Ошибка");
                return;
            }
            if (Bus.Get(selectedRouteSheet.BusNumber) == null)
            {
                MessageBox.Show("Автобуса с таким номером не существует в БД", "Ошибка");
                return;
            }

            int selected = dgridRouteSheet.SelectedIndex;
            if (selected != -1)
            {
                try
                {
                    lstRouteSheets[selected] = (RouteSheet)selectedRouteSheet.Clone();
                }
                catch (Exception ex)
                {
                    UpdateData(sender, e);
                }
            }
        }

        // Восстановить таблицу автобусов.
        private void btnRestoreTable_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new PassengerCarCompanyEntities())
            {
                db.Database.ExecuteSqlCommand("DELETE FROM RouteSheet");
                db.Database.ExecuteSqlCommand(@"INSERT INTO RouteSheet(Number, Date, DepartureTime, ArrivalTime, PlannedProfit, RealProfit, DriverNumber, RouteNumber, BusNumber)
			                                    VALUES(1, '2017-06-19', '06:11', '07:15', 10000, 8830,  168, 41, 'ч823ор'),
			                                    	  (2, '2017-06-19', '10:52', '11:40', 7000,  8775,  204, 20, 'о548лд'),
			                                    	  (3, '2017-06-19', '16:30', '17:05', 8500,  8330,  168, 2,  'ж784не'),
			                                    	  (4, '2017-06-19', '16:50', '17:34', 7500,  7905,  204, 2,  'и134да'),
			                                    	  (5, '2017-06-19', '20:40', '21:15', 7000,  7010,  125, 20, 'ч823ор'),
			                                    	  (1, '2017-08-13', '07:20', '08:03', 9000,  9920,  246, 41, 'ж784не'),
			                                    	  (2, '2017-08-13', '20:23', '20:59', 8000,  8360,  159, 2,  'п456ва'),
			                                    	  (3, '2017-08-13', '20:50', '21:38', 7900,  8470,  125, 20, 'е432ыф'),
			                                    	  (1, '2017-07-26', '07:09', '07:52', 8300,  7950,  193, 20, 'и134да'),
			                                    	  (2, '2017-07-26', '09:46', '10:50', 10500, 10670, 219, 41, 'е432ыф'),
			                                    	  (3, '2017-07-26', '16:44', '17:31', 9500,  9160,  168, 2,  'п456ва'),
			                                    	  (4, '2017-07-26', '19:12', '20:10', 7500,  7975,  212, 41, 'и134да'),
			                                    	  (1, '2017-06-13', '07:40', '08:21', 8400,  8450,  159, 2,  'п456ва'),
			                                    	  (2, '2017-06-13', '14:04', '14:50', 7400,  7345,  212, 20, 'ч823ор'),
			                                    	  (3, '2017-06-13', '15:49', '16:27', 8200,  8055,  249, 2,  'т134уц'),
			                                    	  (4, '2017-06-13', '21:44', '22:30', 6900,  7360,  141, 20, 'ч823ор'),
			                                    	  (1, '2017-07-21', '11:53', '12:25', 8100,  8980,  172, 2,  'п456ва'),
			                                    	  (2, '2017-07-21', '15:02', '16:30', 9800,  9810,  249, 41, 'ф083ув'),
			                                    	  (3, '2017-07-21', '18:53', '19:55', 8300,  8095,  219, 41, 'п456ва'),
			                                    	  (1, '2017-05-26', '09:36', '10:04', 10500, 10600, 174, 20, 'п456ва'),
			                                    	  (2, '2017-05-26', '14:02', '14:50', 9000,  9275,  212, 20, 'у204то'),
			                                    	  (3, '2017-05-26', '14:47', '15:29', 9500,  9310,  159, 20, 'п454тр'),
			                                    	  (1, '2017-07-06', '12:26', '13:21', 8400,  8305,  176, 41, 'т134уц'),
			                                    	  (2, '2017-07-06', '16:16', '17:10', 7600,  7325,  246, 41, 'п454тр'),
			                                    	  (3, '2017-07-06', '18:32', '19:11', 7100,  7505,  176, 20, 'ч823ор'),
			                                    	  (4, '2017-07-06', '19:17', '19:50', 8000,  8550,  141, 2,  'з645ал'),
			                                    	  (1, '2017-06-13', '08:40', '09:55', 9000,  8800,  172, 41, 'т134уц'),
			                                    	  (2, '2017-06-13', '15:29', '16:12', 8200,  8110,  171, 2,  'ж784не'),
			                                    	  (3, '2017-06-13', '16:12', '17:37', 8100,  8410,  249, 41, 'у204то'),
			                                    	  (4, '2017-06-13', '21:27', '22:15', 7600,  7340,  141, 20, 'п454тр'),
			                                    	  (5, '2017-06-13', '21:40', '22:13', 7200,  7150,  174, 2,  'к683вы'),
			                                    	  (1, '2017-08-23', '11:09', '11:52', 9900,  10525, 193, 2,  'с859вд'),
			                                    	  (2, '2017-08-23', '15:40', '16:21', 8900,  9130,  171, 20, 'з645ал'),
			                                    	  (3, '2017-08-23', '18:44', '19:30', 7500,  7875,  176, 2,  'у204то'),
			                                    	  (4, '2017-08-23', '20:52', '21:38', 7000,  7920,  249, 2,  'к683вы'),
			                                    	  (1, '2017-08-02', '09:15', '09:49', 9400,  10650, 290, 2,  'о548лд'),
			                                    	  (2, '2017-08-02', '12:22', '13:20', 8300,  8290,  169, 41, 'г752сн'),
			                                    	  (3, '2017-08-02', '17:26', '18:14', 10200, 10385, 290, 41, 'к683вы'),
			                                    	  (1, '2017-09-12', '12:42', '13:23', 7600,  7900,  216, 2,  'с859вд'),
			                                    	  (2, '2017-09-12', '14:37', '15:14', 8000,  8185,  228, 2,  'г752сн'),
			                                    	  (3, '2017-09-12', '19:10', '19:58', 9700,  9840,  216, 20, 'ф083ув')");
            }

            UpdateData(sender, e);
        }

        #endregion
    }
}
