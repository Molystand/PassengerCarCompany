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
                db.Database.ExecuteSqlCommand(@"INSERT INTO StopsOnTheRoute(Title)
			                                    VALUES('Железнодорожный вокзал'),
			                                    	  ('Белгородского полка'),
			                                    	  ('Кинотеатр Победа'),
			                                    	  ('Свято-троицкий бульвар'),
			                                    	  ('Водстрой'),
			                                    	  ('5 Августа'),
			                                    	  ('Магазин Океан'),
			                                    	  ('4-й микрорайон'),
			                                    	  ('Технологический университет'),
			                                    	  ('Подстанция'),
			                                    	  ('Губкина'),
			                                    	  ('ДС Космос'),
			                                    	  ('Мокроусова'),
			                                    	  --('5 Августа'),
			                                    	  --('Водстрой'),
			                                    	  --('Свято-троицкий бульвар'),
			                                    	  ('Гастроном Заря'),
			                                    	  --('Белгородского полка'),
			                                    	  --('Железнодорожный вокзал'),
			                                    	  
			                                    	  --15
			                                    	  ('Спутник'),
			                                    	  ('Горбольница №2'),
			                                    	  ('Щорса'),
			                                    	  ('Королёва'),
			                                    	  --('Мокроусова'),
			                                    	  --('Магазин Океан'),
			                                    	  --('4-й микрорайон'),
			                                    	  --('Технологический университет'),
			                                    	  ('Мясокомбинат'),
			                                    	  ('Хлебобаза'),
			                                    	  ('Микрорайон Заря'),
			                                    	  ('Почтовая'),
			                                    	  ('Микрорайон Новая заря'),

			                                    	  --24
			                                    	  ('Рынок Салют'),
			                                    	  --('Белгородского полка'),
			                                    	  --('Железнодорожный вокзал'),
			                                    	  ('Скорая помощь'),
			                                    	  ('Центральный рынок'),
			                                    	  ('Стадион'),
			                                    	  --('Свято-троицкий бульвар'),
			                                    	  --('Водстрой'),
			                                    	  --('5 Августа'),
			                                    	  --('Магазин Океан'),
			                                    	  --('4-й микрорайон'),
			                                    	  --('Технологический университет'),
			                                    	  --('Подстанция'),
			                                    	  --('Губкина'),
			                                    	  ('Шаландина'),
			                                    	  --('Щорса'),
			                                    	  --('Королёва'),
			                                    	  --('Мокроусова'),
			                                    	  --('5 Августа'),
			                                    	  --('Водстрой'),
			                                    	  --('Свято-троицкий бульвар'),
			                                    	  --('Стадион'),
			                                    	  --('Центральный рынок'),
			                                    	  --('Скорая помощь'),
			                                    	  --('Железнодорожный вокзал'),
			                                    	  ('Универсам')
			                                    	  --('Рынок Салют')");
            }

            UpdateData(sender, e);
        }

        #endregion
    }
}
