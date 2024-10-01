using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PRO2TS2324EX2;

public class ResultsService : IResultsService
{
    // private string url = "http://www.intakeresults.gradpro.ap.com/api/v1.0/results/";
    private string url = "http://localhost:3000/gradpro/";

    public string Url
    {
        get { return url; }
        set { url = value; }
    }

    public Results GetResults(string student)
    {
        url = url + student;
        using (var httpClient = new HttpClient())
        {
            var httpResponse = httpClient.GetAsync(url).GetAwaiter().GetResult();
            var response = httpResponse.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            return JsonSerializer.Deserialize<Results>(response);
        }
    }
}
