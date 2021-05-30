using System;
using System.Collections.Generic;

namespace Hibernating_Rhinos_Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("The query result:");
            Console.WriteLine("--------------------");

            User shay = new User("shaymautner0203@gmail.com", "shay mautner", 25); // bulding new user.
            User john = new User("John@gmail.com", "John Doe", 31); // bulding new user.

            List<User> users = new List<User>(); //bulding new list of users.
            users.Add(shay); // adding user shay to the list.
            users.Add(john); // adding user john to the list.

            Data d = new Data(users); // bulding new data with the list of users.

            // sending the sql string to the engine.
            d.splitsTheSqlString("from Users where Email = 'John@gmail.com' or Email = 'shaymautner0203@gmail.com' select FullName, Email"); // the query we want to solve.

            Console.ReadLine(); // function that keep the console open.
        } // End Main function.
    }// End Class Program.
}
