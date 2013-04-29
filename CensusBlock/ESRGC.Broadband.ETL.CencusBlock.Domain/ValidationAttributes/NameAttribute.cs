using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace ESRGC.Broadband.ETL.CensusBlock.Domain.ValidationAttributes
{
    public class NameAttribute: ValidationAttribute
    {
        public NameAttribute() {
            //Default error message unless declared on the attribute
            ErrorMessage = "[{0}] is invalid";
        }

        public override bool IsValid(object value) {
            var name = value as string;
            if (string.IsNullOrEmpty(name)) {
                ErrorMessage = "[{0}] can not be empty";
                return false;
            }
            try {
                int.Parse(name);
                float.Parse(name);
                double.Parse(name);
                ErrorMessage = "[{0}] can not be a number";
                return false;
            }
            catch {
                //any exception means name is valid, so ignore them
                //if no exception was raised name only contains number,
                //which is invalid
            }
            return true;
        }
    }
}
