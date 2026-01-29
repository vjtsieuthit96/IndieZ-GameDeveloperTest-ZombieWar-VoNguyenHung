using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private int maxAmmo = 300;
    [SerializeField] private int reserveAmmo = 60;

    public void AddAmmo(int amount)
    {
        reserveAmmo = Mathf.Min(reserveAmmo + amount, maxAmmo);
        GameEventManager.Instance.InvokeReserveAmmoChanged(reserveAmmo);

    }

    public int GetAmmo()
    {
        return reserveAmmo;
    }

    public int UseAmmo(int amount)
    {
        int toUse = Mathf.Min(amount, reserveAmmo);
        reserveAmmo -= toUse;
        return toUse;
    }
}