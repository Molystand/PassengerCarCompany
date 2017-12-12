using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace PassengerCarCompany.Windows
{
    public partial class DriverWindow : Window
    {
        Driver selectedDriver;
        ObservableCollection<Driver> lstDrivers;

        #region Загрузка окна

        public DriverWindow()
        {
            InitializeComponent();

            // Контекст данных для блока доп. информации.
            selectedDriver = new Driver();
            gboxCurrDriver.DataContext = selectedDriver;

            // Выбрать первую строку таблицы.
            dgridDriver.SelectedIndex = 0;
        }

        #endregion

        #region Работа со списком данных

        // Заполнение списка.
        private void UpdateData(object sender, RoutedEventArgs e)
        {
            lstDrivers = new ObservableCollection<Driver>(Driver.Get());
            dgridDriver.ItemsSource = lstDrivers;
            lstDrivers.CollectionChanged += lstDrivers_CollectionChanged;
        }

        // Событие изменения списка данных.
        private void lstDrivers_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            // При удалении элемента.
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                (e.OldItems[0] as Driver).Delete();
            }
            // При добавлении элемента.
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                (e.NewItems[0] as Driver).Insert();
            }
            // При изменении элемента.
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Replace)
            {
                (e.OldItems[0] as Driver).Update(e.NewItems[0] as Driver);
            }
        }

        #endregion

        #region Подробная информация

        // При выборе строки вывести подробную информацию.
        private void dgridDriver_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count <= 0 || dgridDriver.SelectedItems.Count > 1 || dgridDriver.Items.IndexOf(e.AddedItems[0]) >= lstDrivers.Count)
                return;

            //selectedDriver = (Driver)((Driver)e.AddedItems[0]).Clone();
            selectedDriver.Number       = ((Driver)e.AddedItems[0]).Number;
            selectedDriver.Surname      = ((Driver)e.AddedItems[0]).Surname;
            selectedDriver.Name         = ((Driver)e.AddedItems[0]).Name;
            selectedDriver.Patronymic   = ((Driver)e.AddedItems[0]).Patronymic;
            selectedDriver.Sex          = ((Driver)e.AddedItems[0]).Sex;
            cboxSex.SelectedIndex = selectedDriver.Sex == "муж" ? 0 : 1;
            selectedDriver.Birthdate    = ((Driver)e.AddedItems[0]).Birthdate;
            selectedDriver.PhoneNumber  = ((Driver)e.AddedItems[0]).PhoneNumber;
            selectedDriver.Address      = ((Driver)e.AddedItems[0]).Address;
        }

        private void cboxSex_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedDriver.Sex = ((ComboBoxItem)((ComboBox)sender).SelectedItem).Content.ToString();
        }

        private void ValidationError(object sender, ValidationErrorEventArgs e)
        {
            int selected = dgridDriver.SelectedIndex;

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
            int selected = dgridDriver.SelectedIndex;

            if (selected != -1)
            {
                // Количество выбранных элементов.
                int count = dgridDriver.SelectedItems.Count;
                if (count > lstDrivers.Count)
                    count = lstDrivers.Count;

                for (int i = 0; i < count; i++)
                {
                    // Удаляем из теблицы.
                    lstDrivers.Remove((Driver)dgridDriver.SelectedItem);
                }
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (tbErrorInfo.Text != string.Empty)
                return;

            if (Driver.Get(selectedDriver.Number) != null)
            {
                MessageBox.Show("Водитель с таким номером уже есть в БД", "Ошибка");
                return;
            }

            lstDrivers.Add((Driver)selectedDriver.Clone());
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            int selected = dgridDriver.SelectedIndex;
            if (selected != -1)
            {
                lstDrivers[selected] = (Driver)selectedDriver.Clone();
            }
        }

        // Восстановить таблицу автобусов.
        private void btnRestoreTable_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new PassengerCarCompanyEntities())
            {
                db.Database.ExecuteSqlCommand("DELETE FROM Driver");
                db.Database.ExecuteSqlCommand(@"INSERT INTO Driver(Number, Surname, Name, Patronymic, Sex, Birthdate, PhoneNumber, Address)
			                                    VALUES(168, 'Бокарёв',	  'Святослав', 'Андреевич',	   'муж', '1970-04-30', '8(967)753-19-28', 'ул. Советская, дом 7, кв. 101'),
			                                    	  (176, 'Курганов',	  'Олег ',	   'Андреевич',	   'муж', '1972-04-06', '8(928)640-54-53', 'ул. Журавлёва, дом 65, кв. 52'),
			                                    	  (249, 'Пономарёв',  'Леонтий',   'Дмитриевич',   'муж', '1984-10-04', '8(938)592-92-24', 'ул. Дальняя, дом 41, кв. 43'),
			                                    	  (290, 'Грешнев',	  'Антон',     'Денисович',    'муж', '1989-11-08', '8(964)573-80-23', 'ул. Садовая, дом 96, кв. 13'),
			                                    	  (216, 'Назаров',	  'Фёдор',     'Семёнович',    'муж', '1976-08-23', '8(934)264-59-49', 'ул. Баррикадная, дом 95, кв. 54'),
			                                    	  (219, 'Иноземцев',  'Игорь ',	   'Матвеевич',	   'муж', '1970-04-19', '8(916)912-59-45', 'ул. Дачная, дом 47, кв. 16'),
			                                    	  (204, 'Маслов',	  'Николай',   'Геннадиевич',  'муж', '1990-09-17', '8(964)196-77-98', 'ул. Весенняя, дом 43, кв. 47'),
			                                    	  (125, 'Щукина',	  'Ирина',	   'Сергеевна',    'жен', '1983-10-01', '8(970)185-90-60', 'ул. Герцена, дом 68'),
			                                    	  (141, 'Маслов',	  'Виталий',   'Денисович',    'муж', '1981-05-15', '8(929)160-79-50', 'ул. Загородная, дом 46, кв. 105'),
			                                    	  (174, 'Филатов',	  'Аркадий',   'Глебович',	   'муж', '1993-12-19', '8(911)612-95-65', 'ул. Строителей, дом 92, кв. 14'),
			                                    	  (172, 'Винокурова', 'Кристина',  'Артуровна',    'жен', '1984-07-19', '8(928)812-82-42', 'ул. Молодёжная, дом 96, кв. 37'),
			                                    	  (193, 'Шульц',	  'Алина ',	   'Вячеславовна', 'жен', '1992-03-12', '8(951)312-42-43', 'ул. Весенняя, дом 68, кв. 72'),
			                                    	  (246, 'Жуков',	  'Семён',	   'Глебович',	   'муж', '1975-10-22', '8(909)191-90-49', 'ул. Герцена, дом 57'),
			                                    	  (169, 'Назарова',	  'Регина',	   'Андреевна',    'жен', '1979-08-12', '8(911)993-68-46', 'ул. Строителей, дом 94, кв. 104'),
			                                    	  (228, 'Шульц',	  'Кирилл',    'Игнатиевич',   'муж', '1973-08-25', '8(929)508-90-37', 'ул. Герцена, дом 20'),
			                                    	  (171, 'Митькин',	  'Аркадий',   'Сергеевич',    'муж', '1990-08-13', '8(937)989-97-97', 'ул. Желябова, дом 2, кв. 12'),
			                                    	  (212, 'Холодов',	  'Антон',	   'Денисович',    'муж', '1981-12-10', '8(955)370-37-19', 'ул. Баррикадная, дом 5, кв. 54'),
			                                    	  (159, 'Глушаков',	  'Дмитрий',   'Вадимович',	   'муж', '1987-06-30', '8(946)790-66-86', 'ул. Георгиевская, дом 17, кв. 21')");
            }

            UpdateData(sender, e);
        }

        #endregion
    }
}
