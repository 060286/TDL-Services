﻿using System;
using TDL.Infrastructure.Persistence.Base;

namespace TDL.Domain.Entities
{
    public class TodoCategory : BaseEntity
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }
    }
}
