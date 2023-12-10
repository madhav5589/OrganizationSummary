namespace OrganizationSummary.Models
{
    public class OrganizationSummaryModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int BlacklistTotal { get; set; }
        public int TotalCount { get; set; }
        public List<UserSummaryModel> Users { get; set; }
    }

    public class UserSummaryModel
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public int PhoneCount { get; set; }
    }

}
