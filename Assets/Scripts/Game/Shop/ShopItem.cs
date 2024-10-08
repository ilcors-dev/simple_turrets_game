﻿using System.Collections;
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
        if(GameManager.Instance.coins >= buyableTurret.GetComponent<Turret>().price && GameManager.Instance.boughtTurret == null)
            TurretShop.Instance.BuyTurret(buyableTurret, buyableTurret.GetComponent<Turret>().price);
    }
}
