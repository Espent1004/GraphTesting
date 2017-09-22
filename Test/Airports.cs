using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    class Airports
    {
        //Endre til ID
        public String AirportID { get; set; }
        public String AirportName { get; set; }


        public override string ToString()
        {

            return AirportName;
        }
    }
}
