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
        public Bus()
        {
            RouteSheet = new HashSet<RouteSheet>();
        }

        public Bus(string number, string mark, int releaseYear, int capacity) : this()
        {
            Number = number;
            Mark = mark;
            ReleaseYear = releaseYear;
            Capacity = capacity;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

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

        #region Операции

        #region Insert

        public void Insert()
        {
            using (var db = new PassengerCarCompanyEntities())
            {
                // Есть ли такая запись в БД.
                bool entryFound = db.Bus.Find(this.Number) != null;

                if (!entryFound)
                {
                    db.Bus.Add(this);
                    db.SaveChanges();
                }
            }
        }

        public static void Insert(Bus insEntry)
        {
            if (insEntry != null)
            {
                insEntry.Insert();
            }
        }

        #endregion

        #region Delete

        public void Delete()
        {
            using (var db = new PassengerCarCompanyEntities())
            {
                Bus entry = db.Bus.Find(this.Number);

                if (entry != null)
                {
                    try
                    {
                        // Удаляем из БД.
                        db.Bus.Remove(entry);
                        db.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        //MessageBox.Show(ex.InnerException.Message);
                    }
                }
            }
        }

        public static void Delete(Bus delEntry)
        {
            if (delEntry != null)
            {
                delEntry.Delete();
            }
        }

        #endregion

        #region Update

        public void Update(Bus newEntry)
        {
            if (newEntry != null)
            {
                using (var db = new PassengerCarCompanyEntities())
                {
                    db.Database.ExecuteSqlCommand(@"UPDATE Bus
                                                    SET Number = @number, Mark = @mark, ReleaseYear = @year, Capacity = @capacity
                                                    WHERE Number = @oldNumber",
                                                    new SqlParameter("number", newEntry.Number),
                                                    new SqlParameter("mark", newEntry.Mark),
                                                    new SqlParameter("year", newEntry.ReleaseYear),
                                                    new SqlParameter("capacity", newEntry.Capacity),
                                                    new SqlParameter("oldNumber", this.Number));
                }
            }

        }

        public static void Update(Bus updEntry, Bus newEntry)
        {
            if (updEntry != null)
            {
                updEntry.Update(newEntry);
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
    }
}
