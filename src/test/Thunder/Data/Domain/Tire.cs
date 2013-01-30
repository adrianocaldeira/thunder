using System.Collections.Generic;
using NHibernate.Criterion;

namespace Thunder.Data.Domain
{
    public class Tire : ActiveRecord<Tire, int>
    {
        public Tire()
        {
            Items = new List<TireItem>();
        }

        public virtual string Name { get; set; }
        public virtual string Size { get; set; }
        public virtual IList<TireItem> Items { get; set; }

        public static bool Exist(int id, string name)
        {
            return Exist(id, Restrictions.Eq("Name", name.ToLower()).IgnoreCase());
        }
    }
}