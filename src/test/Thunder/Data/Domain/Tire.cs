using NHibernate.Criterion;
using Thunder.Collections;

namespace Thunder.Data.Domain
{
    public class Tire : ActiveRecord<Tire, int>
    {
        public virtual string Name { get; set; }
        public virtual string Size { get; set; }

        public static bool Exist(int id, string name)
        {
            return Exist(id, Restrictions.Eq("Name", name.ToLower()).IgnoreCase());
        }
    }
}