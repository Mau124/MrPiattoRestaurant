using System;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Collections.Generic;

namespace MrPiattoRestaurant.ModelsDB
{
    public partial class WaiterStatistics
    {
        public int IdwaiterStatistics { get; set; }
        public int Idwaiter { get; set; }
        public float Rating { get; set; }
        public DateTime DateStatistics { get; set; }
        public virtual Waiters IdwaitersNavigation { get; set; }
    }
}