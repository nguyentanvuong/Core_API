using System.ComponentModel.DataAnnotations;

public class CoreRequiredAttribute : RequiredAttribute
{
    public override string FormatErrorMessage(string name)
    {
        return string.Format("Field [{0}] is required", name);
    }
}


public class CoreMaxLengthAttribute : MaxLengthAttribute
{
    public CoreMaxLengthAttribute(int length) : base(length)
    {
        base.ErrorMessageResourceName = "MaximumLength";
    }

    public override string FormatErrorMessage(string name)
    {
        return string.Format("Maximum length of field [{0}] is {1}", name , Length);
    }
}

public class CoreMinLengthAttribute : MinLengthAttribute
{
    public CoreMinLengthAttribute(int length) : base(length)
    {
    }

    public override string FormatErrorMessage(string name)
    {
        return string.Format("Minimum length of field [{0}] is {1}", name, Length);
    }
}
