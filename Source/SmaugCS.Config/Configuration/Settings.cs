namespace SmaugCS.Config.Configuration
{
    public class Settings
    {
        public string Database { get; set; }

        public string Races { get; set; }
        public string Classes { get; set; }
        public string Deities { get; set; }
        public string Areas { get; set; }
        public string Languages { get; set; }
        public string Counciles { get; set; }

        public string SysDataFile { get; set; }
        
        public string Host {  get; set; }
        public int Port { get; set; }
        public string MudName { get; set; }
        public bool CheckImmortalHost { get; set; }
        public bool NameResolving { get; set; }
        public bool WaitForAuthorization { get; set; }
        public bool WizardLocked { get; set; }

        public Values Values { get; set; }
        public Directories Directories { get; set; }
    }
}
