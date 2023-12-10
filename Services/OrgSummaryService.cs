using OrganizationSummary.Models;

public class OrgSummaryService
{
    private readonly HttpClient _httpClient;

    public OrgSummaryService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<OrganizationSummaryModel>> GetOrganizationSummariesAsync()
    {
        const int maxRetries = 5;
        int retries = 0;

        // Index to keep track of the last successfully processed organization
        int lastProcessedIndex = 0;
        var summaries = new List<OrganizationSummaryModel>();

        while (retries < maxRetries)
        {
            try
            {
                // Fetch organizations
                var organizations = await _httpClient.GetFromJsonAsync<List<Organization>>("https://607a0575bd56a60017ba2618.mockapi.io/organization");

                for (int i = lastProcessedIndex; i < organizations.Count; i = i + 1)
                {
                    var organization = organizations[i];

                    // Fetch users for the organization
                    var users = await _httpClient.GetFromJsonAsync<List<User>>($"https://607a0575bd56a60017ba2618.mockapi.io/organization/{organization.id}/users");

                    // Fetch phones for each user
                    var phones = await FetchPhonesForUsersAsync(users);

                    // Generate summary
                    var organizationSummary = new OrganizationSummaryModel
                    {
                        Id = organization.id,
                        Name = organization.name,
                        BlacklistTotal = phones.Count(p => p.blacklisted),
                        Users = users.Select(user => new UserSummaryModel
                        {
                            Id = user.id,
                            Name = user.name,
                            PhoneCount = phones.Count(p => p.userId == user.id)
                        }).ToList()
                    };

                    summaries.Add(organizationSummary);

                    // Update the last successfully processed organization index
                    lastProcessedIndex = i;
                }

                return summaries;
            }
            catch (HttpRequestException ex) when (ex.Message.Contains("429"))
            {
                // Log the error or handle as needed
                Console.WriteLine($"Received 429 for organization. Retrying... Exception: {ex}");
                Console.WriteLine($"Received 429. Retrying in {Math.Pow(2, retries)} seconds.");

                // Exponential backoff
                await Task.Delay(TimeSpan.FromSeconds(20));

                retries++;
            }
        }

        // If retries are exhausted, throw an exception or handle as needed
        throw new ApplicationException("Failed to retrieve data after multiple retries.");
    }

    private async Task<List<Phone>> FetchPhonesForUsersAsync(List<User> users)
    {
        var phones = new List<Phone>();

        foreach (var user in users)
        {
            var userPhones = await _httpClient.GetFromJsonAsync<List<Phone>>($"https://607a0575bd56a60017ba2618.mockapi.io/organization/{user.organizationId}/users/{user.id}/phones");
            phones.AddRange(userPhones);
        }

        return phones;
    }
}
