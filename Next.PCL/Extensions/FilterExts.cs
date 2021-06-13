﻿using System.Collections.Generic;
using System.Linq;
using Common;
using Next.PCL.Entities;

namespace Next.PCL.Extensions
{
    public static class FilterExts
    {
        public static bool Contains<TEntity>(this IEnumerable<TEntity> query, string value)
            where TEntity : INamedEntity
        {
            if (value.IsValid())
                return query.Any(x => x.Name.EqualsOIC(value));

            return false;
        }
    }
}