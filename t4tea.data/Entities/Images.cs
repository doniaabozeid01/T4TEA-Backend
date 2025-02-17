﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace t4tea.data.Entities
{
    public class Images : BaseEntity
    {
        public int Id { get; set; }
        public string ImagePath { get; set; }
        public int ProductId { get; set; }
        [JsonIgnore]
        public Products Product { get; set; }
    }
}
