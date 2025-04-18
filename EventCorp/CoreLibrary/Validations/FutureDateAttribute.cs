using System.ComponentModel.DataAnnotations;

public class FutureDateAttribute : ValidationAttribute
{
    public override bool IsValid(object value)
    {
        if (value == null)
            return true; // Ya se valida con [Required]

        var fecha = (DateTime)value;
        return fecha.Date >= DateTime.Today;
    }
}
