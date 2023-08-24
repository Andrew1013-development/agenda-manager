using Octokit;
using AgendaLibrary.Definitions;

namespace AgendaLibrary.Libraries
{
    public class UpdateLibrary
    {
        public static async Task<Tuple<bool,Uri,exitCode>> CheckForUpdate(string current_version, bool decreased_output)
        {
            var client = new GitHubClient(new ProductHeaderValue("subscribe-to-hamyly"));
            try 
            {
                var latest = await client.Repository.Release.GetLatest("Andrew1013-development", "agenda-manager");
                Console.WriteLine($"Latest version available: {latest.TagName}");
                if (compare_versions(current_version, latest.TagName))
                {
                    Console.WriteLine("Update detected, ready to install and update");
                    Console.WriteLine($"The latest release version name: {latest.TagName}");
                    if (!decreased_output)
                    {
                        Console.WriteLine($"Description of release: {latest.Body}");
                        Console.WriteLine($"URL of release: {latest.Url}");
                        Console.WriteLine($"Release assets' URL: {latest.AssetsUrl}");
                    }
                    return Tuple.Create(true, new Uri(latest.Assets.ElementAt(0).BrowserDownloadUrl), exitCode.SuccessfulExecution);
                } else
                {
                    Console.WriteLine("Your program is updated.");
                    return Tuple.Create(false,new Uri("https://www.youtube.com/watch?v=EgrhzWzfTb4"),exitCode.SuccessfulExecution);
                }
            } 
            catch (ApiException)
            {
                Console.WriteLine("Cannot check for updates.");
                return Tuple.Create(false,new Uri("https://www.youtube.com/watch?v=dYWOi1bK48s"), exitCode.FetchUpdateFailure);
            }
        }

        public static async Task<exitCode> DownloadUpdater()
        {
            var github_client = new GitHubClient(new ProductHeaderValue("subscribe-to-lege"));
            if (File.Exists(@"AgendaManagerUpdater.exe")) {
                Console.WriteLine("Old updater is found, deleting old version");
                File.Delete(@"AgendaManagerUpdater.exe");
            } 
            try
            {
                var latest = await github_client.Repository.Release.GetLatest("Andrew1013-development", "agenda-manager");
                try
                {
                    Uri download_uri = new Uri(latest.Assets.ElementAt(1).BrowserDownloadUrl);
                    HttpClient client = new HttpClient();
                    client.DefaultRequestHeaders.Add("user-agent", "subscribe-to-cef");
                    var response = await client.GetAsync(download_uri);
                    if (response.IsSuccessStatusCode)
                    {
                        try
                        {
                            FileStream file_stream = new FileStream(@"AgendaManagerUpdater.exe", System.IO.FileMode.Create);
                            Stream web_stream = await client.GetStreamAsync(download_uri);
                            web_stream.CopyTo(file_stream);
                            file_stream.Close(); // must close otherwise it's still accessing the file
                            web_stream.Close();
                            Console.WriteLine("Finished downloading updater.");
                        } 
                        catch (Exception)
                        {
                            Console.WriteLine("Cannot download updater to perform update.");
                            return exitCode.DownloadUpdaterFailure;
                        }
                    }
                    return exitCode.SuccessfulExecution;
                }
                catch (Exception)
                {
                    Console.WriteLine("Cannot download updater to perform update.");
                    return exitCode.DownloadUpdaterFailure;
                }
            }
            catch (ApiException)
            {
                Console.WriteLine("Cannot download updater to perform update.");
                return exitCode.DownloadUpdaterFailure;
            }
        }

        public static async Task<exitCode> DownloadUpdate(Uri download_uri)
        {
            Console.WriteLine($"Downloading update from {download_uri} to {Directory.GetCurrentDirectory()}.....");
            // check if old update already exists -> delete
            if (File.Exists(@"AgendaManager_update.exe"))
            {
                File.Delete(@"AgendaManager_update.exe");
            }
            // download new update
            try
            {
                //WebClient client = new WebClient();
                //client.Headers.Add(HttpRequestHeader.UserAgent, "subscribe-to-eviel");
                //client.DownloadFile(download_uri, @"AgendaManager_update.exe");
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Add("user-agent", "subscribe-to-eviel");
                var response = await client.GetAsync(download_uri);
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Downloading updater.....");
                    try
                    {
                        FileStream file_stream = new FileStream(@"AgendaManager_update.exe", System.IO.FileMode.Create);
                        Stream web_stream = await client.GetStreamAsync(download_uri);
                        web_stream.CopyTo(file_stream);
                        file_stream.Close(); // must close otherwise it's still accessing the file
                        web_stream.Close();
                    } 
                    catch (Exception)
                    {
                        Console.WriteLine("Cannot download update file.");
                        return exitCode.DownloadUpdateFailure;
                    }
                    
                } else
                {
                    Console.WriteLine("Cannot download update file.");
                    return exitCode.DownloadUpdateFailure;
                }
            } 
            catch (Exception)
            {
                Console.WriteLine("Cannot download update file.");
                return exitCode.DownloadUpdateFailure;
            }
            Console.WriteLine("Update downloaded.");
            return exitCode.SuccessfulExecution;
        }

        public static exitCode InstallUpdate() 
        {
            Console.WriteLine("Waiting 30 seconds for Agenda Manager to fully quit exiting (for safety).....");
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
            bool update_confirmed = false;
            string[] src_version_list = src_version.Split(".");
            string[] upd_version_list = upd_version.Split(".");
            for (int i = 0; i < 4; i++) 
            {
                // true = update needed, false = no update needed
                //Console.WriteLine($"{int.Parse(src_version_list[i])} {int.Parse(upd_version_list[i])}");
                if (int.Parse(src_version_list[i]) < int.Parse(upd_version_list[i]))
                {
                    update_confirmed = true;
                }
            }
            //Console.WriteLine(update_confirmed);
            return update_confirmed;
        }
    }
}
