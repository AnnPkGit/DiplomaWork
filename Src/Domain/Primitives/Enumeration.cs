using System.Reflection;

namespace Domain.Primitives;

public abstract class Enumeration<TEnum> : IEquatable<Enumeration<TEnum>>
    where TEnum : Enumeration<TEnum>
{
    private static readonly Dictionary<int, TEnum> Enumerations  = CreateEnumeration();
    protected Enumeration(int id, string name)
    {
        Id = id;
        Name = name;
    }
    public int Id { get; protected init; }
    
    public string Name { get; protected init; }

    public static TEnum? FromValue(int id)
    {
        return Enumerations.TryGetValue(
            id,
            out TEnum? enumeration) ? 
                enumeration :
                default;
    }
    
    public static TEnum? FromValue(string name)
    {
        return Enumerations
            .Values
            .SingleOrDefault(e => e.Name == name);
    }
    public bool Equals(Enumeration<TEnum>? other)
    {
        if (other is null)
        {
            return false;
        }

        return GetType() == other.GetType() &&
               Id == other.Id;
    }

    public override bool Equals(object? obj)
    {
        return obj is Enumeration<TEnum> other &&
               Equals(other);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    public override string ToString()
    {
        return Name;
    }

    private static Dictionary<int, TEnum> CreateEnumeration()
    {
        var enumerationType = typeof(TEnum);
        var fieldsForType = enumerationType
            .GetFields(
                BindingFlags.Public |
                BindingFlags.Static |
                BindingFlags.FlattenHierarchy)
            .Where(fieldInfo =>
                enumerationType.IsAssignableFrom(fieldInfo.FieldType))
            .Select(fieldInfo =>
                (TEnum)fieldInfo.GetValue(default)!);
        return fieldsForType.ToDictionary(x => x.Id);
    }

    public static ICollection<TEnum> GetValues()
    {
        return Enumerations.Values;
    }
}