namespace PassengerCarCompany
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Configuration;
    using System.Data.SqlClient;
    using System.Linq;

    public partial class RouteSheet
    {
        public RouteSheet(int id, int number, DateTime date, TimeSpan departureTime, TimeSpan arrivalTime, int? plannedProfit, int realProfit, int driverNumber, int routeNumber, string busNumber)
        {
            Id            = id;
            Number        = number;
            Date          = date;
            DepartureTime = departureTime;
            ArrivalTime   = arrivalTime;
            PlannedProfit = plannedProfit;
            RealProfit    = realProfit;
            DriverNumber  = driverNumber;
            RouteNumber   = routeNumber;
            BusNumber     = busNumber;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #region Свойства

        int id;
        int number;
        DateTime date;
        TimeSpan departureTime;
        TimeSpan arrivalTime;
        int? plannedProfit;
        int realProfit;
        int driverNumber;
        int routeNumber;
        string busNumber;

        public int Id
        {
            get { return id; }
            set
            {
                id = value;
                OnPropertyChanged("Id");
            }
        }

        public int Number
        {
            get { return number; }
            set
            {
                number = value;
                OnPropertyChanged("Number");
            }
        }

        public DateTime Date
        {
            get { return date; }
            set
            {
                date = value;
                OnPropertyChanged("Date");
            }
        }

        public TimeSpan DepartureTime
        {
            get { return departureTime; }
            set
            {
                departureTime = value;
                OnPropertyChanged("DepartureTime");
            }
        }

        public TimeSpan ArrivalTime
        {
            get { return arrivalTime; }
            set
            {
                arrivalTime = value;
                OnPropertyChanged("ArrivalTime");
            }
        }

        public int? PlannedProfit
        {
            get { return id; }
            set
            {
                plannedProfit = value;
                OnPropertyChanged("PlannedProfit");
            }
        }

        public int RealProfit
        {
            get { return realProfit; }
            set
            {
                realProfit = value;
                OnPropertyChanged("RealProfit");
            }
        }

        public int DriverNumber
        {
            get { return driverNumber; }
            set
            {
                driverNumber = value;
                OnPropertyChanged("DriverNumber");
            }
        }

        public int RouteNumber
        {
            get { return routeNumber; }
            set
            {
                routeNumber = value;
                OnPropertyChanged("RouteNumber");
            }
        }

        public string BusNumber
        {
            get { return busNumber; }
            set
            {
                busNumber = value;
                OnPropertyChanged("BusNumber");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public virtual Bus Bus { get; set; }

        public virtual Driver Driver { get; set; }

        public virtual Route Route { get; set; }

        #endregion

        #region Операции

        #region Insert

        public void Insert()
        {
            using (var db = new PassengerCarCompanyEntities())
            {
                // Есть ли такая запись в БД.
                bool entryFound = db.RouteSheet.Find(this.Id) != null;

                if (!entryFound)
                {
                    db.RouteSheet.Add(this);
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
                RouteSheet entry = db.RouteSheet.Find(this.Id);

                if (entry != null)
                {
                    try
                    {
                        // Удаляем из БД.
                        db.RouteSheet.Remove(entry);
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

        public void Update(RouteSheet newEntry)
        {
            if (newEntry != null)
            {
                using (var db = new PassengerCarCompanyEntities())
                {
                    db.Database.ExecuteSqlCommand(@"UPDATE RouteSheet
                                                    SET Id = @id, Number = @number, Date = @date, DepartureTime = @departureTime, ArrivalTime = @arrivalTime,
                                                        PlannedProfit = @plannedProfit, RealProfit = @realProfit,
                                                        DriverNumber = @drivernumber, RouteNumber = @routeNumber, BusNumber = @busNumber
                                                    WHERE Id = @oldId",
                                                    new SqlParameter("id",            newEntry.Id),
                                                    new SqlParameter("number",        newEntry.Number),
                                                    new SqlParameter("date",          newEntry.Date),
                                                    new SqlParameter("departureTime", newEntry.DepartureTime),
                                                    new SqlParameter("arrivalTime",   newEntry.ArrivalTime),
                                                    new SqlParameter("plannedProfit", newEntry.PlannedProfit),
                                                    new SqlParameter("realProfit",    newEntry.RealProfit),
                                                    new SqlParameter("drivernumber",  newEntry.DriverNumber),
                                                    new SqlParameter("routeNumber",   newEntry.RouteNumber),
                                                    new SqlParameter("busNumber",     newEntry.BusNumber),
                                                    new SqlParameter("oldId",         this.Id));
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

        public static IEnumerable<RouteSheet> Get()
        {
            using (var db = new PassengerCarCompanyEntities())
            {
                return (from rs in db.RouteSheet select rs).ToArray();
            }
        }

        public static RouteSheet Get(int id)
        {
            using (var db = new PassengerCarCompanyEntities())
            {
                return db.RouteSheet.Find(id);
            }
        }

        #endregion

        #endregion
    }
}
