using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Helpers
{
    public class JwtOptions
    {
        public string Secret {get; set;}
        public JwtOptions()
        {
            Secret = string.Empty;
        }
    }
}