using System;
using System.ComponentModel.DataAnnotations;
using Thunder.Extensions;

namespace Thunder.ComponentModel.DataAnnotations
{
    /// <summary>
    /// Document validator
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class DocumentAttribute : ValidationAttribute
    {
        ///<summary>
        /// Document type
        ///</summary>
        public DocumentType Type { get; set; }

        /// <summary>
        /// IsValid
        /// </summary>
        /// <param name="value">Value</param>
        /// <returns></returns>
        public override bool IsValid(object value)
        {
            try
            {
                if (value == null)
                {
                    return true;
                }

                if (!string.IsNullOrEmpty(value.ToString()))
                {
                    if(Type == DocumentType.Cpf)
                    {
                        return value.ToString().IsCpf();
                    }

                    if (Type == DocumentType.Cnpj)
                    {
                        return value.ToString().IsCnpj();
                    }
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
