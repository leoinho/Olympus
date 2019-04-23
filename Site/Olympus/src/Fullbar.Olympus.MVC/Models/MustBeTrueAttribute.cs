using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Fullbar.Olympus.MVC.Models
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class MustBeTrueAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            return value != null && value is bool && (bool)value;
        }
    }
}