using MongoDB.Bson;

namespace csharp_mongodb_quickstart
{
    public class Telemetry
    {
        public ObjectId Id { get; set; }
        public string? machine_name = Environment.MachineName;
        public System.OperatingSystem platform = Environment.OSVersion;
        public bool platform_64bit = Environment.Is64BitOperatingSystem;
        public System.Version? clr_version = Environment.Version;
        public int cpu_count = Environment.ProcessorCount;
        public string? username = Environment.UserName;
        public string? system_dir = Environment.SystemDirectory;
        public int process_id = Environment.ProcessId;
        public string? process_path = Environment.ProcessPath;
    }
}
