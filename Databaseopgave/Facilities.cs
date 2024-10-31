using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Databaseopgave
{
    public class Facilities
    {
        public int Facility_id { get; set; }
        public string Facility_Name { get; set; }

     
        public override string ToString() {
            return $"ID: {Facility_id}, Name: {Facility_Name}";
        }
    }
}
