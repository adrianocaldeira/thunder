using Thunder.NHibernate.Pattern;

namespace Thunder.Data.Pattern.ActiveRecord.Domain
{
    // ActiveRecord está [Obsolete]; este domínio existe apenas para exercitar a delegação em testes.
#pragma warning disable 618
    public class Tire : ActiveRecord<Tire, short>
#pragma warning restore 618
    {
        public virtual string Name { get; set; }
    }
}
