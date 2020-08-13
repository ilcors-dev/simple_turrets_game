using TMPro;
using UnityEngine;

public abstract class Turret : MonoBehaviour
{
    [Header("Turret specs")]
    /// <summary>
    /// turret damage
    /// </summary>
    [SerializeField]
    protected int damage = 1;

    /// <summary>
    /// firerate of the turret in seconds
    /// </summary>
    [SerializeField]
    [Tooltip("Fire every n seconds")]
    protected float fireRate;

    /// <summary>
    /// Counter backwards, when 0 is reached then the turret will fire if a target is locked
    /// </summary>
    protected float fireCountdown = 0f;

    /// <summary>
    /// range of the turret
    /// </summary>
    [SerializeField]
    protected float fireRange;

    /// <summary>
    /// the range of the turret
    /// </summary>
    protected SpriteRenderer range;

    /// <summary>
    /// The turret chance to make a critical hit
    /// </summary>
    [SerializeField]
    [Tooltip("Critical chance (%)")]
    protected float critChance;

    /// <summary>
    /// How much will the turret damage be increased on a critical hit
    /// </summary>
    [SerializeField]
    [Tooltip("The increased damage of a critical hit (%)")]
    protected float critIncrease;

    /// <summary>
    /// The turret price
    /// </summary>
    public int price;

    /// <summary>
    /// How many enemies the turret killed
    /// </summary>
    public int totalKilled { get; set; }

    ///<summary>
    ///Turret current level
    ///</summary>
    protected int turretLevel = 1;

    /// <summary>
    /// The percentage that will determine the worth of the turret on selling
    /// </summary>
    [SerializeField]
    [Tooltip("The percentage that will determine the worth of the turret on selling (%). The higher the less value the turret will have")]
    protected float sellWorth;

    [Header("Turret upgrade specs increment")]
    [SerializeField]
    [Tooltip("Damage increase on upgrade (%)")]
    ///<summary>Damage increase on upgrade</summary>
    protected int damageUpgradeIncrease;

    [SerializeField]
    [Tooltip("Firerate increase on upgrade (%)")]
    ///<summary>Firerate increase on upgrade</summary>
    protected int fireRateUpgradeIncrease;

    [SerializeField]
    [Tooltip("Range increase on upgrade (%)")]
    ///<summary>Range increase on upgrade</summary>
    protected float rangeUpgradeIncrease;

    [SerializeField]
    [Tooltip("Critical chance increase on upgrade (%)")]
    ///<summary>Critical chance increase on upgrade</summary>
    protected float critChanceUpgradeIncrease;

    [SerializeField]
    [Tooltip("Critical damage increase on upgrade (%)")]
    ///<summary>Critical damage increase on upgrade</summary>
    protected float critDamageUpgradeIncrease;

    [Header("The upgrade of the turret economy")]
    [SerializeField]
    [Tooltip("The upgrade cost")]
    /// <summary>
    /// How much each upgrade will cost
    /// </summary>
    protected int upgradeCost;

    [SerializeField]
    [Tooltip("How much the upgrade cost of the turret will increase by (%)")]
    /// <summary>
    /// How much the upgrade cost of the turret will increase
    /// </summary>
    protected int upgradeCostIncrease;


    /// <summary>
    /// The turret locked target
    /// </summary>
    protected Transform lockedTarget { get; set; }

    [Header("Turret prefabs")]
    /// <summary>
    /// The turret info UI components
    /// </summary>
    [SerializeField]
    protected GameObject turretInfosUI;

    /// <summary>
    /// Turret shoot audio
    /// </summary>
    protected AudioSource audio;


    public bool isSilenced;

    /// <summary>
    /// Acquires the nearest enemy and locks it.
    /// </summary>
    protected void AcquireTarget()
    {
        // get all the enemies in the field
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        // temporary nearest enemy
        GameObject nearestEnemy = null;

        // shortest distance to enemy
        float shortestDistance = Mathf.Infinity;

        // the turret position
        Vector3 currentPos = transform.position;

        foreach (GameObject enemy in enemies)
        {
            // this transform distance to enemy
            float distanceToEnemy = Vector3.Distance(enemy.transform.position, currentPos);

            // if the calculated distance is less than the one calculated before
            // it means that the current enemy is the nearest one
            if (distanceToEnemy < shortestDistance)
            {
                nearestEnemy = enemy;
                shortestDistance = distanceToEnemy;
            }
        }
        // this prevents the script from locking a new target when the old one is still in range
        if (lockedTarget != null && Vector3.Distance(lockedTarget.transform.position, currentPos) < fireRange)
            return;
        
        // if the nearest enemy is not null and its distance is in the turret range then we can lock the target
        if (nearestEnemy != null && shortestDistance <= fireRange)
            lockedTarget = nearestEnemy.transform;
        else lockedTarget = null;// nearest enemy is not in range or no enemies at all found
    }

    protected void OnMouseDown()
    {
        EnableDisableInfos();
        EnableDisableRange();
    }

    /// <summary>
    /// Shows / Deletes turret infos UI
    /// </summary>
    protected void EnableDisableInfos()
    {
        // if another turret infos are shown, close them to let this turret infos open
        if (GameManager.Instance.shownTurretInfos != null && !turretInfosUI.Equals(GameManager.Instance.shownTurretInfos))
            GameManager.Instance.shownTurretInfos.SetActive(false);

        // if the infos are active close them
        if (turretInfosUI.activeSelf)
        {
            turretInfosUI.SetActive(false);
            GameManager.Instance.shownTurretInfos = null;
        }
        else// otherwise show them
        {
            turretInfosUI.SetActive(true);
            // show actual gui
            ShowTurretInfos();

            GameManager.Instance.shownTurretInfos = turretInfosUI;
        }
    }

    /// <summary>
    /// Updates turret infos texts
    /// </summary>
    public void UpdateInfos()
    {
        GameObject infos = turretInfosUI.transform.GetChild(0).gameObject;
        // turret lvl
        infos.transform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText("Turret infos (LVL: " + turretLevel.ToString() + ")");
        // damage
        infos.transform.GetChild(1).GetComponent<TextMeshProUGUI>().SetText("Damage: " + damage.ToString());
        // firerate
        infos.transform.GetChild(2).GetComponent<TextMeshProUGUI>().SetText("Firerate: " + fireRate.ToString("#.#"));
        // firerange
        infos.transform.GetChild(3).GetComponent<TextMeshProUGUI>().SetText("Range: " + fireRange.ToString("#.#"));
        // total enemies killed
        infos.transform.GetChild(4).GetComponent<TextMeshProUGUI>().SetText("Total killed: " + totalKilled.ToString("#.#"));
        // critical chance
        infos.transform.GetChild(5).GetComponent<TextMeshProUGUI>().SetText("Crit chance: " + critChance.ToString("#.#") + "%");
        // critical hit damage increase
        infos.transform.GetChild(6).GetComponent<TextMeshProUGUI>().SetText("Critical damage increase: " + critIncrease.ToString("#.#") + "%");

        // upgrade button text
        TextMeshProUGUI upgradeText = infos.transform.GetChild(7).GetChild(0).GetComponent<TextMeshProUGUI>();

        // not enough money to buy the upgrade
        if (GameManager.Instance.coins < upgradeCost)
        {
            upgradeText.color = new Color32(201, 201, 201, 255);// show red color
        }else// can buy
            upgradeText.color = new Color32(32, 231, 51, 255);// show green color

        // upgrade price
        upgradeText.SetText("Upgrade (" + upgradeCost + ")");

        // upgrade button text
        TextMeshProUGUI sellText = infos.transform.GetChild(8).GetChild(0).GetComponent<TextMeshProUGUI>();

        // upgrade price
        sellText.SetText("Sell (" + CalculateSellValue() + ")");
    }

    /// <summary>
    /// Enables/disables the turret range indicator
    /// </summary>
    protected void EnableDisableRange()
    {
        // show just one range indicator
        if (GameManager.Instance.shownRange != null && !range.Equals(GameManager.Instance.shownRange))
            GameManager.Instance.shownRange.enabled = false;

        if (range.enabled)
            range.enabled = false;
        else
            range.enabled = true;

        // assign new range indicator to this that got set now
        GameManager.Instance.shownRange = range;
    }

    /// <summary>
    /// Determines if the damage to deal will be a critical one based on the turret crit chance.
    /// </summary>
    /// <returns>True if the damage will be critical, False otherwise</returns>
    protected bool IsCritDamage()
    {
        float randValue = Random.value;// returns number from 0f to 1f
        if (randValue < critChance / 100)// crit chance 5 means cause of the return from random above 0.05
            return true;

        return false;
    }

    /// <summary>
    /// Rotates the turret towards the locked target.
    /// </summary>
    protected void RotateTorwardsTarget()
    {
        ////find the vector pointing from our position to the target
        Vector3 targetDirection = lockedTarget.transform.position - transform.position;

        //// Draw a ray pointing at our target in
        Debug.DrawRay(transform.position, targetDirection, Color.red);

        var offset = 90f;
        Vector2 direction = (Vector2)lockedTarget.transform.position - (Vector2)transform.position;
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(Vector3.forward * (angle - offset));
    }

    /// <summary>
    /// Draws the range of the turret
    /// </summary>
    protected void DrawRange()
    {
        // dunno why, but the actual radius of the turret seems to be the 2/3 of the actual set radius
        // the turret will also detect an enemy in this range and not in the actual set one..
        float radius = (fireRange * 10 * 2) / 3;
        range.transform.localScale = new Vector3(radius, radius, radius);
    }

    /// <summary>
    /// Shows the range of the turret
    /// </summary>
    protected void OnDrawGizmos()
    {
        // dunno why, but the actual radius of the turret seems to be the 2/3 of the actual set radius
        // the turret will also detect an enemy in this range and not in the actual set one..
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, fireRange * 2 / 3);
    }

    /// <summary>
    /// Upgrades the turret based on the increments defined
    /// </summary>
    public void UpgradeTurret()
    {
        if (GameManager.Instance.coins >= upgradeCost)
        {
            damage += IncreaseByPercentageInt(damage, damageUpgradeIncrease);

            fireRange += IncreaseByPercentageFloat(fireRange, rangeUpgradeIncrease);
            fireRate += IncreaseByPercentageFloat(fireRate, fireRateUpgradeIncrease);

            if (critChance < 100)
                critChance += IncreaseByPercentageFloat(critChance, critChanceUpgradeIncrease);

            if (critChance >= 100)
                critChance = 100;// max limit reached

            if (critIncrease < 100)
                critIncrease += IncreaseByPercentageFloat(critIncrease, critDamageUpgradeIncrease);

            if (critIncrease >= 100)
                critIncrease = 100;// max limi reached

            turretLevel += 1;

            // coin spent on upgrade
            GameManager.Instance.UpdateBalance(-upgradeCost);

            // increase the turret upgrade cost
            upgradeCost += IncreaseByPercentageInt(upgradeCost, upgradeCostIncrease);

            // update turret infos
            ShowTurretInfos();
        }
    }

    /// <summary>
    /// Sells the turret giving back the player a part of the price of the turret
    /// </summary>
    public void SellTurret()
    {
        GameManager.Instance.UpdateBalance(CalculateSellValue());
        Destroy(gameObject);
    }

    /// <summary>
    /// Calculates how much the turret is worth in coins
    /// </summary>
    /// <returns></returns>
    protected int CalculateSellValue()
    {
        return (price - (int)(price * ((sellWorth / 100f) - (turretLevel / 10f))));
    }

    /// <summary>
    /// Shows what will change on the next turret upgrade
    /// </summary>
    public void UpdateUpgradePreviewInfos()
    {
        GameObject upgradeInfos = turretInfosUI.transform.GetChild(1).gameObject;
        // damage
        upgradeInfos.transform.GetChild(0).GetComponent<TextMeshProUGUI>().SetText("+" + IncreaseByPercentageInt(damage, damageUpgradeIncrease).ToString());
        // fire rate
        upgradeInfos.transform.GetChild(1).GetComponent<TextMeshProUGUI>().SetText("+" + IncreaseByPercentageFloat(fireRate, fireRateUpgradeIncrease).ToString("#.#"));
        // range
        upgradeInfos.transform.GetChild(2).GetComponent<TextMeshProUGUI>().SetText("+" + IncreaseByPercentageFloat(fireRange, rangeUpgradeIncrease).ToString("#.#"));
        // critical chance
        if (critChance < 100)
            upgradeInfos.transform.GetChild(3).GetComponent<TextMeshProUGUI>().SetText("+" + IncreaseByPercentageFloat(critChance, critChanceUpgradeIncrease).ToString("#.#") + "%");
        else
            upgradeInfos.transform.GetChild(3).GetComponent<TextMeshProUGUI>().SetText(string.Empty);
        // critical damage increase
        if (critIncrease < 100)
            upgradeInfos.transform.GetChild(4).GetComponent<TextMeshProUGUI>().SetText("+" + IncreaseByPercentageFloat(critIncrease, critDamageUpgradeIncrease).ToString("#.#") + "%");
        else
            upgradeInfos.transform.GetChild(4).GetComponent<TextMeshProUGUI>().SetText(string.Empty);
    }

    /// <summary>
    /// Calculates the percentage increment given a base and a percentage increase and returns the result as NEXT INTEGER
    /// </summary>
    /// <param name="nBase"></param>
    /// <param name="percentageIncrease"></param>
    /// <returns>Rounds to next INTEGER</returns>
    protected int IncreaseByPercentageInt(int nBase, float percentageIncrease)
    {
        return Mathf.CeilToInt(nBase * (percentageIncrease / 100));
    }

    /// <summary>
    /// Calculates the percentage increment given a base and a percentage increase and returns the result as FLOAT
    /// </summary>
    /// <param name="nBase"></param>
    /// <param name="percentageIncrease"></param>
    /// <returns>Rounds to next FLOAT</returns>
    protected float IncreaseByPercentageFloat(float nBase, float percentageIncrease)
    {
        return nBase * (percentageIncrease / 100);
    }

    /// <summary>
    /// Shows turret infos
    /// </summary>
    protected void ShowTurretInfos()
    {
        // update the turret infos
        UpdateInfos();
        UpdateUpgradePreviewInfos();

        // show turret range
        DrawRange();
    }

    /// <summary>
    /// Every turret must have a shoot method
    /// </summary>
    public abstract void Shoot(Transform enemy);
}
