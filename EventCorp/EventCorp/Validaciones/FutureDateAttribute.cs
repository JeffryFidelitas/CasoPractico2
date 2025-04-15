using System.ComponentModel.DataAnnotations;

namespace EventCorp.Validaciones
{
    public class FutureDateAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value is DateTime dt)
            {
                return dt.Date >= DateTime.Now.Date;
            }
            return false;
        }
    }
}
