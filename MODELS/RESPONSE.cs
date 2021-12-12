using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.MODELS
{
    public class RESPONSE
    {
        public string? url { get; set; }
        public CONTENT content { get; set; }

        public override bool Equals(object? obj)
        {
            if(obj == null)
            {
                return false;
            }
            if (obj.GetType() == typeof(RESPONSE))
            {
                var copy = (RESPONSE)obj;
                return url == copy.url && content == copy.content;
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return url.GetHashCode() ^ content.GetHashCode();
        }
    }
}
