using MongoDB.Bson;
using System;
using Windows.Devices.Bluetooth.Advertisement;

namespace csharp_mongodb_quickstart
{
    public class Telemetry
    {
        public ObjectId Id { get; set; }
        public string? machine_name { get; set; }
        public System.OperatingSystem? platform { get; set; }
        public bool platform_64bit { get; set; }
        public System.Version? clr_version { get; set; }
        public int cpu_count { get; set; }
        public string? username { get; set; }
        public string? system_dir { get; set; }
        public int process_id { get; set; }
        public string? process_path { get; set; }
    }
}
