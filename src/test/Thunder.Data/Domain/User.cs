using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Thunder.Data.Domain
{
    public class User : Persist<User, int>
    {
        public virtual string Name { get; set; }
        public virtual int Age { get; set; }
    }
}
