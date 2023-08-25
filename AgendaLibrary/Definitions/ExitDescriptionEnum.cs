using AgendaLibrary.Libraries;

namespace AgendaLibrary.Definitions
{
    public class exitDescriptions
    {
        public static string exitSuccessfulExecution = $"{LanguageLibrary.GetString("exit_successful_execution")}";
        public static string exitDatabaseConnectionFailure = $"{LanguageLibrary.GetString("exit_database_connection_failure")}";
        public static string exitCollectionRetrievalFailure = $"{LanguageLibrary.GetString("exit_connection_retrieval_failure")}";
        public static string exitInvalidRole = $"{LanguageLibrary.GetString("exit_invalid_role")}";
        public static string exitInvalidInput = $"{LanguageLibrary.GetString("exit_invalid_input")}";
        public static string exitWrongPassword = $"{LanguageLibrary.GetString("exit_wrong_password")}";
        public static string exitDatabaseUploadFailure = $"{LanguageLibrary.GetString("exit_database_upload_failure")}";
        public static string exitSecretMenuAccessed = $"{LanguageLibrary.GetString("exit_secret_menu")}";
        public static string exitFetchUpdateFailure = $"{LanguageLibrary.GetString("exit_fetch_update_failure")}";
        public static string exitDownloadUpdateFailure = $"{LanguageLibrary.GetString("exit_download_update_failure")}";
        public static string exitDownloadUpdaterFailure = $"{LanguageLibrary.GetString("exit_download_updater_failure")}";
        public static string exitInstallUpdateFailure = $"{LanguageLibrary.GetString("exit_install_update_failure")}";
        public static string exitUnknownExitCode = $"{LanguageLibrary.GetString("exit_unknown")}";
    }
}
