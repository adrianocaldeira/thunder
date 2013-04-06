using NHibernate.Dialect;

namespace Thunder.Data.Dialect
{
    /// <summary>
    /// Mysql InnoDb dialect
    /// </summary>
    public class MySQL5InnoDBDialect : MySQL5Dialect
    {
        /// <summary>
        /// Support cascade delete
        /// </summary>
        public override bool SupportsCascadeDelete
        {
            get { return true; }
        }

        /// <summary>
        /// Table tyle string
        /// </summary>
        public override string TableTypeString
        {
            get { return " ENGINE=InnoDB"; }
        }

        /// <summary>
        /// Has self referential foreign key bug
        /// </summary>
        public override bool HasSelfReferentialForeignKeyBug
        {
            get { return true; }
        }
    }
}
