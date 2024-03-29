﻿using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ECommerce.Domain.Entities;

public class ProductAttribute : RootEntity
{
    [Display(Name = "عنوان")]
    [StringLength(500, MinimumLength = 2, ErrorMessage = @"حداقل 2 و حداکثر 050 کاراکتر")]
    [Required(ErrorMessage = @"{0} را وارد کنید")]
    public string Title { get; set; }

    [Display(Name = "نوع فیلد")]
    [Required(ErrorMessage = @"{0} را وارد کنید")]
    public AttributeType AttributeType { get; set; }

    //ForeignKey

    public List<ProductAttributeValue>? AttributeValue { get; set; } = new();

    public int? AttributeGroupId { get; set; }

    [JsonIgnore] public ProductAttributeGroup? AttributeGroup { get; set; }
}

public enum AttributeType
{
    Text,
    Checkbox,
    Number
}