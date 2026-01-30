using TMPro;
using UnityEngine;
using System.Collections;

public class PopupUI : MonoBehaviour
{
    [SerializeField] private TMP_Text messageText;

    public void SetMessage(string message)
    {
        StopAllCoroutines();
        StartCoroutine(TypeText(message));
    }

    private IEnumerator TypeText(string message)
    {
        messageText.text = "";
        foreach (char c in message)
        {
            messageText.text += c;
            yield return new WaitForSeconds(0.05f);
        }
    }

    public void ReturnPopupButton()
    {
        ObjectPoolManager.ReturnObjectToPool(this.gameObject);
    }
}