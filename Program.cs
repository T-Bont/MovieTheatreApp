/*
 * Title: Movie Theater Application
 * Description: Allows users to select what movie they want to watch
 * Author: Tyler Bontrager
 * Class: MIS 411
 */


using System.Diagnostics;

namespace MovieTheatreApp
{
    // Put Struct here with movie titles, price, rating, screen and showtimes
    // Create ratings using an enum list

    public enum Rating
    {
        G,
        PG,
        PG13,
        R,
        N17
    }
    public class Movies
    {
        public string title;
        public double price;
        public Rating rating;
        public Screens screen;
        public new List<string> times;
        public Movies(string t, double p, Rating r, Screens s, List<string> st)
        {
            title = t; price = p; rating = r; screen = s; times = st;
        }
        public Movies() { }

    }


    public class Order
    {
        public int movieIndex;
        public string title;
        public double price;
        public string showtime;
        public int screen;
        public int numTickets;
        
        public Order(int mI, string t, double p, string st, int s, int nt)
        {
            movieIndex = mI; title = t; price = p; showtime = st; screen = s; numTickets = nt; 
        }
    }

    public class Screens
    {
        public int screen;
        public int capacity;

        public Screens(int s, int c)
        {
            screen = s; capacity = c;
        }
        public Screens() { }   
        
    }



    internal class Program
    {
        static void Main(string[] args)
        {
            const int capacity = 100; //Creating the seating capacity variable
            int input = -1; //initializing the input value
            int index = -1;
            int i = 0;
            string confInput;
            bool Valid = false; //creating a bool value for the loops
            bool Valid2 = false; //creating a second one for the loop that covers almost the whole thing
            int numMovies = 0;
            List<Movies> movies = new List<Movies>(); //Creating a Movies list to store the selected movie
            List<Order> orders = new List<Order>(); //Creating the order list to store the users orders
            double grandTotal = 0;
            string path = "movie.txt";
            string firstName;
            string lastName;   
            int age = -1;
            DataValidation dv = new DataValidation();
            FileReading fr = new FileReading();


            Console.WriteLine("Hello and welcome to CMA Theaters!");
            Console.WriteLine("What is your first name?");
            //firstName = Console.ReadLine();
            Console.WriteLine("What is your last name?");
            //lastName = Console.ReadLine();

            //while (!Valid)
            //{
            //    age = dv.DataTypeValidation("What is your age?");
            //    Valid = dv.DataValueValidation(age, 0, 110);
            //}

            fr.FillMovies(movies, path);

            Console.WriteLine("Below is the list of movies now showing:");
            //start a while loop here to cover the entire thing so you can order multiple movies
            while (!Valid2)
            {
                Valid = false;
                //Start of the loop for movie selection
                while (!Valid)
                {
                    Console.WriteLine("\nPlease select the number of the movie you would like to view.");
                    foreach (Movies m in movies)
                    {
                        Console.WriteLine("{0}. {1}", movies.IndexOf(m) + 1, m.title); //Looping through the titles list
                    }
                    index = dv.DataTypeValidation("") - 1;
                    Valid = dv.DataValueValidation(index, 0, 6);
                    if (Valid)
                    {
                        Valid = dv.Confirmation($"is {movies[index].title} your desired choice?");
                    }
                } //End of movie selection loop
                
                Valid = false;
                int timeIndex = -1; //initializing variable
                //Start of time selection loop
                while (!Valid)
                {
                    Console.Clear();
                    Console.WriteLine("{0} is rated {1} and showing on screen {2} at times 1. {3}, 2. {4}, 3. {5}", movies[index].title, movies[index].rating,
                        movies[index].screen.screen, movies[index].times[0], movies[index].times[1], movies[index].times[2]);
                    Console.WriteLine("\nPlease select the number of the time you would like to view (1, 2, or 3):");
                    timeIndex = dv.DataTypeValidation("") - 1;
                    Valid = dv.DataValueValidation(timeIndex, 1, 3);
                    if (Valid)
                    {
                       Valid = dv.Confirmation($"Is {movies[index].times[timeIndex]} your desired time?");
                    }
                       
                }//End of time selection loop

                Valid = false;
                int numTickets = -1;
                //Start of ticket selection loop
                Console.Clear();
                do
                {
                    Console.WriteLine("You have selected to see {0} at {1}.",
                    movies[index].title, movies[index].times[timeIndex]);
                    Console.WriteLine("\nHow many tickets would you like to purchase? They are ${0} each.", movies[index].price);
                    numTickets = dv.DataTypeValidation("");
                    Valid = dv.DataValueValidation(numTickets, 0, movies[index].screen.capacity);
                } while (!Valid);
                //end of ticket selection loop

                double priceTickets = movies[index].price * numTickets;
                grandTotal += priceTickets; //Adding the tickets to the grand total for later
                Console.Clear();
                Console.WriteLine("You have selected to see {0} rated {1} on screen {2} at {3} and the ticket total is {4:C}.", movies[index].title,
                    movies[index].rating, movies[index].screen.screen, movies[index].times[timeIndex], priceTickets);

                //Add the movie to the orders list
                orders.Add(new Order(index, movies[index].title, movies[index].price, movies[index].times[timeIndex],
                    movies[index].screen.screen, numTickets));

                numMovies++;
                Valid2 = dv.Confirmation("Would you like to pick an additional movie?");

            } //end of overall loop
            Console.Clear();
            Console.WriteLine($"You have selected a total of {numMovies} movies.\n");
            Console.WriteLine("============ Your Receipt ============");
            Console.WriteLine("{0,-20} {1,-10} {2,-10} {3,-10} {4,-10} {5,-10}",
                "Title", "Screen", "Time", "Price", "Quantity", "Subtotal");
            Console.WriteLine("===========================================================================");

            //loop to display all the movies ordered
            foreach (Order o in orders)
            {
                double subTotal = o.price * o.numTickets;
                Console.WriteLine("{0,-20} {1,-10} {2,-10} {3,-10:C} {4,-10} {5,-10:C}",
                    o.title, o.screen, o.showtime, o.price, o.numTickets, subTotal);

            }
            Console.WriteLine("===========================================================================");
            Console.WriteLine("Grand Total: {0,58:C}", grandTotal);
            Console.WriteLine("\nThank you for your purchase and enjoy the magic of the movies!");


        }

        

    }
    public class FileReading  // Class for working with external files
    {

        public void FillMovies(List<Movies> movies, string p)  // Populates the zoo using the animals.txt format
                                                               // (one element per line)
        {

            using (StreamReader sr = new StreamReader(p))
            {
                while (!sr.EndOfStream)
                {
                    Movies movie = new Movies();
                    Screens screen = new Screens();
                    List<string> times = new List<string>();
                    movie.title = sr.ReadLine();
                    movie.rating = ReadRating(sr.ReadLine());
                    screen.screen = Convert.ToInt32(sr.ReadLine());
                    screen.capacity = AssignCapacity(screen.screen);
                    movie.screen = screen;
                    movie.price = Convert.ToDouble(sr.ReadLine());
                    times.Add(sr.ReadLine());
                    times.Add(sr.ReadLine());
                    times.Add(sr.ReadLine());
                    movie.times = times;

                    movies.Add(movie);
                }

                sr.Close();
            }
        } //End of FillMovies method

        public Rating ReadRating(string r) //method to assign Rating to the Movies class
        {
            Rating rating = new Rating();
            try
            {
                switch (r)
                {
                    case "G":
                        rating = Rating.G;
                        break;
                    case "PG":
                        rating = Rating.PG;
                        break;
                    case "PG-13":
                        rating = Rating.PG13;
                        break;
                    case "R":
                        rating = Rating.R;
                        break;
                    case "N-17":
                        rating = Rating.N17;
                        break;
                    default:
                        throw new Exception();
                }
            } catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("Error whilst reading rating");
            }
            return rating;

        } //end of rating method

        public int AssignCapacity(int s) //Method to assign capacity to all the different screens
        {
            try
            {
                switch (s)
                {
                    case 1:
                        return 100;
                    case 2:
                        return 150;
                    case 3:
                        return 100;
                    case 4:
                        return 200;
                    case 5:
                        return 100;
                    case 6:
                        return 250;
                    default:
                        throw new Exception();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error assigning screen capacity on screen {0}",s);
            }
            return -1;
        } //End of capacity method
    }
    
    public class DataValidation
    {
        public int DataTypeValidation(string q) //Method for validating integer data
        {
            string input;
            int idx = 0;
            bool isValid = false;

            while (!isValid)
            {
                isValid = true;

                Console.WriteLine(q);
                input = Console.ReadLine();
                try
                {
                    idx = Convert.ToInt32(input);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message + "Please enter a valid number from the list above.");
                    isValid = false;
                }
            }
            return idx;
        }//end of int validation method
        public bool DataValueValidation(int value, int min, int max) //Method for validating the value
        {
            bool isValid = true;
            if (value < min || value > max)
            {
                Console.WriteLine("Please enter a number between {1} and {0}.", max, min);
                isValid = false;
            }
            return isValid;
        }//End of value validate method

        public bool Confirmation(string s)
        {
            Console.WriteLine(s);
            Console.WriteLine("Press 'y' to confirm");
            ConsoleKeyInfo input = Console.ReadKey();
            if (input.KeyChar == 'y')
            {
                return true;
            }
            else { return false; }
        }


    }
   
}
