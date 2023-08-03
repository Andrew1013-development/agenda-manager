using System;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using Octokit;
using System.Text;

namespace AgendaLibrary
{
    public class UpdateLibrary
    {
        public static async Task<Tuple<bool,Uri,AgendaLibrary.exitCode>> CheckForUpdate(string current_version)
        {
            var client = new GitHubClient(new ProductHeaderValue("subscribe-to-hamyly"));
            try 
            {
                var latest = await client.Repository.Release.GetLatest("Andrew1013-development", "agenda-manager");
                Console.WriteLine($"The latest release version name is: {latest.TagName}");
                Console.WriteLine($"Description of release: {latest.Body}");
                Console.WriteLine($"URL of release: {latest.Url}");
                Console.WriteLine($"Release assets' URL: {latest.AssetsUrl}");
                if (compare_versions(current_version, latest.TagName))
                {
                    Console.WriteLine("Update detected, ready to install and update");
                    return Tuple.Create(true, new Uri(latest.Assets.ElementAt(0).BrowserDownloadUrl), exitCode.SuccessfulExecution);
                } else
                {
                    return Tuple.Create(false,new Uri(""),exitCode.FetchUpdateFailure);
                }
            } 
            catch (ApiException ae)
            {
                Console.WriteLine("Cannot check for updates.");
                return Tuple.Create(false,new Uri(""), exitCode.FetchUpdateFailure);
            }
        }
        public static exitCode DownloadUpdate(Uri download_uri)
        {
            Console.WriteLine($"Downloading update from {download_uri} to {Directory.GetCurrentDirectory()}.....");
            WebClient client = new WebClient();
            client.Headers.Add(HttpRequestHeader.UserAgent, "subscribe-to-eviel");
            if (File.Exists(@"AgendaManager_update.zip"))
            {
                File.Delete(@"AgendaManager_update.zip");
            }
            client.DownloadFile(download_uri, @"AgendaManager_update.exe");
            Console.WriteLine("Update downloaded.");
            return exitCode.SuccessfulExecution;
        }
        public static exitCode InstallUpdate(Uri download_uri) 
        {
            Console.WriteLine("Installing update.....");
            Console.WriteLine("Update installed.");
            return exitCode.SuccessfulExecution;
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
