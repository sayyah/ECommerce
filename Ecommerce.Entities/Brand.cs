﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Entities
{
    public class Brand : BaseEntity
    {

        [Display(Name = "نام")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = @"حداقل 3 و حداکثر 50 کاراکتر")]
        [Required(ErrorMessage = @"{0} را وارد کنید")]
        public string Name { get; set; }

        [Display(Name = "توضیحات")]
        public string? Description { get; set; }

        [Display(Name = "عکس")]
        public string? ImagePath { get; set; }

        [Display(Name = "آدرس سایت برند")]
        public string? Url { get; set; }
        //ForeignKey
        [JsonIgnore]
        public ICollection<Product>? Products { get; set; }
    }
}
