using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace PassengerCarCompany.Windows
{
    public partial class StopsOnTheRouteWindow : Window
    {
        StopsOnTheRoute selectedStopsOnTheRoute;
        ObservableCollection<StopsOnTheRoute> lstStopsOnTheRoute;

        #region Загрузка окна

        public StopsOnTheRouteWindow()
        {
            InitializeComponent();

            // Контекст данных для блока доп. информации.
            selectedStopsOnTheRoute = new StopsOnTheRoute();
            gboxCurrStopsOnTheRoute.DataContext = selectedStopsOnTheRoute;

            // Выбрать первую строку таблицы.
            dgridStopsOnTheRoute.SelectedIndex = 0;
        }

        #endregion

        #region Работа со списком данных

        // Заполнение списка.
        private void UpdateData(object sender, RoutedEventArgs e)
        {
            lstStopsOnTheRoute = new ObservableCollection<StopsOnTheRoute>(StopsOnTheRoute.Get());
            dgridStopsOnTheRoute.ItemsSource = lstStopsOnTheRoute;
            lstStopsOnTheRoute.CollectionChanged += lstStopsOnTheRoute_CollectionChanged;
        }

        // Событие изменения списка данных.
        private void lstStopsOnTheRoute_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            // При удалении элемента.
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                (e.OldItems[0] as StopsOnTheRoute).Delete();
            }
            // При добавлении элемента.
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                (e.NewItems[0] as StopsOnTheRoute).Insert();
            }
            // При изменении элемента.
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Replace)
            {
                (e.OldItems[0] as StopsOnTheRoute).Update(e.NewItems[0] as StopsOnTheRoute);
            }
        }

        #endregion

        #region Подробная информация

        // При выборе строки вывести подробную информацию.
        private void dgridStopsOnTheRoute_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count <= 0 || dgridStopsOnTheRoute.SelectedItems.Count > 1 || dgridStopsOnTheRoute.Items.IndexOf(e.AddedItems[0]) >= lstStopsOnTheRoute.Count)
                return;

            selectedStopsOnTheRoute.Number = ((StopsOnTheRoute)e.AddedItems[0]).Number;
            selectedStopsOnTheRoute.RouteNumber = ((StopsOnTheRoute)e.AddedItems[0]).RouteNumber;
            selectedStopsOnTheRoute.StopId = ((StopsOnTheRoute)e.AddedItems[0]).StopId;
        }

        private void ValidationError(object sender, ValidationErrorEventArgs e)
        {
            int selected = dgridStopsOnTheRoute.SelectedIndex;

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
            int selected = dgridStopsOnTheRoute.SelectedIndex;

            if (selected != -1)
            {
                // Количество выбранных элементов.
                int count = dgridStopsOnTheRoute.SelectedItems.Count;
                if (count > lstStopsOnTheRoute.Count)
                    count = lstStopsOnTheRoute.Count;

                for (int i = 0; i < count; i++)
                {
                    // Удаляем из теблицы.
                    lstStopsOnTheRoute.Remove((StopsOnTheRoute)dgridStopsOnTheRoute.SelectedItem);
                }
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (tbErrorInfo.Text != string.Empty)
                return;

            if (StopsOnTheRoute.Get(selectedStopsOnTheRoute.Number, selectedStopsOnTheRoute.RouteNumber) != null)
            {
                MessageBox.Show("Такая остановка уже есть в БД", "Ошибка");
                return;
            }

            lstStopsOnTheRoute.Add((StopsOnTheRoute)selectedStopsOnTheRoute.Clone());
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (StopsOnTheRoute.Get(selectedStopsOnTheRoute.Number, selectedStopsOnTheRoute.RouteNumber) != null)
            {
                MessageBox.Show("Такая остановка уже есть в БД", "Ошибка");
                return;
            }

            int selected = dgridStopsOnTheRoute.SelectedIndex;
            if (selected != -1)
            {
                lstStopsOnTheRoute[selected] = (StopsOnTheRoute)selectedStopsOnTheRoute.Clone();
            }
        }

        // Восстановить таблицу автобусов.
        private void btnRestoreTable_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new PassengerCarCompanyEntities())
            {
                db.Database.ExecuteSqlCommand("DELETE FROM StopsOnTheRoute");
                db.Database.ExecuteSqlCommand(@"INSERT INTO StopsOnTheRoute(Number, RouteNumber, StopId)
			                                    VALUES(1,  2, 1),
			                                    	  (2,  2, 2),
			                                    	  (3,  2, 3),
			                                    	  (4,  2, 4),
			                                    	  (5,  2, 5),
			                                    	  (6,  2, 6),
			                                    	  (7,  2, 7),
			                                    	  (8,  2, 8),
			                                    	  (9,  2, 9),
			                                    	  (10, 2, 10),
			                                    	  (11, 2, 11),
			                                    	  (12, 2, 12),
			                                    	  (13, 2, 13),
			                                    	  (14, 2, 6),
			                                    	  (15, 2, 5),
			                                    	  (16, 2, 4),
			                                    	  (17, 2, 14),
			                                    	  (18, 2, 2),
			                                    	  (19, 2, 1),

			                                    	  (1,  20, 15),
			                                    	  (2,  20, 16),
			                                    	  (3,  20, 17),
			                                    	  (4,  20, 18),
			                                    	  (5,  20, 13),
			                                    	  (6,  20, 7),
			                                    	  (7,  20, 8),
			                                    	  (8,  20, 9),
			                                    	  (9,  20, 19),
			                                    	  (10, 20, 20),
			                                    	  (11, 20, 21),
			                                    	  (12, 20, 22),
			                                    	  (13, 20, 23),

			                                    	  (1,  41, 24),
			                                    	  (2,  41, 2),
			                                    	  (3,  41, 1),
			                                    	  (4,  41, 25),
			                                    	  (5,  41, 26),
			                                    	  (6,  41, 27),
			                                    	  (7,  41, 4),
			                                    	  (8,  41, 5),
			                                    	  (9,  41, 6),
			                                    	  (10, 41, 7),
			                                    	  (11, 41, 8),
			                                    	  (12, 41, 9),
			                                    	  (13, 41, 10),
			                                    	  (14, 41, 11),
			                                    	  (15, 41, 28),
			                                    	  (16, 41, 17),
			                                    	  (17, 41, 18),
			                                    	  (18, 41, 13),
			                                    	  (19, 41, 6),
			                                    	  (20, 41, 5),
			                                    	  (21, 41, 4),
			                                    	  (22, 41, 27),
			                                    	  (23, 41, 26),
			                                    	  (24, 41, 25),
			                                    	  (25, 41, 1),
			                                    	  (26, 41, 29),
			                                    	  (27, 41, 24)");
            }

            UpdateData(sender, e);
        }

        #endregion
    }
}
