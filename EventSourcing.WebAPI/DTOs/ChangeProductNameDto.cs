﻿using System;

namespace EventSourcing.WebAPI.DTOs
{
    public class ChangeProductNameDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
