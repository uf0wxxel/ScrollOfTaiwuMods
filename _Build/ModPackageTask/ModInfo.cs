namespace ModPackageTask
{
    public class ModInfo
    {
        public string Id { get; set; }

        public string DisplayName { get; set; }

        public string Author { get; set; }

        public string Version { get; set; }

        public string ManagerVersion { get; set; }

        public string[] Requirements { get; set; }

        public string AssemblyName { get; set; }

        public string EntryMethod { get; set; }

        public string HomePage { get; set; }

        public string Repository { get; set; }

        public bool Validate()
        {
            DisplayName = DisplayName.Trim();
            return !string.IsNullOrEmpty(DisplayName);
        }
    }
}
