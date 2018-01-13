namespace PassengerCarCompany
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;

    public partial class CountOfBusTrips
    {
        public string BusNumber { get; set; }

        public string Mark { get; set; }

        public Nullable<int> TripsCount { get; set; }

        public static IEnumerable<CountOfBusTrips> Get()
        {
            using (var db = new PassengerCarCompanyEntities())
            {
                return db.Database.SqlQuery<CountOfBusTrips>("SELECT * FROM CountOfBusTrips").ToList();
            }
        }
    }
}
