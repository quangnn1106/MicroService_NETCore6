﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.Product
{
    public class CreateProductDto: CreateOrUpdateProductDto
    {
        public string No { get; set; } = default!;
    }
}
