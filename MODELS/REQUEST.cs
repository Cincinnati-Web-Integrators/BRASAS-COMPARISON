using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.MODELS
{
    public class REQUEST
    {
        public string? url { get; set; }

        public override bool Equals(object? obj)
        {
            if(obj == null)
            {
                return false;
            }
            if (obj.GetType() == typeof(REQUEST))
            {
                var copy = (REQUEST)obj;
                return url == copy.url;
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return url.GetHashCode();
        }
    }
}
