namespace Thunder.Data.Pattern.Listener.Domain
{
    /// <summary>
    ///     Entidade de teste mapeada com <c>dynamic-update="true"</c>, usada para verificar o
    ///     comportamento do listener de timestamps em updates parciais de entidades attached.
    /// </summary>
    public class Product : Persist<Product, int>
    {
        public virtual string Name { get; set; }
    }
}
