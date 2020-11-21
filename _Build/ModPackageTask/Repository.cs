using System;

namespace ModPackageTask
{
    public class Repository
    {
        public Release[] Releases { get; set; }
    }

    [Serializable]
    public class Release
    {
        public string Id { get; set; }

        public string Version { get; set; }

        public string DownloadUrl { get; set; }
    }
}
