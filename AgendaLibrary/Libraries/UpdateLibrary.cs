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
                    return Tuple.Create(false,new Uri("https://www.youtube.com/watch?v=EgrhzWzfTb4"),exitCode.FetchUpdateFailure);
                }
            } 
            catch (ApiException ae)
            {
                Console.WriteLine("Cannot check for updates.");
                return Tuple.Create(false,new Uri("https://www.youtube.com/watch?v=dYWOi1bK48s"), exitCode.FetchUpdateFailure);
            }
        }

        public async static Task<exitCode> DownloadUpdater()
        {
            var github_client = new GitHubClient(new ProductHeaderValue("subscribe-to-lege"));
            var web_client = new WebClient();
            web_client.Headers.Add(HttpRequestHeader.UserAgent, "subscribe-to-cef");
            if (File.Exists(@"AgendaManagerUpdater.exe")) {
                Console.WriteLine("Updater is already installed and found, skipping download");
                return exitCode.SuccessfulExecution;
            } 
            else
            {
                try
                {
                    var latest = await github_client.Repository.Release.GetLatest("Andrew1013-development", "agenda-manager");
                    try
                    {
                        web_client.DownloadFile(new Uri(latest.Assets.ElementAt(1).BrowserDownloadUrl), @"AgendaManagerUpdater.exe");
                        return exitCode.SuccessfulExecution;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Cannot download updater to perform update.");
                        return exitCode.DownloadUpdateFailure;
                    }
                }
                catch (ApiException ae)
                {
                    Console.WriteLine("Cannot download updater to perform update.");
                    return exitCode.DownloadUpdateFailure;
                }
            } 
        }

        public static exitCode DownloadUpdate(Uri download_uri)
        {
            Console.WriteLine($"Downloading update from {download_uri} to {Directory.GetCurrentDirectory()}.....");
            WebClient client = new WebClient();
            client.Headers.Add(HttpRequestHeader.UserAgent, "subscribe-to-eviel");
            if (File.Exists(@"AgendaManager_update.exe"))
            {
                File.Delete(@"AgendaManager_update.exe");
            }
            try
            {
                client.DownloadFile(download_uri, @"AgendaManager_update.exe");
            } 
            catch (Exception e)
            {
                Console.WriteLine("Cannot download update file.");
                return exitCode.DownloadUpdateFailure;
            }
            Console.WriteLine("Update downloaded.");
            return exitCode.SuccessfulExecution;
        }

        public static exitCode InstallUpdate() 
        {
            Console.WriteLine("Sleeping 30 seconds for Agenda Manager to fully quit exiting (for safety).....");
            Thread.Sleep(30000);
            Console.WriteLine("Creating backup of the old version.....");
            File.Move(@"AgendaManager.exe", @"AgendaManager_old.exe", true);
            Console.WriteLine("Old backup is created.");
            Console.WriteLine("Installing update.....");
            File.Move(@"AgendaManager_update.exe", @"AgendaManager.exe", true);
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
