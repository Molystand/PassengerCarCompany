namespace PassengerCarCompany
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Configuration;
    using System.Data.SqlClient;
    using System.Linq;

    public partial class Route
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Route()
        {
            this.RouteSheet = new HashSet<RouteSheet>();
            this.StopsOnTheRoute = new HashSet<StopsOnTheRoute>();
        }

        public Route(int number, string title, int len, TimeSpan averTravelTime) : this()
        {
            Number = number;
            Title = title;
            RouteLength = len;
            AverTravelTime = averTravelTime;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #region Свойства

        int number;
        string title;
        int len;
        TimeSpan averTravelTime;

        public int Number
        {
            get { return number; }
            set
            {
                number = value;
                OnPropertyChanged("Number");
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

        public int RouteLength
        {
            get { return len; }
            set
            {
                len = value;
                OnPropertyChanged("RouteLength");
            }
        }

        public TimeSpan AverTravelTime
        {
            get { return averTravelTime; }
            set
            {
                averTravelTime = value;
                OnPropertyChanged("AverTravelTime");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RouteSheet> RouteSheet { get; set; }
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
                bool entryFound = db.Route.Find(this.Number) != null;

                if (!entryFound)
                {
                    db.Route.Add(this);
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
                Route entry = db.Route.Find(this.Number);

                if (entry != null)
                {
                    try
                    {
                        // Удаляем из БД.
                        db.Route.Remove(entry);
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

        public void Update(Route newEntry)
        {
            if (newEntry != null)
            {
                using (var db = new PassengerCarCompanyEntities())
                {
                    db.Database.ExecuteSqlCommand(@"UPDATE Route
                                                    SET Number = @number, Title = @title, RouteLength = @len, AverTravelTime = @averTravelTime
                                                    WHERE Number = @oldId",
                                                    new SqlParameter("number",         newEntry.Number),
                                                    new SqlParameter("title",          newEntry.Title),
                                                    new SqlParameter("len",            newEntry.RouteLength),
                                                    new SqlParameter("averTravelTime", newEntry.AverTravelTime),
                                                    new SqlParameter("oldNumber",      this.Number));
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

        public static IEnumerable<Route> Get()
        {
            using (var db = new PassengerCarCompanyEntities())
            {
                return (from Route in db.Route select Route).ToArray();
            }
        }

        public static Route Get(int number)
        {
            using (var db = new PassengerCarCompanyEntities())
            {
                return db.Route.Find(number);
            }
        }

        public static Route Get(string title)
        {
            using (var db = new PassengerCarCompanyEntities())
            {
                return (from bs in db.Route
                        where bs.Title == title
                        select bs)
                        .FirstOrDefault();
            }
        }

        #endregion

        #endregion
    }
}
