namespace Thunder.Data.Pattern
{
    /// <summary>
    /// Property
    /// </summary>
    public class Property : Property<object>
    {
        /// <summary>
        /// Create property
        /// </summary>
        /// <param name="name">Name</param>
        /// <param name="value">Value</param>
        /// <returns></returns>
        public new static Property<object> Create(string name, object value)
        {
            return Property<object>.Create(name, value);
        }
    }

    /// <summary>
    /// Property
    /// </summary>
    public class Property<T>
    {
        /// <summary>
        /// Get or set name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Get or set value
        /// </summary>
        public T Value { get; set; }

        /// <summary>
        /// Create property
        /// </summary>
        /// <param name="name">Name</param>
        /// <param name="value">Value</param>
        /// <returns><see cref="Property{T}"/></returns>
        public static Property<T> Create(string name, T value)
        {
            return new Property<T> { Name = name, Value = value };
        }
    }
}