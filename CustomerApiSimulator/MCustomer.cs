using System.Text.Json.Serialization;

namespace CustomerApiSimulator
{
    public class MCustomer
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("age")]
        public int Age { get; set; }

        [JsonPropertyName("firstName")]
        public string FirstName { get; set; }

        [JsonPropertyName("lastName")]
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
