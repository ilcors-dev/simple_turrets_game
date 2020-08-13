using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TurretShop : Singleton<TurretShop>
{
    /// <summary>
    /// The turrets the player can buy
    /// </summary>
    [SerializeField]
    public ShopItem[] shopItems;

    /// <summary>
    /// The shop elements UI
    /// </summary>
    [SerializeField]
    public GameObject[] UIShopElements;

    /// <summary>
    /// Put the turret and the ui elements in a dictionary because they are
    /// a 1-1 relationship
    /// </summary>
    public List<KeyValuePair<ShopItem, GameObject>> shop;

    // Start is called before the first frame update
    void Start()
    {
        InitShop();
    }

    // Update is called once per frame
    void Update()
    {
        CanBuy();
    }

    /// <summary>
    /// Gets the turret infos and inits the shop with those
    /// </summary>
    private void InitShop()
    {
        shop = new List<KeyValuePair<ShopItem, GameObject>>();
        shop.Add(new KeyValuePair<ShopItem, GameObject>(shopItems[0], UIShopElements[0]));
        shop.Add(new KeyValuePair<ShopItem, GameObject>(shopItems[1], UIShopElements[1]));
        shop.Add(new KeyValuePair<ShopItem, GameObject>(shopItems[2], UIShopElements[2]));

        foreach (var item in shop)
        {
            // get the ui cost text
            TextMeshProUGUI cost = item.Value.gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            // get the ui image of the turret
            Image turretShopImage = item.Value.gameObject.GetComponent<Image>();

            // get the image of the turret to place in the shop
            SpriteRenderer img = item.Key.buyableTurret.GetComponent<SpriteRenderer>();

            // change the ui turret image with the turret image to place in the shop
            turretShopImage.sprite = img.sprite;

            // set the turret price
            cost.SetText(item.Key.buyableTurret.GetComponent<Turret>().price.ToString());
        }
    }

    /// <summary>
    /// Shows green text if a turret can be bought, default color otherwise
    /// </summary>
    private void CanBuy()
    {
        foreach (var item in shop)
        {
            // get the ui cost text
            TextMeshProUGUI cost = item.Value.gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            if (item.Key.buyableTurret.GetComponent<Turret>().price <= GameManager.Instance.coins)
                cost.color = new Color32(32, 231, 51, 255);// show green color
            else
                cost.color = new Color32(186, 190, 9, 255);// show green color
        }
    }

    /// <summary>
    /// Buys a turret and updates the round money.
    /// The actual placing of the turret is dealt in TileScript
    /// </summary>
    /// <param name="turret">The turret to instantiate</param>
    /// <param name="cost">How much the turret costs</param>
    public void BuyTurret(GameObject turret, int cost)
    {
        GameManager.Instance.UpdateBalance(-cost);
        GameManager.Instance.boughtTurret = turret;
    }
}
