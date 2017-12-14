namespace PassengerCarCompany
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Configuration;
    using System.Data.SqlClient;
    using System.Linq;

    public partial class BusStop : INotifyPropertyChanged, ICloneable
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public BusStop()
        {
            this.StopsOnTheRoute = new HashSet<StopsOnTheRoute>();
        }

        public BusStop(int id, string title) : this()
        {
            Id = id;
            Title = title;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #region Свойства

        int id;
        string title;

        public int Id
        {
            get { return id; }
            set
            {
                id = value;
                OnPropertyChanged("Id");
            }
        }

        public string Title
        {
            get { return title; }
            set
            {
                title = value;
                OnPropertyChanged("Title");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StopsOnTheRoute> StopsOnTheRoute { get; set; }

        #endregion

        #region Операции

        #region Insert

        public void Insert()
        {
            using (var db = new PassengerCarCompanyEntities())
            {
                // Есть ли такая запись в БД.
                bool entryFound = (from bs in db.BusStop
                                   where bs.Title == this.Title
                                   select bs)
                                   .FirstOrDefault() != null;

                if (!entryFound)
                {
                    db.BusStop.Add(this);
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
                BusStop entry = db.BusStop.Find(this.Id);

                if (entry != null)
                {
                    try
                    {
                        // Удаляем из БД.
                        db.BusStop.Remove(entry);
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

        public void Update(BusStop newEntry)
        {
            if (newEntry != null)
            {
                using (var db = new PassengerCarCompanyEntities())
                {
                    db.Database.ExecuteSqlCommand(@"UPDATE BusStop
                                                    SET Title = @title
                                                    WHERE Id = @oldId",
                                                    new SqlParameter("title", newEntry.Title),
                                                    new SqlParameter("oldId", this.Id));
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

        public static IEnumerable<BusStop> Get()
        {
            using (var db = new PassengerCarCompanyEntities())
            {
                return (from bs in db.BusStop select bs).ToArray();
            }
        }

        public static BusStop Get(int id)
        {
            using (var db = new PassengerCarCompanyEntities())
            {
                return db.BusStop.Find(id);
            }
        }

        public static BusStop Get(string title)
        {
            using (var db = new PassengerCarCompanyEntities())
            {
                return (from bs in db.BusStop
                        where bs.Title == title
                        select bs)
                        .FirstOrDefault();
            }
        }

        #endregion

        #endregion
    }
}
