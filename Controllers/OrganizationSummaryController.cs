using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class OrganizationSummaryController : ControllerBase
{
    private readonly OrgSummaryService _organizationSummaryService;

    public OrganizationSummaryController(OrgSummaryService organizationSummaryService)
    {
        _organizationSummaryService = organizationSummaryService;
    }

    [HttpGet]
    public async Task<IActionResult> GetOrganizationSummaries()
    {
        var summaries = await _organizationSummaryService.GetOrganizationSummariesAsync();
        return Ok(summaries);
    }
}
