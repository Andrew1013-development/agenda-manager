using Octokit;
using AgendaLibrary.Definitions;

namespace AgendaLibrary.Libraries
{
    public class UpdateLibrary
    {
        // localized
        public static async Task<Tuple<bool,Uri,exitCode>> CheckForUpdate(string current_version, bool decreased_output)
        {
            Console.WriteLine($"{LanguageLibrary.GetString("check_update")}");
            var client = new GitHubClient(new ProductHeaderValue("subscribe-to-hamyly"));
            try 
            {
                var latest = await client.Repository.Release.GetLatest("Andrew1013-development", "agenda-manager");
                Console.WriteLine($"{LanguageLibrary.GetString("latest_update")}: {latest.TagName}");
                if (compare_versions(current_version, latest.TagName))
                {
                    Console.WriteLine($"{LanguageLibrary.GetString("new_update")}");
                    Console.WriteLine($"{LanguageLibrary.GetString("latest_update_name")}: {latest.TagName}");
                    if (!decreased_output)
                    {
                        Console.WriteLine($"{LanguageLibrary.GetString("update_description")}: {latest.Body}");
                        Console.WriteLine($"{LanguageLibrary.GetString("update_url")}: {latest.Url}");
                        //Console.WriteLine($"Release assets' URL: {latest.AssetsUrl}");
                    }
                    return Tuple.Create(true, new Uri(latest.Assets.ElementAt(0).BrowserDownloadUrl), exitCode.SuccessfulExecution);
                } else
                {
                    Console.WriteLine($"{LanguageLibrary.GetString("no_new_update")}");
                    return Tuple.Create(false,new Uri("https://www.youtube.com/watch?v=EgrhzWzfTb4"),exitCode.SuccessfulExecution);
                }
            } 
            catch (ApiException)
            {
                Console.WriteLine($"{LanguageLibrary.GetString("check_update_error")}");
                return Tuple.Create(false,new Uri("https://www.youtube.com/watch?v=dYWOi1bK48s"), exitCode.FetchUpdateFailure);
            }
        }
        // localized
        public static async Task<exitCode> DownloadUpdater(bool decreased_output)
        {
            var github_client = new GitHubClient(new ProductHeaderValue("subscribe-to-lege"));
            Console.WriteLine($"{LanguageLibrary.GetString("check_old_updater")}");
            
            if (File.Exists(@"AgendaManagerUpdater.exe")) {
                if (!decreased_output)
                {
                    Console.WriteLine($"{LanguageLibrary.GetString("delete_old_updater")}");
                }
                File.Delete(@"AgendaManagerUpdater.exe");
                if (!decreased_output)
                {
                    Console.WriteLine($"{LanguageLibrary.GetString("delete_old_updater_finish")}");
                }
            } else
            {
                Console.WriteLine($"{LanguageLibrary.GetString("no_old_updater")}");
            }
            try
            {
                if (!decreased_output)
                {
                    Console.WriteLine($"{LanguageLibrary.GetString("download_updater")}");
                }
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
                        } 
                        catch (Exception)
                        {
                            Console.WriteLine($"{LanguageLibrary.GetString("download_updater_error")}");
                            return exitCode.DownloadUpdaterFailure;
                        }
                    }
                    Console.WriteLine($"{LanguageLibrary.GetString("download_updater_finish")}");
                    return exitCode.SuccessfulExecution;
                }
                catch (Exception)
                {
                    Console.WriteLine($"{LanguageLibrary.GetString("download_updater_error")}");
                    return exitCode.DownloadUpdaterFailure;
                }
            }
            catch (ApiException)
            {
                Console.WriteLine($"{LanguageLibrary.GetString("download_updater_error")}");
                return exitCode.DownloadUpdaterFailure;
            }
        }
        // localized
        public static async Task<exitCode> DownloadUpdate(Uri download_uri)
        {
            Console.WriteLine($"{LanguageLibrary.GetString("check_old_update")}");
            // check if old update already exists -> delete
            if (File.Exists(@"AgendaManager_update.exe"))
            {
                Console.WriteLine($"{LanguageLibrary.GetString("delete_old_update")}");
                File.Delete(@"AgendaManager_update.exe");
                Console.WriteLine($"{LanguageLibrary.GetString("delete_old_update_finish")}");
            } else
            {
                Console.WriteLine($"{LanguageLibrary.GetString("no_old_update")}");
            }
            // download new update
            try
            {
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Add("user-agent", "subscribe-to-eviel");
                var response = await client.GetAsync(download_uri);
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("");
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
                        Console.WriteLine($"{LanguageLibrary.GetString("download_update_error")}");
                        return exitCode.DownloadUpdateFailure;
                    }
                    Console.WriteLine($"{LanguageLibrary.GetString("download_update_finish")}");
                    return exitCode.SuccessfulExecution;
                } else
                {
                    Console.WriteLine($"{LanguageLibrary.GetString("download_update_error")}");
                    return exitCode.DownloadUpdateFailure;
                }
            } 
            catch (Exception)
            {
                Console.WriteLine($"{LanguageLibrary.GetString("download_update_error")}");
                return exitCode.DownloadUpdateFailure;
            }
        }
        // localized
        public static exitCode InstallUpdate() 
        {
            Console.WriteLine($"{LanguageLibrary.GetString("wait_update")}");
            Thread.Sleep(30000);
            Console.WriteLine($"{LanguageLibrary.GetString("backup_old")}");
            File.Move(@"AgendaManager.exe", @"AgendaManager_old.exe", true);
            Console.WriteLine($"{LanguageLibrary.GetString("backup_old_finish")}");
            try
            {
                Console.WriteLine($"{LanguageLibrary.GetString("install_update")}");
                File.Move(@"AgendaManager_update.exe", @"AgendaManager.exe", true);
                Console.WriteLine($"{LanguageLibrary.GetString("install_update_finish")}");
                return exitCode.SuccessfulExecution;
            } catch (Exception)
            {
                // rollback
                Console.WriteLine($"{LanguageLibrary.GetString("install_update_error")}");
                Console.WriteLine($"{LanguageLibrary.GetString("rollback_update")}");
                File.Move(@"AgendaManager_old.exe",@"AgendaManager.exe");
                Console.WriteLine($"{LanguageLibrary.GetString("rollback_update_finish")}");
                return exitCode.InstallUpdateFailure;
            }
        }

        internal static bool compare_versions(string src_version, string upd_version)
        {
            bool update_confirmed = false;
            string[] src_version_list = src_version.Split(".");
            string[] upd_version_list = upd_version.Split(".");
            for (int i = 0; i < src_version_list.Length; i++) 
            {
                // true = update needed, false = no update needed
                //Console.WriteLine($"{int.Parse(src_version_list[i])} {int.Parse(upd_version_list[i])} {update_confirmed}");
                if (int.Parse(upd_version_list[i]) != int.Parse(src_version_list[i]))
                {
                    // if version number is different -> check if update version or current version is "higher"
                    update_confirmed = (int.Parse(upd_version_list[i]) > int.Parse(src_version_list[i]));
                    break;
                }
            }
            //Console.WriteLine(update_confirmed);
            return update_confirmed;
        }
    }
}
