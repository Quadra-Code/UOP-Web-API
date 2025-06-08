using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UOP.Domain.Models
{
    public struct CompositeKey
    {

        public string First { get; }
        public string Second { get; }
        
        public CompositeKey(string first, string second)
        {
            First = first;
            Second = second;
        }

        public override bool Equals(object? obj)
        {
            if (obj is not CompositeKey)
            {
                return false;
            }

            var other = (CompositeKey)obj;
            return First == other.First && Second == other.Second;
        }

        public override int GetHashCode()
        {
            unchecked // Allow arithmetic overflow, just wrap around
            {
                int hash = 17;
                hash = hash * 23 + (First?.GetHashCode() ?? 0);
                hash = hash * 23 + (Second?.GetHashCode() ?? 0);
                return hash;
            }
        }

        public override string ToString()
        {
            return $"{First}-{Second}";
        }
    }
}
