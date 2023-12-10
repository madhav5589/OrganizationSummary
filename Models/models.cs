namespace OrganizationSummary.Models
{
    public class Organization
    {
        public string id { get; set; }
        public string name { get; set; }
        public string createdAt {  get; set; }
    }

    public class User
    {
        public string id { get; set; }
        public string name { get; set; }
        public string organizationId { get; set; }
        public string createdAt { get; set; }
        public string avatar { get; set; }
        
    }

    public class Phone
    {
        public string id { get; set; }
        public string userId { get; set; }
        public string createdAt { get; set; }
        public string name { get; set; }
        public bool blacklisted { get; set; }

    }

}
