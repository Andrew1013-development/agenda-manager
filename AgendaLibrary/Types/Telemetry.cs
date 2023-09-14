using MongoDB.Bson;
using System.Diagnostics;

namespace AgendaLibrary.Types
{
    public class Telemetry
    {
        public ObjectId Id { get; set; }
        public string? machine_name { get; set; }
        public System.OperatingSystem platform { get; set; }
        public bool platform_64bit { get; set; }
        public System.Version? clr_version { get; set; }
        public int cpu_count { get; set; }
        public string? username { get; set; }
        public string? system_dir { get; set; }
        public int process_id { get; set; }
        public string? process_path { get; set; }
        public Telemetry()
        {
            machine_name = Environment.MachineName;
            platform = Environment.OSVersion;
            platform_64bit = Environment.Is64BitOperatingSystem;
            clr_version = Environment.Version;
            cpu_count = Environment.ProcessorCount;
            username = Environment.UserName;
            system_dir = Environment.SystemDirectory;
            process_id = Process.GetCurrentProcess().Id;
            process_path = Process.GetCurrentProcess().ProcessName;
        }
    }
}
