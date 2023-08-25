namespace NaughtyChoppersDA.Globals.Utils
{
    public static class ObjectUtils
    {
        public static bool AreAnyPropertiesNull(object obj, List<object?>? excludedProperty)
        {
            var type = obj.GetType();
            var properties = type.GetProperties();

            foreach (var property in properties)
            {
                if (excludedProperty != null && excludedProperty.Contains(property))
                {
                    continue; // Skip excluded properties
                }

                var value = property.GetValue(obj);
                if (value == null)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
