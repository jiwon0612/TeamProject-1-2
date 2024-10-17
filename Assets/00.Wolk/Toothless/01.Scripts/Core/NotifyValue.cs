public class NotifyValue<T>
{
    public delegate void ValueChanged(T prev, T next);

    public ValueChanged OnValueChanged;

    private T _value;

    public T Value
    {
        get => _value;

        set
        {
            T before = _value;
            _value = value;
            if ((before == null && value != null) || !before.Equals(value)) 
                OnValueChanged?.Invoke(before, value);
        }
    }
}
