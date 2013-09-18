namespace Thunder.Data.Pattern.Repository.Domain
{
    public class User : Persist<User, int>
    {
        public virtual string Name { get; set; }
        public virtual int Age { get; set; }
        public virtual Status Status { get; set; }
    }
}
