using UnityEngine;

public class FixedJoystick : Joystick
{
    private bool isMoving;
    private void Update()
    {
        Vector2 input = new Vector2(Horizontal, Vertical);

        if (input.sqrMagnitude > 0.001f)
        {
            GameEventManager.Instance.TriggerMove(input);

            if (!isMoving)
            {
                isMoving = true;
            }
        }
        else if (isMoving)
        {
            GameEventManager.Instance.TriggerMoveRelease();
            isMoving = false;
        }
    }
}