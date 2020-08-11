using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopItem : MonoBehaviour
{
    [SerializeField]
    public GameObject buyableTurret;

    /// <summary>
    /// Calls the shop method BuyTurret with the shop item parameters
    /// </summary>
    public void Buy()
    {
        TurretShop.Instance.BuyTurret(buyableTurret, buyableTurret.GetComponent<Turret>().price);
    }
}
