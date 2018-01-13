namespace PassengerCarCompany
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class PassengerCarCompanyEntities : DbContext
    {
        public PassengerCarCompanyEntities()
            : base("name=PassengerCarCompanyEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Bus> Bus { get; set; }
        public virtual DbSet<BusStop> BusStop { get; set; }
        public virtual DbSet<Driver> Driver { get; set; }
        public virtual DbSet<Route> Route { get; set; }
        public virtual DbSet<RouteSheet> RouteSheet { get; set; }
        public virtual DbSet<StopsOnTheRoute> StopsOnTheRoute { get; set; }
        public virtual DbSet<CountOfBusTrips> CountOfBusTrips { get; set; }
    }
}
