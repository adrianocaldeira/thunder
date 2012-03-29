namespace Thunder.NHibernate.Domain
{
    public class Car
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual int Door { get; set; }
        public virtual string Color { get; set; }
        public virtual Tire Tire { get; set; }

        public override bool Equals(object obj)
        {
            var compare = obj as Car;

            if (compare == null)
            {
                return false;
            }

            return (GetHashCode() == compare.GetHashCode());
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (29*Id.GetHashCode());
            }
        }
    }
}