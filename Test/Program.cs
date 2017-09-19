using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuickGraph;
using System.Diagnostics;
using QuickGraph.Algorithms.ShortestPath;
using QuickGraph.Algorithms.Observers;
using QuickGraph.Algorithms;
using System.Data;
using QuickGraph.Algorithms.RankedShortestPath;
using QuickGraph.Graphviz;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
            Retrieve all airports from database and add them as vertices in the graph
            Retrieve all flights on the specified day and calculate traveltime based on departure and arrival
            Flight must reach the next airport before the next flight leaves
            Take into account waiting time at airport when calculating total time
            */
            //Airports
            var gardermoen = new Airport() { };
            gardermoen.Name = "Gardermoen";

            var gatwick = new Airport() { };
            gatwick.Name = "Gatwick";

            var barcelona = new Airport() { };
            barcelona.Name = "El Prat";

            var frankfurt = new Airport() { };
            frankfurt.Name = "Flughafen Frankfurt am Main";


            //Routes from OSL
            var route11 = new Route();
            route11.Source = gardermoen;
            route11.Target = gatwick;
            route11.TravelTime = 90;

            var route12 = new Route();
            route12.Source = gardermoen;
            route12.Target = frankfurt;
            route12.TravelTime = 90;

            var route13 = new Route();
            route13.Source = gardermoen;
            route13.Target = barcelona;
            route13.TravelTime = 200;

            //Routes from gatwick
            var route21 = new Route();
            route21.Source = gatwick;
            route21.Target = gardermoen;
            route21.TravelTime = 90;

            var route22 = new Route();
            route22.Source = gatwick;
            route22.Target = frankfurt;
            route22.TravelTime = 80;

            //Routes from frankfurt
            var route31 = new Route();
            route31.Source = frankfurt;
            route31.Target = barcelona;
            route31.TravelTime = 150;

            var route32 = new Route();
            route32.Source = frankfurt;
            route32.Target = gardermoen;
            route32.TravelTime = 90;

            var route33 = new Route();
            route33.Source = frankfurt;
            route33.Target = gatwick;
            route33.TravelTime = 80;

            //Routes from barcelona
            var route41 = new Route();
            route41.Source = barcelona;
            route41.Target = frankfurt;
            route41.TravelTime = 150;

            var route42 = new Route();
            route42.Source = barcelona;
            route42.Target = gardermoen;
            route42.TravelTime = 200;

            //Test

            var route99 = new Route();
            route99.Source = gardermoen;
            route99.Target = barcelona;
            route99.TravelTime = 50;

            //Graph where the edges go both ways
            var g = new BidirectionalGraph<Airport, Route>();
            //Adding airports and routes to graph
            g.AddVertex(gardermoen);
            g.AddVertex(gatwick);
            g.AddVertex(frankfurt);
            g.AddVertex(barcelona);
            g.AddEdge(route11);
            g.AddEdge(route12);
            g.AddEdge(route13);
            g.AddEdge(route21);
            g.AddEdge(route22);
            g.AddEdge(route31);
            g.AddEdge(route32);
            g.AddEdge(route33);
            g.AddEdge(route41);
            g.AddEdge(route42);
            g.AddEdge(route99);

            //Func that returns the traveltime of the route arg
            Func<Route, double> travelTime = t => t.TravelTime;

            //Source airport and target
            Airport sourceA = barcelona;
            Airport targetA = gardermoen;

            //Using HoffmanPavleyRankedShortestPath to find the 2 shortest paths
            HoffmanPavleyRankedShortestPathAlgorithm<Airport, Route> hoffmanAlgorithm = new HoffmanPavleyRankedShortestPathAlgorithm<Airport, Route>(g,travelTime);

            try
            {
                hoffmanAlgorithm.ShortestPathCount = 5;
                hoffmanAlgorithm.SetRootVertex(sourceA);
                hoffmanAlgorithm.Compute(sourceA, targetA);
                //Prints the paths to the console
                foreach (IEnumerable<Route> path in hoffmanAlgorithm.ComputedShortestPaths)
                {
                    foreach (var e in path) Debug.WriteLine(e);
                    Debug.WriteLine("------------------------------");
                }
            }
            catch(Exception e) {
                Debug.WriteLine(e);
            }

        }
    }
}
