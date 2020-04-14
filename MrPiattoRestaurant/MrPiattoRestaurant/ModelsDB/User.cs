using System;
using System.Collections.Generic;

namespace MrPiattoRestaurant.ModelsDB
{
    public partial class User
    {
        public int Iduser { get; set; }
        public string Mail { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Gender { get; set; }
        public string UserType { get; set; }
        public DateTime? UnlockedDay { get; set; }
    }
}
