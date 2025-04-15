using System.ComponentModel.DataAnnotations;

namespace EventCorp.Validaciones
{
    public class PositiveDurationAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value is int duracion)
            {
                return duracion > 0;
            }
            return false;
        }
    }
}
