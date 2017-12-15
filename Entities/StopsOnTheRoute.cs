namespace PassengerCarCompany
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Configuration;
    using System.Data.SqlClient;
    using System.Linq;

    public partial class StopsOnTheRoute : INotifyPropertyChanged, ICloneable
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public StopsOnTheRoute() { }

        public StopsOnTheRoute(int number, int routeNumber, int stopId)
        {
            Number      = number;
            RouteNumber = routeNumber;
            StopId      = stopId;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #region Свойства

        int number;
        int routeNumber;
        int stopId;

        public int Number
        {
            get { return number; }
            set
            {
                number = value;
                OnPropertyChanged("Number");
            }
        }

        public int RouteNumber
        {
            get { return routeNumber; }
            set
            {
                routeNumber = value;
                using (var db = new PassengerCarCompanyEntities())
                {
                    RouteTitle = db.Route.Find(routeNumber).Title;
                    OnPropertyChanged("RouteTitle");
                }
                OnPropertyChanged("RouteNumber");
            }
        }

        public int StopId
        {
            get { return stopId; }
            set
            {
                stopId = value;
                using (var db = new PassengerCarCompanyEntities())
                {
                    StopTitle = db.BusStop.Find(stopId).Title;
                    OnPropertyChanged("StopTitle");
                }
                OnPropertyChanged("StopId");
            }
        }


        public string RouteTitle { get; set; }

        public string StopTitle { get; set; }



        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public virtual BusStop BusStop { get; set; }

        public virtual Route Route { get; set; }

        #endregion

        #region Операции

        #region Insert

        public void Insert()
        {
            using (var db = new PassengerCarCompanyEntities())
            {
                // Есть ли такая запись в БД.
                bool entryFound = db.StopsOnTheRoute.Find(this.Number, this.RouteNumber) != null;

                if (!entryFound)
                {
                    db.StopsOnTheRoute.Add(this);
                    db.SaveChanges();
                }
            }
        }

        public static void Insert(StopsOnTheRoute insEntry)
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
                StopsOnTheRoute entry = db.StopsOnTheRoute.Find(this.Number, this.RouteNumber);

                if (entry != null)
                {
                    try
                    {
                        // Удаляем из БД.
                        db.StopsOnTheRoute.Remove(entry);
                        db.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        //MessageBox.Show(ex.InnerException.Message);
                    }
                }
            }
        }

        public static void Delete(StopsOnTheRoute delEntry)
        {
            if (delEntry != null)
            {
                delEntry.Delete();
            }
        }

        #endregion

        #region Update

        public void Update(StopsOnTheRoute newEntry)
        {
            if (newEntry != null)
            {
                using (var db = new PassengerCarCompanyEntities())
                {
                    db.Database.ExecuteSqlCommand(@"UPDATE StopsOnTheRoute
                                                    SET Number = @number, RouteNumber = @routeNumber, StopId = @stopId
                                                    WHERE Number = @oldNumber",
                                                    new SqlParameter("number",      newEntry.Number),
                                                    new SqlParameter("routeNumber", newEntry.RouteNumber),
                                                    new SqlParameter("stopId",      newEntry.StopId),
                                                    new SqlParameter("oldNumber",   this.Number));
                }
            }

        }

        public static void Update(StopsOnTheRoute updEntry, StopsOnTheRoute newEntry)
        {
            if (updEntry != null)
            {
                updEntry.Update(newEntry);
            }
        }

        #endregion

        #region Get

        public static IEnumerable<StopsOnTheRoute> Get()
        {
            using (var db = new PassengerCarCompanyEntities())
            {
                return (from stop in db.StopsOnTheRoute
                        orderby stop.RouteNumber, stop.Number
                        select stop).ToArray();
            }
        }

        public static StopsOnTheRoute Get(int number, int routeNumber)
        {
            using (var db = new PassengerCarCompanyEntities())
            {
                return db.StopsOnTheRoute.Find(number, routeNumber);
            }
        }

        #endregion

        #endregion        
    }
}
