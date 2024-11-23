using Microsoft.AspNetCore.Mvc.Rendering;

namespace NATS.Models;

public class PostDetailViewModel
{
    [Display(Name = DisplayNames.Id)]
    public int Id { get; set; }
    
    [Display(Name = DisplayNames.Title)]
    [Required]
    [MaxLength(150)]
    public string Title { get; set; }
    
    [Display(Name = DisplayNames.Thumbnail)]
    [BindNever]
    public string ThumbnailUrl { get; set; }
    
    [Display(Name = DisplayNames.Content)]
    [Required]
    [MaxLength(10000)]
    public string Content { get; set; }
    
    [Display(Name = DisplayNames.CreatedDateTime)]
    [BindNever]
    public DateTime CreatedDateTime { get; set; }
    
    [Display(Name = DisplayNames.UpdatedDateTime)]
    [BindNever]
    public DateTime? UpdatedDateTime { get; set; }
    
    [Display(Name = DisplayNames.IsPinned)]
    [Required]
    public bool IsPinned { get; set; }
    
    [Display(Name = DisplayNames.IsPublished)]
    [Required]
    public bool IsPublished { get; set; }
    
    [Display(Name = DisplayNames.Views)]
    [BindNever]
    public int Views { get; set; }
    
    [Display(Name = DisplayNames.User)]
    [BindNever]
    public UserBasicViewModel User { get; set; }
    
    public bool ThumbnailChanged { get; set; }
    public IFormFile ThumbnailFile { get; set; }
    
    [BindNever]
    public bool IsForCreating { get; set; }
}