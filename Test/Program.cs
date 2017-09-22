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
using System.Collections;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            //Dictionary to hold all the airports, using dictionary so we can retrieve the value with the string id in flight
            Dictionary<String, Airports> airports = new Dictionary<String, Airports>();
            //List of all the flights
            List<Route> flights = new List<Route>();
            //Data for airports
            String s =
               "OSL,Oslo Gardermoen;BCN, Barcelona El Prat;LGW,London Gatwick;CDG,Paris Charles De Gaulle;ARN,Stockholm Arland;CPH,Copenhagen Kastrup;AGP,Malaga Costa del Sol Airport;AMS,Amsterdam Airport Schipol;FRA,Frankfurt Airport;JFK,New York John F. Kennedy International Airport;LAX,Los Angeles International Airport;FCO,Roma Leonardo da Vinci - Fiumicino Airport;TRD,Trondheim Værnes;BGO,Bergen Flesland;TXL,Berlin Tegel Airport;BKK,Suvarnabhumi Airport;JNB,Johanesburg O. R. Tambo International Airport;BNE,Brisbane Airport;GIG,Rio de Janeiro - Antonio Carlos Jobim International Airport;KEF,Reykjavik Keflavík International Airport";

            //Splitting the string and creating airports objects, and putting them into the list
            String[] data = s.Split(';');
            foreach (string a in data)
            {
                var airport = new Airports();
                String[] b = a.Split(',');
                airport.AirportID = b[0];
                airport.AirportName = b[1];
                airports.Add(b[0], airport);


            }

            //Flight data

            String x = "30.09.2017 09:00:00,30.09.2017 10:40:00,499,OSL,LGW,1;30.09.2017 09:30:00,30.09.2017 12:30:00,599,OSL,BCN,2;30.09.2017 10:00:00,30.09.2017 12:00:00,299,OSL,FRA,3;30.09.2017 07:00:00,30.09.2017 08:00:00,199,OSL,TRD,4;30.09.2017 08:00:00,30.09.2017 09:00:00,199,OSL,BGO,5;30.09.2017 12:00:00,30.09.2017 15:00:00,549,OSL,CDG,6;30.09.2017 10:00:00,30.09.2017 11:00:00,199,BGO,OSL,5;30.09.2017 09:00:00,30.09.2017 10:00:00,149,BGO,TRD,7;30.09.2017 09:00:00,30.09.2017 10:00:00,199,TRD,OSL,4;30.09.2017 11:00:00,30.09.2017 12:00:00,149,TRD,BGO,7;30.09.2017 12:00:00,30.09.2017 13:40:00,499,LGW,OSL,1;30.09.2017 11:00:00,30.09.2017 13:00:00,299,LGW,FRA,8;30.09.2017 13:00:00,30.09.2017 16:00:00,449,LGW,BCN,9;30.09.2017 14:30:00,30.09.2017 17:30:00,599,BCN,OSL,2;30.09.2017 17:00:00,30.09.2017 20:00:00,449,BCN,LGW,9;30.09.2017 19:00:00,30.09.2017 21:00:00,349,BCN,FRA,10;30.09.2017 13:30:00,30.09.2017 15:30:00,299,FRA,OSL,3;30.09.2017 14:30:00,30.09.2017 16:30:00,299,FRA,LGW,8;30.09.2017 23:00:00,01.10.2017 01:00:00,349,FRA,BCN,10;30.09.2017 16:00:00,30.09.2017 19:00:00,549,CDG,OSL,6";

            //Splitting the string and creating flight objects and adding them to the list
            String[] arrayOfFlights = x.Split(';');

            foreach (String a in arrayOfFlights)
            {
                String[] b = a.Split(',');
                var f = new Route();
                var source = new Airports();
                var target = new Airports();
                String[] departureDateTime = b[0].Split(' ');
                String[] departureDate = departureDateTime[0].Split('.');
                String[] departureTime = departureDateTime[1].Split(':');

                String[] arrivalDateTime = b[1].Split(' ');
                String[] arrivalDate = arrivalDateTime[0].Split('.');
                String[] arrivalTime = arrivalDateTime[1].Split(':');

                f.DepartureTime = new DateTime(int.Parse(departureDate[2]), int.Parse(departureDate[1]), int.Parse(departureDate[0]), int.Parse(departureTime[0]), int.Parse(departureTime[1]), int.Parse(departureTime[2]));
                f.ArrivalTime = new DateTime(int.Parse(arrivalDate[2]), int.Parse(arrivalDate[1]), int.Parse(arrivalDate[0]), int.Parse(arrivalTime[0]), int.Parse(arrivalTime[1]), int.Parse(arrivalTime[2]));
                airports.TryGetValue(b[3], out source);
                f.Source = source;
                airports.TryGetValue(b[4], out target);
                f.Target = target;
                flights.Add(f);//Adding to list
            }


            //Graph where the edges go both ways
            var g = new BidirectionalGraph<Airports, Route>();
            //Adding airports and routes to graph
            foreach(var airport in airports)
            {
                g.AddVertex(airport.Value);
            }

            foreach (Route r in flights) {
                g.AddEdge(r);
            }


            //Func that returns the traveltime of the route arg
            Func<Route, double> travelTime = t => (t.ArrivalTime - t.DepartureTime).TotalMinutes;

            //Source airport and target
            Airports sourceA;
            airports.TryGetValue("OSL", out sourceA);
            Airports targetA;
            airports.TryGetValue("BCN", out targetA);
            DateTime start = DateTime.Now;

            //Using HoffmanPavleyRankedShortestPath to find the 2 shortest paths
            HoffmanPavleyRankedShortestPathAlgorithm<Airports, Route> hoffmanAlgorithm = new HoffmanPavleyRankedShortestPathAlgorithm<Airports, Route>(g,travelTime);
            //List to hold all the possible routes
            List<IEnumerable<Route>> listOfAvailableFlights = new List<IEnumerable<Route>>();
            try
            {
                //Getting the 5 shortest paths, does not take into account waiting time at the airports
                hoffmanAlgorithm.ShortestPathCount = 10;
                hoffmanAlgorithm.SetRootVertex(sourceA);
                hoffmanAlgorithm.Compute(sourceA, targetA);
                //Adding the path to the list if it is viable
                foreach (IEnumerable<Route> path in hoffmanAlgorithm.ComputedShortestPaths)
                {
                    Boolean viable = true;//Helper variable
                    Route p = null;//Helper variable to hold the previous flight
                    foreach (var e in path) {
                        if(p != null)
                        {

                            if (e.DepartureTime < p.ArrivalTime.AddMinutes(30)) viable = false; //Not adding to list if a flight leaves before the previous one lands + 30 minutes

                        }
                        p = e;//Setting the helper variable
                    }
                    if(viable)listOfAvailableFlights.Add(path);//Adding to list

                }
            }
            catch(Exception e) {
                Debug.WriteLine(e);
            }

            Debug.WriteLine((DateTime.Now - start).TotalMilliseconds);

            //Traversing the list
            foreach (var path in listOfAvailableFlights) {
                //Retrieving the first and last route in the path
                Route last = new Route();
                last = path.LastOrDefault();
                Route first = new Route();
                first = path.FirstOrDefault();
                double timespan = 0;

                //Calculating the total travel time, including downtime at the airports
                if (!last.Equals(first)) timespan = (last.ArrivalTime - first.DepartureTime).TotalHours;
                else timespan = (first.ArrivalTime - first.DepartureTime).TotalHours;
                //Printing the path
                foreach (var e in path) {
                    Debug.WriteLine(e);
                }
                //Printing the total waiting time   
                Debug.WriteLine(timespan);
                Debug.WriteLine("--------------------------------");
            }

        }
    }
}
