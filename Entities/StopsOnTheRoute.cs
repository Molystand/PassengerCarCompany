namespace PassengerCarCompany
{
    using System;
    using System.Collections.Generic;
    
    public partial class StopsOnTheRoute
    {
        public int Number { get; set; }
        public int RouteNumber { get; set; }
        public int StopId { get; set; }
    
        public virtual BusStop BusStop { get; set; }
        public virtual Route Route { get; set; }
    }
}
