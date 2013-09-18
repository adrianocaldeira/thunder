namespace Thunder.Data.Pattern.ActiveRecord.Domain
{
    public class Car : ActiveRecord<Car, int>
    {
        public virtual string Name { get; set; }
        public virtual int Year { get; set; }
        public virtual Tire Tire { get; set; }
    }
}
