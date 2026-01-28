using UnityEngine;

public class FixedJoystick : Joystick
{
    private Vector2 lastInput;

    private void Update()
    {
        Vector2 input = new Vector2(Horizontal, Vertical);

        // Chỉ gửi event khi input thay đổi
        if (input.sqrMagnitude > 0.01f)
        {
            if (input != lastInput)
            {
                GameEventManager.Instance.TriggerMove(input);
                lastInput = input;
            }
        }
        else if (lastInput != Vector2.zero)
        {
            GameEventManager.Instance.TriggerMoveRelease();
            lastInput = Vector2.zero;
        }
    }
}