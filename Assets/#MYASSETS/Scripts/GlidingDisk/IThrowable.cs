using Valve.VR.InteractionSystem;

public interface IThrowable
{
    void OnHandHoverBegin(Hand hand);
    void HandHoverUpdate(Hand hand);
    void OnAttachedToHand(Hand hand);
    void OnDetachedFromHand(Hand hand);
    void HandAttachedUpdate(Hand hand);
}
