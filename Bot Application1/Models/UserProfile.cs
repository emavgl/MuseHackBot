using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bot_Application1.Models
{
    [Serializable]
    public class UserProfile
    {
        public enum GenderEnum { Male, Female};
        public string Name { get; set; }
        public int Age { get; set; }
        public GenderEnum Gender { get; set; }
        public string PhotoUrl { get; set; }
    }
}