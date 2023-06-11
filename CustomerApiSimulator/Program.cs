using System.Text.Json;
using System.Text;
using CustomerApiSimulator;

class Program
{
    private static int port = 7069; //The port number where CustoemrApi is running.
    private static string baseUrl = $"https://localhost:{port}";
    private static string endPoint = baseUrl + "/Customer";

    private static string[] firstNames = new string[10] { "Leia", "Sadie", "Jose", "Sara", "Frank", "Dewey", "Tomas", "Joel", "Lukas", "Carlos" };
    private static string[] lastNames = new string[10] { "Liberty", "Ray", "Harrison", "Ronan", "Drew", "Powell", "Larsen", "Chan", "Anderson", "Lane" };

    private static Random rand = new Random();

    static async Task Main(string[] args)
    {
        var tasks = new List<Task>();
        int customerId = 1;
        int postCounter = 1;
        for (int i = 0; i <= 10; i++)
        {
            //each request is sent with a different thread.
            tasks.Add(Task.Run(async () =>
            {
                await SendRequest(customerId, postCounter);
            }));


            postCounter++;

            //Since there are two customers in each post request, it increases it by 2.
            customerId += 2;
        }

        await Task.WhenAll(tasks);
    }

    private static async Task SendRequest(int customerId, int postCounter)
    {

        if (postCounter % 3 == 0) //A get request will be sent after every 3 post requests.
        {
            await InnerSendRequest(false, null);
        }

        using (var httpClient = new HttpClient())
        {
            var jsonBody = CreatePostRequestBody(customerId);
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
            await InnerSendRequest(true, content);
        }

    }

    //The same method is used for post and get requests.
    private static async Task InnerSendRequest(bool sendPostRequest, StringContent? content)
    {

        using (var httpClient = new HttpClient())
        {
            var response = sendPostRequest
                ? await httpClient.PostAsync(endPoint, content)
                : await httpClient.GetAsync(endPoint);

            if (response.IsSuccessStatusCode)
            {
                var responseText = await response.Content.ReadAsStringAsync();

                var title = sendPostRequest
                    ? "POST request. Content:"
                    : "GET request:";
                Console.WriteLine($"{title} {string.Join(", ", responseText)}");
                Console.WriteLine($"--");
                Console.WriteLine($"--");

            }
            else
            {
                var title = sendPostRequest
                    ? "POST request failed with status code:"
                    : "Failed to get customers:";
                Console.WriteLine($"{title} {response.StatusCode}");
            }
        }

    }

    // It reates Customer Models to use in post request
    private static string CreatePostRequestBody(int id)
    {
        MCustomer[] customers = new MCustomer[]
        {
                new MCustomer(id: id, age: rand.Next(10, 91), firstName: firstNames[rand.Next(0, 10)], lastName: lastNames[rand.Next(0, 10)]),
                new MCustomer(id: id+1, age: rand.Next(10, 91), firstName: firstNames[rand.Next(0, 10)], lastName: lastNames[rand.Next(0, 10)]),

        };

        return JsonSerializer.Serialize(value: customers);
    }
}