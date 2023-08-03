using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Net.Http;
using Octokit;

namespace AgendaLibrary
{    
    public class UpdateLibrary
    {   
        public static async Task<Tuple<bool,string,AgendaLibrary.exitCode>> CheckForUpdate(string current_version)
        {
            var client = new GitHubClient(new ProductHeaderValue("subscribe-to-hamyly"));
            try 
            {
                var latest = await client.Repository.Release.GetLatest("Andrew1013-development", "agenda-manager");
                Console.WriteLine($"The latest release version name is: {latest.TagName}");
                Console.WriteLine($"Description of release: {latest.Body}");
                Console.WriteLine($"URL of release: {latest.Url}");
                if (compare_versions(current_version, latest.TagName))
                {
                    Console.WriteLine("Update detected.");
                    return Tuple.Create(true,latest.Url, exitCode.SuccessfulExecution);
                } else
                {
                    return Tuple.Create(false,"",exitCode.FetchUpdateFailure);
                }
            } 
            catch (ApiException ae)
            {
                Console.WriteLine("Cannot check for updates.");
                return Tuple.Create(false,"",exitCode.FetchUpdateFailure);
            }
        }
        public static async void InstallUpdate(string update_url) 
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("User-Agent", "subscribe-to-hamyly");
            client.DefaultRequestHeaders.Add("Accept", "application/vnd.github+json");
            
            try
            {
                HttpResponseMessage response = await client.GetAsync(update_url);
                Console.WriteLine(response.StatusCode);
                response.EnsureSuccessStatusCode();
            } catch (HttpRequestException hqe)
            {
                Console.WriteLine("Cannot download update, request failed.");
                Console.WriteLine($"Status code: {hqe.StatusCode}");
            } 
            
        }

        // helper functions
        internal static bool compare_versions(string src_version, string upd_version)
        {
            
            string[] version1_list = src_version.Split(".");
            string[] version2_list = upd_version.Split(".");
            for (int i = 0; i < 4; i++) 
            {
                // true = update needed, false = no update needed
                if (int.Parse(version1_list[i]) < int.Parse(version2_list[i]))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
