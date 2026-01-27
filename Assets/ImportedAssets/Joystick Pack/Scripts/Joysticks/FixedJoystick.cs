using UnityEngine;

public class FixedJoystick : Joystick
{
    private void Update()
    {
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

    public override void OnPointerUp(UnityEngine.EventSystems.PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
        GameEventManager.Instance.TriggerMoveRelease();
    }
}