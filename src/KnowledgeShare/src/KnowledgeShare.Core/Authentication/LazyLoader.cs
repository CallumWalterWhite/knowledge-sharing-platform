namespace KnowledgeShare.Core.Authentication;

public class LazyLoader<T>
{
    private readonly Func<T> _valueFactory;
    private bool _isValueCreated;
    private T? _value;

    public LazyLoader(Func<T> valueFactory)
    {
        _valueFactory = valueFactory;
    }

    public T? Value
    {
        get
        {
            if (!_isValueCreated)
            {
                _value = _valueFactory();
                _isValueCreated = true;
            }
            return _value;
        }
    }
}