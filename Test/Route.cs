using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuickGraph;

namespace Test
{
    class Route : IEdge<Airport>
    {
        //Regn ut ved hjelp av departure og arrival
        public double TravelTime { get; set; }//The weight to use in the graph

        public Airport Source { get; set; }

        public Airport Target { get; set; }

        public override string ToString() {

            return Source.ToString() + " -> " + Target.ToString();
        }
    }
}
