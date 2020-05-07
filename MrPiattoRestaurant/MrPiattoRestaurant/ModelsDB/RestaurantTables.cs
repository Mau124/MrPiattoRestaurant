using System;
using System.Collections.Generic;

namespace MrPiattoRestaurant.ModelsDB
{
    public partial class RestaurantTables
    {
        public RestaurantTables()
        {
            //LockedTables = new HashSet<LockedTables>();
            //Reservation = new HashSet<Reservation>();
            //TableDistribution = new HashSet<TableDistribution>();
        }

        public RestaurantTables(int Idtables, int CoordenateX, int CoordenateY, int Seats, string tableName)
        {
            this.Idtables = Idtables;
            this.CoordenateX = CoordenateX;
            this.CoordenateY = CoordenateY;
            this.Seats = Seats;
            this.tableName = tableName;
        }

        public int Idtables { get; set; }
        public int Idrestaurant { get; set; }
        public string FloorName { get; set; }
        public double CoordenateX { get; set; }
        public double CoordenateY { get; set; }
        public double AvarageUse { get; set; }
        public int Seats { get; set; }
        public string Type { get; set; }
        public int floorIndex { get; set; }
        public string tableName { get; set; }
        public bool? IsJoin { get; set; }

        public virtual ICollection<Reservation> Reservation { get; set; }

        //public virtual Restaurant IdrestaurantNavigation { get; set; }
        //public virtual ICollection<LockedTables> LockedTables { get; set; }
        //public virtual ICollection<TableDistribution> TableDistribution { get; set; }
    }
}
