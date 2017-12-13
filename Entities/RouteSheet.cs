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
        public int Id { get; set; }
        public int Number { get; set; }
        public System.DateTime Date { get; set; }
        public System.TimeSpan DepartureTime { get; set; }
        public System.TimeSpan ArrivalTime { get; set; }
        public int? PlannedProfit { get; set; }
        public int RealProfit { get; set; }
        public int DriverNumber { get; set; }
        public int RouteNumber { get; set; }
        public string BusNumber { get; set; }
    
        public virtual Bus Bus { get; set; }
        public virtual Driver Driver { get; set; }
        public virtual Route Route { get; set; }
    }
}
