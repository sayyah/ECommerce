﻿using ECommerce.Domain.Entities;
using System.Text.Json.Serialization;

namespace ECommerce.Application.Services.BlogCategories.Results;

public class BlogCategoryResult
{
    public string Name { get; set; }

    public int? Depth { get; set; } = 0;

    public string? Description { get; set; }

    //ForeignKey
    public int? ParentId { get; set; }
    public BlogCategory? Parent { get; set; }

    [JsonIgnore] public ICollection<BlogCategory>? BlogCategories { get; set; } = new List<BlogCategory>();

    public ICollection<Blog>? Blogs { get; set; }
}
