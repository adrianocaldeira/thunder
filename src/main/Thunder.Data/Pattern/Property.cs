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

        /// <summary>
        /// Set property value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="entity">Entity</param>
        /// <param name="name">Name</param>
        /// <param name="value">Value</param>
        public static void SetValue<T, TProperty>(T entity, string name, TProperty value)
        {
            SetValue(entity, Property<TProperty>.Create(name, value));
        }

        /// <summary>
        /// Set property value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="entity">Entity</param>
        /// <param name="property"><see cref="Property{T}"/></param>
        public static void SetValue<T, TProperty>(T entity, Property<TProperty> property)
        {
            entity.GetType().GetProperty(property.Name).SetValue(entity, property.Value, null);
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