namespace Thunder.NHibernate.Domain
{
    public class Tire
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }

        public override bool Equals(object obj)
        {
            var compare = obj as Tire;

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