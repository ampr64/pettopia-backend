using System.Linq.Expressions;
using System.Reflection;

namespace UnitTests.TestExtensions
{
    internal static class TestExtensions
    {
        public static void SetProperty<T, TProperty>(this T source, Expression<Func<T, TProperty>> propertySelector, TProperty value)
        {
            var propertyInfo = (PropertyInfo)((MemberExpression)propertySelector.Body).Member;
            propertyInfo.SetValue(source, value, null);
        }
        
        public static void SetField<T, TField>(this T source, string fieldName, TField value)
        {
            var field = typeof(T).GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic)
                ?? throw new InvalidOperationException($"{nameof(fieldName)} not found in type {typeof(T).Name}");
            field.SetValue(source, value);
        }

        public static void SetFieldByConvention<T, TProperty>(this T source, Expression<Func<T, TProperty>> propertySelector, TProperty value)
        {
            var propertyName = ((MemberExpression)propertySelector.Body).Member.Name;
            var fieldName = "_" + propertyName.ToLowerInvariant();

            var field = typeof(T).GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic)
                ?? throw new InvalidOperationException($"{nameof(fieldName)} not found in type {typeof(T).Name}");
            field.SetValue(source, value);
        }
    }
}