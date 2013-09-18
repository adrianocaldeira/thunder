namespace Thunder.Data.Pattern.ActiveRecord.Domain
{
    public class Tire : ActiveRecord<Tire, short>
    {
        public virtual string Name { get; set; }
    }
}
