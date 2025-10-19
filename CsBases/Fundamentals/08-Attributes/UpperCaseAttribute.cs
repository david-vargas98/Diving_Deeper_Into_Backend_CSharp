[AttributeUsage(AttributeTargets.Property)]
public class UpperCaseAttribute : Attribute
{
}

public static class AttributeProcessor
{
  public static void ApplyUpperCase(object obj)
  {
    var props = obj.GetType().GetProperties();

    foreach (var prop in props)
    {
      // verifies that the property has the attribute and if it's a string type
      if (prop.PropertyType == typeof(string) &&
          Attribute.IsDefined(prop, typeof(UpperCaseAttribute)))
      {
        var value = prop.GetValue(obj) as string;

        if (!string.IsNullOrWhiteSpace(value))
        {
          // sets tha value to uppercase
          prop.SetValue(obj, value.ToUpper());
        }
      }
    }
  }
}