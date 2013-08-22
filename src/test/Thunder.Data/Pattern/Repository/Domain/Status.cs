namespace Thunder.Data.Pattern.Repository.Domain
{
    public class Status : Persist<Status, short>
    {
        public virtual string Name { get; set; }
    }
}
