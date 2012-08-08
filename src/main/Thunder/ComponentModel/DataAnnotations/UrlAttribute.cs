using System;
using System.ComponentModel.DataAnnotations;

namespace Thunder.ComponentModel.DataAnnotations
{
    ///<summary>
    /// Url Attribute
    ///</summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class UrlAttribute : DataTypeAttribute
    {
        private readonly bool _requireProtocol = true;

        ///<summary>
        /// Initialize new instance of <see cref="UrlAttribute"/>.
        ///</summary>
        public UrlAttribute() : base(DataType.Url)
        {
        }

        /// <summary>
        /// Initialize new instance of <see cref="UrlAttribute"/>.
        /// </summary>
        /// <param name="requireProtocol">Require protocol</param>
        public UrlAttribute(bool requireProtocol) : base(DataType.Url)
        {
            _requireProtocol = requireProtocol;
        }

        /// <summary>
        /// IsValid
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override bool IsValid(object value)
        {
            if (value == null)
                return true;

            var url = value is Uri ? value.ToString() : value as string;

            return url.IsUrl(_requireProtocol);
        }
    }
}
