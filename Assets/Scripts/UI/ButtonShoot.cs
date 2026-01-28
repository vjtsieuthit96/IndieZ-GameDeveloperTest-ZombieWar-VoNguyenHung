using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonShoot : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{   
    public void OnPointerDown(PointerEventData eventData)
    {       
        GameEventManager.Instance.TriggerShootHold();
    }

    public void OnPointerUp(PointerEventData eventData)
    {        
        GameEventManager.Instance.TriggerShootRelease();
    }    
}