using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace PassengerCarCompany.Windows
{
    public partial class BusWindow : Window
    {
        Bus selectedBus;
        ObservableCollection<Bus> lstBuses;

        #region Загрузка окна

        public BusWindow()
        {
            InitializeComponent();

            // Контекст данных для блока доп. информации.
            selectedBus = new Bus();
            gboxCurrBus.DataContext = selectedBus;

            // Выбрать первую строку таблицы.
            dgridBus.SelectedIndex = 0;
        }

        #endregion

        #region Работа со списком данных

        // Заполнение списка.
        private void UpdateData(object sender, RoutedEventArgs e)
        {
            lstBuses = new ObservableCollection<Bus>(Bus.Get());
            dgridBus.ItemsSource = lstBuses;
            lstBuses.CollectionChanged += LstBuses_CollectionChanged;
        }

        // Событие изменения списка данных.
        private void LstBuses_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            // При удалении элемента.
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                (e.OldItems[0] as Bus).Delete();
            }
            // При добавлении элемента.
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                (e.NewItems[0] as Bus).Insert();
            }
            // При изменении элемента.
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Replace)
            {
                (e.OldItems[0] as Bus).Update(e.NewItems[0] as Bus);
            }
        }

        #endregion

        #region Подробная информация

        // При выборе строки вывести подробную информацию.
        private void dgridBus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count <= 0 || dgridBus.SelectedItems.Count > 1 || dgridBus.Items.IndexOf(e.AddedItems[0]) >= lstBuses.Count)
                return;

            selectedBus.Number = ((Bus)e.AddedItems[0]).Number;
            selectedBus.Mark = ((Bus)e.AddedItems[0]).Mark;
            selectedBus.ReleaseYear = ((Bus)e.AddedItems[0]).ReleaseYear;
            selectedBus.Capacity = ((Bus)e.AddedItems[0]).Capacity;
        }

        private void ValidationError(object sender, ValidationErrorEventArgs e)
        {
            int selected = dgridBus.SelectedIndex;

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
            int selected = dgridBus.SelectedIndex;

            if (selected != -1)
            {
                // Количество выбранных элементов.
                int count = dgridBus.SelectedItems.Count;
                if (count > lstBuses.Count)
                    count = lstBuses.Count;

                for (int i = 0; i < count; i++)
                {
                    // Удаляем из теблицы.
                    lstBuses.Remove((Bus)dgridBus.SelectedItem);
                }
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (tbErrorInfo.Text != string.Empty)
                return;

            if (Bus.Get(selectedBus.Number) != null)
            {
                MessageBox.Show("Автобус с таким номером уже есть в БД", "Ошибка");
                return;
            }
                
            lstBuses.Add((Bus)selectedBus.Clone());
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (tbErrorInfo.Text != string.Empty)
                return;

            int selected = dgridBus.SelectedIndex;
            if (selected != -1)
            {
                try
                {
                    lstBuses[selected] = (Bus)selectedBus.Clone();
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
                db.Database.ExecuteSqlCommand("DELETE FROM Bus");
                db.Database.ExecuteSqlCommand(@"INSERT INTO Bus(Number, Mark, ReleaseYear, Capacity)
                                                VALUES('у204то', 'ПАЗ-3205',	  2016, 42),
                   	                                  ('о548лд', 'ПАЗ-320412-05', 2012, 60),
                   	                                  ('з645ал', 'МАЗ-206',		  2009, 72),
                   	                                  ('ж784не', 'ПАЗ-4234',	  2013, 50),
                   	                                  ('и134да', 'ЛиАЗ-429260',	  2008, 75),
                   	                                  ('к683вы', 'ПАЗ-4234',	  2010, 50),
                   	                                  ('е432ыф', 'ПАЗ-32053',	  2006, 43),
                   	                                  ('п454тр', 'МАЗ-256',		  2007, 43),
                   	                                  ('о908па', 'ПАЗ-320412-05', 2016, 60),
                   	                                  ('п456ва', 'ПАЗ-3205',	  2005, 42),
                   	                                  ('ч823ор', 'ПАЗ-3205',	  2014, 42),
                   	                                  ('с859вд', 'ПАЗ-320412-05', 2011, 60),
                   	                                  ('т134уц', 'ПАЗ-3205',	  2012, 42),
                   	                                  ('ф083ув', 'ПАЗ-32053',	  2009, 43),
                   	                                  ('г752сн', 'ПАЗ-32054',	  2007, 43)");
            }

            UpdateData(sender, e);
        }

        #endregion
    }
}
