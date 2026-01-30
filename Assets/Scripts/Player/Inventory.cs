using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private int maxAmmo = 300;
    [SerializeField] private int reserveAmmo = 60;

    [SerializeField] private int maxGrenade = 5;
    [SerializeField] private int reserveGrenade = 2;

    // ----- Ammo -----
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

    public void SetAmmo(int amount)
    {
        reserveAmmo = Mathf.Min(amount, maxAmmo);
        GameEventManager.Instance.InvokeReserveAmmoChanged(reserveAmmo);
    }

    // ----- Grenade -----
    public void AddGrenade(int amount)
    {
        reserveGrenade = Mathf.Min(reserveGrenade + amount, maxGrenade);
        GameEventManager.Instance.InvokeGrenadeChanged(reserveGrenade);
    }

    public int GetGrenade()
    {
        return reserveGrenade;
    }

    public bool UseGrenade()
    {
        if (reserveGrenade > 0)
        {
            reserveGrenade--;
            GameEventManager.Instance.InvokeGrenadeChanged(reserveGrenade);
            return true;
        }
        return false;
    }

    public void SetGrenade(int amount)
    {
        reserveGrenade = Mathf.Min(amount, maxGrenade);
        GameEventManager.Instance.InvokeGrenadeChanged(reserveGrenade);
    }
}