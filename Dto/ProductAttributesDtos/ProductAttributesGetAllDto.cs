﻿using Ecommerce.Entities;
using System.Text.Json.Serialization;

namespace Dto.ProductAttributesDtos
{
    public class ProductAttributesGetAllDto
    {
        public string Title { get; set; }

        
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
}

