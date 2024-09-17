using System.Reflection;

namespace CaloryCalculation.Application.Helpers;

public static class EntityHelper
{
    public static bool UpdatePropertiesIfChanged<T>(T existingEntity, T updatedEntity)
    {
        if (existingEntity == null) throw new ArgumentNullException(nameof(existingEntity));
        if (updatedEntity == null) throw new ArgumentNullException(nameof(updatedEntity));

        bool isModified = false;
        
        var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var property in properties)
        {
            if (property.PropertyType.IsClass && property.PropertyType != typeof(string))
            {
                continue;
            }
            
            var existingValue = property.GetValue(existingEntity);
            var updatedValue = property.GetValue(updatedEntity);
            
            if (!Equals(existingValue, updatedValue))
            {
                property.SetValue(existingEntity, updatedValue);
                isModified = true;
            }
        }

        return isModified;
    }
    
    public static bool UpdatePropertiesIfChanged<T>(T existingEntity, object updateDto)
    {
        bool isModified = false;

        var entityProperties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        var dtoProperties = updateDto.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var dtoProperty in dtoProperties)
        {
            var entityProperty = entityProperties.FirstOrDefault(p => p.Name == dtoProperty.Name);

            if (entityProperty == null || !entityProperty.CanWrite)
            {
                continue;
            }

            var existingValue = entityProperty.GetValue(existingEntity);
            var updatedValue = dtoProperty.GetValue(updateDto);

            if (!Equals(existingValue, updatedValue))
            {
                entityProperty.SetValue(existingEntity, updatedValue);
                isModified = true;
            }
        }

        return isModified;
    }
}