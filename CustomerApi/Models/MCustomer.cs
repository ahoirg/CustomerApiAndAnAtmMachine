namespace CustomerApi.Models
{
    // It contains the Customer model
    public class MCustomer
    {
        public int Id { get; set; }
        public int Age { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public MCustomer(int id, int age, string firstName, string lastName)
        {
            Id = id;
            Age = age;
            FirstName = firstName;
            LastName = lastName;
        }
    }
}
