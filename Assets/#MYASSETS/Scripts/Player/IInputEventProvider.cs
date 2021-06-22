using UniRx;
using UniRx.Triggers;

public interface IInputEventProvider
{
    IReadOnlyReactiveProperty<bool> IsGrabPinch { get; }
}
