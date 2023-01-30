﻿using Ecommerce.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Linq;

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

