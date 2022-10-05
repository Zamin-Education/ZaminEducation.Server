using ZaminEducation.Domain.Entities.MainPages.Commons;

namespace ZaminEducation.Domain.Entities.MainPages;

public class Reason : Text
{
    public long OfferedOpportunitiesId { get; set; }
    public OfferedOpportunities OfferedOpportunities { get; set; }
}