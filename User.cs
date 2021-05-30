namespace Hibernating_Rhinos_Test
{
    class User
    {
        // Fields ---------------------------------------------------------------------------------
        public string Email;
        public string FullName;
        public int Age;

        //Constructors ----------------------------------------------------------------------------
        public User() { } //defult constructor with no arguments.

        public User(string email, string fullName, int age) // constructor.
        {
            Email = email;
            FullName = fullName;
            Age = age;
        }
    }// End Class User.
}
