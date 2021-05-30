using System;
using System.Collections.Generic;
using System.Linq;

namespace Hibernating_Rhinos_Test
{
    class Data
    {
        // Fields ---------------------------------------------------------------------------------
        public List<User> Users;
        
        //Constructors ----------------------------------------------------------------------------
        public Data() { } //defult constructor with no arguments.

        public Data(List<User> users) //constructor.
        {
            Users = users;
        }

        //Metods ----------------------------------------------------------------------------------
        
        // Splits the SQL string into three parts: from, where, select.
        // Checks what is the sorce (from).
        // Splits the select part so we will know what fields of the sorce we want to print.
        public void splitsTheSqlString(string SQLstring)
        {
            string[] pattern = { "from", "where", "select" }; // the delimiters of sqlString.
            string[] patternFields = { ",", " " }; // the delimiters of sqlFieldString.
            string[] stringList = SQLstring.Split(pattern, StringSplitOptions.RemoveEmptyEntries); // splits the query
            string[] fields = stringList[2].Split(patternFields, StringSplitOptions.RemoveEmptyEntries); // splits the select part.
            switch (stringList[0]) // check what is the source.
            {
                case " Users ":
                    {
                        splitTheWhereCondition(stringList[1], fields); //sending the where condition to fit it to c#.
                        break;
                    } // If we had more classes we would do more cases for to the soure.
                default: break;
            }

        }

        // Checks if there is an operator in the condition (&, |).
        // Sends to a function suitable for the operator. 
        // If the operator is an AND operator the function sends the decomposition to andCondition function.
        // If the operator is an OR operator the function sends the decomposition to orCondition function.
        // If there is no operator the function sends the initial condition to orCondition function.
        public void splitTheWhereCondition(string whereConditions, string[] fields)
        {
            string[] AND = { "AND" }; string[] OR = { "or" };
            if (whereConditions.Contains("AND"))
            {
                //splits the condition into two picese: the part before the operator and the part after the operator.
                string[] conditions = whereConditions.Split(AND, StringSplitOptions.RemoveEmptyEntries);
                andCondition(conditions, fields); //sending the picese to the andCondition function.
            }
            else
            {
                //splits the condition into two picees: the part before the operator and the part after the operator.
                string[] conditions = whereConditions.Split(OR, StringSplitOptions.RemoveEmptyEntries);
                orCondition(conditions, fields); //sending the picese to the orCondition function.
            }
        }

        // If there is an AND operator in the SQL query, the two parts must exist.
        //Therefore if the flag = 2, the two parts exist.
        //If not, the user is not the result of the query
        public void andCondition(string[] ANDConditions, string[] fields)
        {
            foreach (User u in Users) // Goes through each user in the user list.
            {
                int flag = 0;
                flag = splitsAndOrConditions(ANDConditions, flag, u); // Checks whether the user meets the conditions.
                if (flag == 2)
                {
                    printUser(fields, u);
                }
            }
        }

        // If there is an OR operator in the SQL query, one part must exist.
        //Therefore if the flag = 1 one part exist, if the flag = 2 the two parts exist. 
        //If not, the user is not the result of the query
        public void orCondition(string[] ORConditions, string[] fields)
        {
            foreach (User u in Users) // Goes through each user in the user list.
            {
                int flag = 0;
                flag = splitsAndOrConditions(ORConditions, flag, u); // Checks whether the user meets the conditions.
                if (flag > 1 || flag == 1)
                {
                    printUser(fields, u);
                }
            }
        }

        // For each part of the query the function checks whether it is a comparison, greater than or less than.
        public int splitsAndOrConditions(string[] WhereConditions, int flag, User u)
        {
            // split each part of the condition into two parts: before the operator and after the operator.
            // when we check whether a variable is large or small we must delete the spaces.
            // Therefore we must add to the divisor the {" "} string in addition to the "<" or ">".
            // Only when I use comparison I do not have to delete the spaces because I use the contain function in comparison
            // so it is enough for me to use the divider "="
            string[] equal = {"="}; string[] big = { ">", " " }; string[] small = { "<", " " };
               foreach (string s in WhereConditions) 
                    {
                        if (s.Contains("=")) // Checks if my condition contains the operator =.
                        {
                            string rightExpression = s.Split(equal, StringSplitOptions.RemoveEmptyEntries).First(); //Splits the first variable I want to compare.
                            string leftExpression = s.Split(equal, StringSplitOptions.RemoveEmptyEntries).Last(); //Splits the second variable I want to compare.
                            string field = check1Varibale(rightExpression, u); // Checking which field in the class I want to compare.
                            flag += isEquals(field, leftExpression); // Sends to a function that compares the two variables and returns 1 if they are equal.
                        }
                        else
                        if (s.Contains(">")) // Checks if my condition contains the operator >.
                        {
                            string rightExpression = s.Split(big, StringSplitOptions.RemoveEmptyEntries).First(); //Splits the first variable I want to compare.
                            string leftExpression = s.Split(big, StringSplitOptions.RemoveEmptyEntries).Last(); //Splits the second variable I want to compare.
                            string field = check1Varibale(rightExpression, u); // Checking which field in the class I want to compare.
                            flag += IsBigger(field, leftExpression); // Sends to a function that checks whether variable 1 is greater than variable 2. if so returns 1.
                        }
                        else
                        if (s.Contains("<")) // Checks if my condition contains the operator <.
                        {
                            string rightExpression = s.Split(small, StringSplitOptions.RemoveEmptyEntries).First(); //Splits the first variable I want to compare.
                            string leftExpression = s.Split(small, StringSplitOptions.RemoveEmptyEntries).Last(); //Splits the second variable I want to compare.
                            string field = check1Varibale(rightExpression, u); // Checking which field in the class I want to compare.
                            flag += IsSmaller(field, leftExpression); // Sends to a function that checks whether variable 1 is smaller than variable 2. if so returns 1.
                        }
                        else flag = 0; 
                    }  
            return flag;
         }

        // Checks which varible the query compare.
        public string check1Varibale(string rightExpression, User u)
        {
            switch (rightExpression)
            {
                case " FullName ":
                    {
                        rightExpression = u.FullName;
                        return rightExpression;
                    }
                case " Email ":
                    {
                        rightExpression = u.Email;
                        return rightExpression;
                    }
                default:
                    {
                        rightExpression = (u.Age).ToString();
                        return rightExpression;
                    }
            }
        }

        // Compares two strings. Returns 1 if they are equal.
        public int isEquals(string rightExpression, string leftExpression)
        {
            if (leftExpression.Contains(rightExpression))
                return 1;
            else return 0;
        }

        // Compares two numbers and returns 1 if the first number is larger than the second.
        public int IsBigger(string rightExpression, string leftExpression)
        {
            int re = int.Parse(rightExpression); int le = int.Parse(leftExpression); // Parse int because I accept the variable as a string.
            if (re > le)
                return 1;
            else return 0;
        }

        // Compares two numbers and returns 1 if the first number is smaller than the second.
        public int IsSmaller(string rightExpression, string leftExpression)
        {
            int re = int.Parse(rightExpression); int le = int.Parse(leftExpression); // Parse int because I accept the variable as a string.
            if (re < le)
                return 1;
            else return 0;
        }

        // Receives the fields that the query requested to print and the user that meets the query requirements and prints to the console.
        public void printUser(string[] fileds, User u)
        {
            for (int i = 0; i < fileds.Length; i++)
            {
                if (fileds[i] == "FullName")
                    Console.WriteLine(u.FullName);
                else
                    if (fileds[i] == "Email")
                    Console.WriteLine(u.Email);
                else
                    Console.WriteLine(u.Age);
            }
            Console.WriteLine("--------------------");
        }
    }

}

