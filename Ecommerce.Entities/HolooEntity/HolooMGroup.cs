﻿using System.ComponentModel.DataAnnotations;

namespace Entities.HolooEntity
{
   public class HolooMGroup
    {
        [Key] 
        public string M_groupcode { get; set; }
        public string M_groupname { get; set; }

    }
}
