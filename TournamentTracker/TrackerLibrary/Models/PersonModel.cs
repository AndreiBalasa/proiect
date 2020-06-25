using System;
using System.Collections.Generic;
using System.Text;

namespace TrackerLibrary.Models
{
    public class PersonModel
    {

        public int Id { get; set; }

        public string FirstName { get; set; }

        public string Lastname { get; set; }

        public string EmailAdress { get; set; }

        public string CellphoneNumber { get; set; }

        public string FullName
        {
            get
            {
                return $"{ FirstName } { Lastname }";
            }
        }
    }
}
