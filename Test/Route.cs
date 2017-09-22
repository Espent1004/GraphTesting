using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuickGraph;

namespace Test
{
    class Route : IEdge<Airports>
    {
        //Regn ut ved hjelp av departure og arrival
        public double TravelTime { get; set; }//The weight to use in the graph

        public Airports Source { get; set; }

        public Airports Target { get; set; }

        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }

        public override string ToString() {

            return Source.ToString() + " -> " + Target.ToString();
        }
    }
}
