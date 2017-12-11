namespace PassengerCarCompany
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Configuration;
    using System.Data.SqlClient;
    using System.Linq;

    public partial class Bus : INotifyPropertyChanged, ICloneable
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Bus() => RouteSheet = new HashSet<RouteSheet>();

        public Bus(string number, string mark, int releaseYear, int capacity) : this()
        {
            Number = number;
            Mark = mark;
            ReleaseYear = releaseYear;
            Capacity = capacity;
        }

        // Копирование объекта
        public object Clone() => this.MemberwiseClone();

        #region Операции

        #region Insert

        public void Insert()
        {
            // Возможная валидация.

            using (var db = new PassengerCarCompanyEntities())
            {
                // Есть ли такая запись в БД.
                bool busFound = db.Bus.Find(this.Number) != null;

                if (!busFound)
                {
                    db.Bus.Add(this);
                    db.SaveChanges();
                }
            }
        }

        public static void Insert(Bus insBus)
        {
            if (insBus != null)
            {
                insBus.Insert();
            }
        }

        #endregion

        #region Delete

        public void Delete()
        {
            using (var db = new PassengerCarCompanyEntities())
            {
                Bus bus = db.Bus.Find(this.Number);

                if (bus != null)
                {
                    try
                    {
                        // Удаляем из БД.
                        db.Bus.Remove(bus);
                        db.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        //MessageBox.Show(ex.InnerException.Message);
                    }
                }
            }
        }

        public static void Delete(Bus delBus)
        {
            if (delBus != null)
            {
                delBus.Delete();
            }
        }

        #endregion

        #region Update

        public void Update(Bus newBus)
        {
            // Возможная валидация.

            if (newBus != null)
            {
                // Установка соединения с БД.
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["Sql Server"].ConnectionString))
                {
                    conn.Open();

                    // Формирование запроса к БД.
                    SqlCommand cmd = new SqlCommand();

                    string cmdText;
                    cmd.Connection = conn;

                    cmdText = @"UPDATE Bus
                                SET Number = @number, Mark = @mark, ReleaseYear = @year, Capacity = @capacity
                                WHERE Number = @oldNumber";
                    cmd.CommandText = cmdText;
                    cmd.Parameters.AddWithValue("number", newBus.Number);
                    cmd.Parameters.AddWithValue("mark", newBus.Mark);
                    cmd.Parameters.AddWithValue("year", newBus.ReleaseYear);
                    cmd.Parameters.AddWithValue("capacity", newBus.Capacity);
                    cmd.Parameters.AddWithValue("oldNumber", this.Number);

                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (SqlException ex)
                    {
                        //MessageBox.Show(ex.Message);
                    }
                }
            }

        }

        public static void Update(Bus updBus, Bus newBus)
        {
            if (updBus != null)
            {
                updBus.Update(newBus);
            }
        }

        #endregion

        #region Get

        public static IEnumerable<Bus> Get()
        {
            using (var db = new PassengerCarCompanyEntities())
            {
                return (from bus in db.Bus select bus).ToArray();
            }
        }

        public static Bus Get(string number)
        {
            using (var db = new PassengerCarCompanyEntities())
            {
                return db.Bus.Find(number);
            }
        }

        #endregion

        #endregion

        #region Свойства

        string number;
        string mark;
        int releaseYear;
        int capacity;

        public string Number
        {
            get { return number; }
            set
            {
                number = value;
                OnPropertyChanged("Number");
            }
        }

        public string Mark
        {
            get { return mark; }
            set
            {
                mark = value;
                OnPropertyChanged("Mark");
            }
        }

        public int ReleaseYear
        {
            get { return releaseYear; }
            set
            {
                releaseYear = value;
                OnPropertyChanged("ReleaseYear");
            }
        }

        public int Capacity
        {
            get { return capacity; }
            set
            {
                capacity = value;
                OnPropertyChanged("Capacity");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RouteSheet> RouteSheet { get; set; }

        #endregion
    }
}
