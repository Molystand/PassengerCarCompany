using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data.Entity.Core;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
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

namespace PassengerCarCompany
{
    /// <summary>
    /// Логика взаимодействия для BusWindow.xaml
    /// </summary>
    public partial class BusWindow : Window
    {
        ObservableCollection<Bus> lstBuses;

        #region Загрузка окна

        public BusWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

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
                //Bus.Delete(e.OldItems[0] as Bus);
            }
            // При добавлении элемента.
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                (e.NewItems[0] as Bus).Insert();
                //Bus.Insert(e.NewItems[0] as Bus);
            }
            // При изменении элемента.
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Replace)
            {
                (e.OldItems[0] as Bus).Update(e.NewItems[0] as Bus);
                //Bus.Update(e.OldItems[0] as Bus, e.NewItems[0] as Bus);
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
            lstBuses.Add(new Bus()
            {
                Number = "н293ом",
                Mark = "Марка",
                ReleaseYear = 2010,
                Capacity = 42
            });
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            int selected = dgridBus.SelectedIndex;
            if (selected != -1)
            {
                lstBuses[selected] = new Bus
                {
                    Number = "н293ом",
                    Mark = "Марка",
                    ReleaseYear = 2010,
                    Capacity = 42
                };
            }
        }

        // Восстановить таблицу автобусов.
        private void btnRestoreTable_Click(object sender, RoutedEventArgs e)
        {
            // Установка соединения с БД.
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Sql Server"].ConnectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand();

                // Удалить все записи из таблицы.
                string cmdText = @"DELETE FROM Bus";
                cmd.CommandText = cmdText;
                cmd.Connection = conn;
                cmd.ExecuteNonQuery();

                // Заполнение таблицы заново.
                cmdText = @"INSERT INTO Bus(Number, Mark, ReleaseYear, Capacity)
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
			                	  ('г752сн', 'ПАЗ-32054',	  2007, 43)";
                cmd.CommandText = cmdText;
                cmd.ExecuteNonQuery();
            }

            UpdateData(sender, e);
        }

        #endregion
    }
}
