using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Tests.Attributes
{
    sealed class CaseAttribute : TestCaseAttribute
    {
        public CaseAttribute()
        {
            Author = Environment.MachineName;
        }
        public CaseAttribute(params string[] categories)
            : this()
        {
            if (categories != null && categories.Length > 0)
                Category = string.Join(',', categories);
        }
    }
    sealed class TraitsAttribute : CategoryAttribute
    {
        public TraitsAttribute(params string[] traits)
            : base(string.Join(',', traits))
        {
        }
    }
}