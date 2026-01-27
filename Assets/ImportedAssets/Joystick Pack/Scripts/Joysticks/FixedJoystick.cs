using UnityEngine;
using UnityEngine.EventSystems;

public class FixedJoystick : Joystick
{   
    public override void OnDrag(PointerEventData eventData)
    {
        base.OnDrag(eventData);

        Vector2 input = new Vector2(Horizontal, Vertical);

        if (input.sqrMagnitude > 0.01f)
        {
            GameEventManager.Instance.TriggerMove(input);
        }
        else
        {
            GameEventManager.Instance.TriggerMoveRelease();
        }        
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
        
        GameEventManager.Instance.TriggerMoveRelease();
    }
}