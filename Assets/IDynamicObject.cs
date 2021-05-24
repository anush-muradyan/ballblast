namespace DefaultNamespace
{
    public interface IDynamicObject
    {
        T GetInterface<T>();
    }
}