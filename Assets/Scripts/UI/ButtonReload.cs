using UnityEngine;

public class ButtonReload : MonoBehaviour
{
    public void OnReloadClick()
    {
        GameEventManager.Instance.TriggerReloadClicked();
    }
}
