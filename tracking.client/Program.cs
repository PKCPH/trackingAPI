using System.Net.Http.Json;

namespace tracking.client;

internal class Program
{
    public static async Task Main(string[] args)
    {
        HttpClient client = new HttpClient();
        //url of the API
        client.BaseAddress = new Uri("https://localhost:7142");
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(
            //clarify that we want result form the server in a json format
            new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json")
            );

        //retrieve all the issues get GetAsync, that is a GET request
        HttpResponseMessage response = await client.GetAsync("api/issue");
        response.EnsureSuccessStatusCode();

        //if we can get the issues from the request, we deserialize the
        if (response.IsSuccessStatusCode)
        {
            //deserializing
            var issues = await response.Content.ReadFromJsonAsync<IEnumerable<IssueDto>>();
            //null check, lazy...
            if (issues == null) return;
            foreach (var issue in issues)
            {
                Console.WriteLine(issue.Title);
            }
        }
        else Console.WriteLine("NoResults");

        Console.ReadLine();
    }
}
