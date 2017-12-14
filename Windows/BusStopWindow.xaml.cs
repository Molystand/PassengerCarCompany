using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace PassengerCarCompany.Windows
{
    public partial class BusStopWindow : Window
    {
        BusStop selectedBusStop;
        ObservableCollection<BusStop> lstBusStop;

        #region Загрузка окна

        public BusStopWindow()
        {
            InitializeComponent();

            // Контекст данных для блока доп. информации.
            selectedBusStop = new BusStop();
            gboxCurrBusStop.DataContext = selectedBusStop;

            // Выбрать первую строку таблицы.
            dgridBusStop.SelectedIndex = 0;
        }

        #endregion

        #region Работа со списком данных

        // Заполнение списка.
        private void UpdateData(object sender, RoutedEventArgs e)
        {
            lstBusStop = new ObservableCollection<BusStop>(BusStop.Get());
            dgridBusStop.ItemsSource = lstBusStop;
            lstBusStop.CollectionChanged += lstBusStop_CollectionChanged;
        }

        // Событие изменения списка данных.
        private void lstBusStop_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            // При удалении элемента.
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                (e.OldItems[0] as BusStop).Delete();
            }
            // При добавлении элемента.
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                (e.NewItems[0] as BusStop).Insert();
            }
            // При изменении элемента.
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Replace)
            {
                (e.OldItems[0] as BusStop).Update(e.NewItems[0] as BusStop);
            }
        }

        #endregion

        #region Подробная информация

        // При выборе строки вывести подробную информацию.
        private void dgridBusStop_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count <= 0 || dgridBusStop.SelectedItems.Count > 1 || dgridBusStop.Items.IndexOf(e.AddedItems[0]) >= lstBusStop.Count)
                return;

            //selectedBusStop.Id = ((BusStop)e.AddedItems[0]).Id;
            selectedBusStop.Title = ((BusStop)e.AddedItems[0]).Title;
        }

        private void ValidationError(object sender, ValidationErrorEventArgs e)
        {
            int selected = dgridBusStop.SelectedIndex;

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
            int selected = dgridBusStop.SelectedIndex;

            if (selected != -1)
            {
                // Количество выбранных элементов.
                int count = dgridBusStop.SelectedItems.Count;
                if (count > lstBusStop.Count)
                    count = lstBusStop.Count;

                for (int i = 0; i < count; i++)
                {
                    // Удаляем из теблицы.
                    lstBusStop.Remove((BusStop)dgridBusStop.SelectedItem);
                }
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (tbErrorInfo.Text != string.Empty)
                return;

            if (BusStop.Get(selectedBusStop.Title) != null)
            {
                MessageBox.Show("Остановка с таким названием уже есть в БД", "Ошибка");
                return;
            }

            lstBusStop.Add((BusStop)selectedBusStop.Clone());
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (tbErrorInfo.Text != string.Empty)
                return;

            int selected = dgridBusStop.SelectedIndex;
            if (selected != -1)
            {
                lstBusStop[selected] = (BusStop)selectedBusStop.Clone();
            }
        }

        // Восстановить таблицу автобусов.
        private void btnRestoreTable_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new PassengerCarCompanyEntities())
            {
                db.Database.ExecuteSqlCommand("DELETE FROM BusStop");
                db.Database.ExecuteSqlCommand(@"INSERT INTO BusStop(Title)
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
