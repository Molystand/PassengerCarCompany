namespace PassengerCarCompany
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Configuration;
    using System.Data.SqlClient;
    using System.Linq;

    public partial class Driver : INotifyPropertyChanged, ICloneable
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Driver()
        {
            this.RouteSheet = new HashSet<RouteSheet>();
        }

        public Driver(int number, string surname, string name, string patronimic, string sex, DateTime birthdate, string phoneNumber, string address) : this()
        {
            Number = number;
            Surname = surname;
            Name = name;
            Patronymic = patronimic;
            Sex = sex;
            Birthdate = birthdate;
            PhoneNumber = phoneNumber;
            Address = address;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #region Свойства

        int number;
        string surname;
        string name;
        string patronimic;
        string sex;
        DateTime birthdate;
        string phoneNumber;
        string address;

        public int Number
        {
            get { return number; }
            set
            {
                number = value;
                OnPropertyChanged("Number");
            }
        }

        public string Surname
        {
            get { return surname; }
            set
            {
                surname = value;
                OnPropertyChanged("Surname");
            }
        }

        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }

        public string Patronymic
        {
            get { return patronimic; }
            set
            {
                patronimic = value;
                OnPropertyChanged("Patronymic");
            }
        }

        public string Sex
        {
            get { return sex; }
            set
            {
                sex = value;
                OnPropertyChanged("Sex");
            }
        }

        public DateTime Birthdate
        {
            get { return birthdate; }
            set
            {
                birthdate = value;
                OnPropertyChanged("Birthdate");
            }
        }

        public string PhoneNumber
        {
            get { return phoneNumber; }
            set
            {
                phoneNumber = value;
                OnPropertyChanged("PhoneNumber");
            }
        }

        public string Address
        {
            get { return address; }
            set
            {
                address = value;
                OnPropertyChanged("Address");
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
                bool entryFound = db.Driver.Find(this.Number) != null;

                if (!entryFound)
                {
                    db.Driver.Add(this);
                    db.SaveChanges();
                }
            }
        }

        public static void Insert(Driver insEntry)
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
                Driver entry = db.Driver.Find(this.Number);

                if (entry != null)
                {
                    try
                    {
                        // Удаляем из БД.
                        db.Driver.Remove(entry);
                        db.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        //MessageBox.Show(ex.InnerException.Message);
                    }
                }
            }
        }

        public static void Delete(Driver delEntry)
        {
            if (delEntry != null)
            {
                delEntry.Delete();
            }
        }

        #endregion

        #region Update

        public void Update(Driver newEntry)
        {
            if (newEntry != null)
            {
                using (var db = new PassengerCarCompanyEntities())
                {
                    db.Database.ExecuteSqlCommand(@"UPDATE Driver
                                                    SET Number = @number, Surname = @surname, Name = @name, Patronymic = @patronimic, Sex = @sex, Birthdate = @birthdate, PhoneNumber = @phoneNumber, Address = @address
                                                    WHERE Number = @oldNumber",
                                                    new SqlParameter("number", newEntry.Number),
                                                    new SqlParameter("surname", newEntry.Surname),
                                                    new SqlParameter("name", newEntry.Name),
                                                    new SqlParameter("patronimic", newEntry.Patronymic),
                                                    new SqlParameter("sex", newEntry.Sex),
                                                    new SqlParameter("birthdate", newEntry.Birthdate),
                                                    new SqlParameter("phoneNumber", newEntry.PhoneNumber),
                                                    new SqlParameter("address", newEntry.Address),
                                                    new SqlParameter("oldNumber", this.Number));
                }
            }

        }

        public static void Update(Driver updEntry, Driver newEntry)
        {
            if (updEntry != null)
            {
                updEntry.Update(newEntry);
            }
        }

        #endregion

        #region Get

        public static IEnumerable<Driver> Get()
        {
            using (var db = new PassengerCarCompanyEntities())
            {
                return (from d in db.Driver select d).ToArray();
            }
        }

        public static Driver Get(int number)
        {
            using (var db = new PassengerCarCompanyEntities())
            {
                return db.Driver.Find(number);
            }
        }

        #endregion

        #endregion
    }
}
