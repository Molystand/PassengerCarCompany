using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace PassengerCarCompany.Windows
{
    public partial class RouteWindow : Window
    {
        Route selectedRoute;
        ObservableCollection<Route> lstRoutes;

        #region Загрузка окна

        public RouteWindow()
        {
            InitializeComponent();

            // Контекст данных для блока доп. информации.
            selectedRoute = new Route();
            gboxCurrRoute.DataContext = selectedRoute;

            // Выбрать первую строку таблицы.
            dgridRoute.SelectedIndex = 0;
        }

        #endregion

        #region Работа со списком данных

        // Заполнение списка.
        private void UpdateData(object sender, RoutedEventArgs e)
        {
            lstRoutes = new ObservableCollection<Route>(Route.Get());
            dgridRoute.ItemsSource = lstRoutes;
            lstRoutes.CollectionChanged += LstRoutes_CollectionChanged;
        }

        // Событие изменения списка данных.
        private void LstRoutes_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            // При удалении элемента.
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                (e.OldItems[0] as Route).Delete();
            }
            // При добавлении элемента.
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                (e.NewItems[0] as Route).Insert();
            }
            // При изменении элемента.
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Replace)
            {
                (e.OldItems[0] as Route).Update(e.NewItems[0] as Route);
            }
        }

        #endregion

        #region Подробная информация

        // При выборе строки вывести подробную информацию.
        private void dgridRoute_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count <= 0 || dgridRoute.SelectedItems.Count > 1 || dgridRoute.Items.IndexOf(e.AddedItems[0]) >= lstRoutes.Count)
                return;

            selectedRoute.Number         = ((Route)e.AddedItems[0]).Number;
            selectedRoute.Title          = ((Route)e.AddedItems[0]).Title;
            selectedRoute.RouteLength    = ((Route)e.AddedItems[0]).RouteLength;
            selectedRoute.AverTravelTime = ((Route)e.AddedItems[0]).AverTravelTime;
        }

        private void ValidationError(object sender, ValidationErrorEventArgs e)
        {
            int selected = dgridRoute.SelectedIndex;

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
            int selected = dgridRoute.SelectedIndex;

            if (selected != -1)
            {
                // Количество выбранных элементов.
                int count = dgridRoute.SelectedItems.Count;
                if (count > lstRoutes.Count)
                    count = lstRoutes.Count;

                for (int i = 0; i < count; i++)
                {
                    // Удаляем из теблицы.
                    lstRoutes.Remove((Route)dgridRoute.SelectedItem);
                }
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (tbErrorInfo.Text != string.Empty)
                return;

            if (Route.Get(selectedRoute.Number) != null)
            {
                MessageBox.Show("Маршрут с таким номером уже есть в БД", "Ошибка");
                return;
            }
            if (Route.Get(selectedRoute.Title) != null)
            {
                MessageBox.Show("Маршрут с таким названием уже есть в БД", "Ошибка");
                return;
            }

            lstRoutes.Add((Route)selectedRoute.Clone());
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            int selected = dgridRoute.SelectedIndex;
            if (selected != -1)
            {
                lstRoutes[selected] = (Route)selectedRoute.Clone();
            }
        }

        // Восстановить таблицу автобусов.
        private void btnRestoreTable_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new PassengerCarCompanyEntities())
            {
                db.Database.ExecuteSqlCommand("DELETE FROM Route");
                db.Database.ExecuteSqlCommand(@"INSERT INTO Route(Number, Title, RouteLength, AverTravelTime)
			                                    VALUES(2,  'Железнодорожный вокзал - Технологический университет', 10, '00:41:00'),
			                                    	  (20, 'Спутник - Микрорайон «Новая Заря»',					   14, '00:40:00'),
			                                    	  (41, 'Рынок «Салют» - Технологический университет',		   23, '01:00:00')");
            }

            UpdateData(sender, e);
        }

        #endregion
    }
}
