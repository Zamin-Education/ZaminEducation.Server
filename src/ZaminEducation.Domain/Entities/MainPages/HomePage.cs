using System.ComponentModel.DataAnnotations.Schema;
using ZaminEducation.Domain.Commons;
using ZaminEducation.Domain.Entities.Courses;

namespace ZaminEducation.Domain.Entities.MainPages;

public class HomePage : Auditable
{
    public long HomePageHeaderId { get; set; }
    public HomePageHeader HomePageHeader { get; set; }

    [ForeignKey(nameof(InfoAboutProject))]
    public long InfoAboutProjectId { get; set; }
    public InfoAboutProject InfoAboutProject { get; set; }

    [NotMapped]
    public IEnumerable<Course> PopularCourses { get; set; }

    public long OfferedOpportunitiesId { get; set; }
    public OfferedOpportunities OpportunitiesOffered { get; set; }

    public long PhotoGalleryId { get; set; }
    public PhotoGallery PhotoGallery { get; set; }

    public long SocialNetworksId { get; set; }
    public SocialNetworks SocialNetworks { get; set; }

}