﻿#nullable disable
using System.ComponentModel.DataAnnotations;

namespace NewHabr.Domain.Dto;

public abstract class ArticleManipulationDto
{
    [Required, MinLength(3), MaxLength(500)]
    public string Title { get; set; }

    [Required]
    public string Content { get; set; }

    public string ImgURL { get; set; }

    public ICollection<CategoryUpdateRequest> Categories { get; set; }

    public ICollection<TagCreateRequest> Tags { get; set; }

}
