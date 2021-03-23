using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeMenu : MonoBehaviour
{
    [SerializeField]
    private Text healthText;
    [SerializeField]
    private float healthMultiplier = 1.2f;

    [SerializeField]
    private Text damageText;
    [SerializeField]
    private float damageMultiplier = 1.1f;

    [SerializeField]
    private Text maxAmmoText;
    [SerializeField]
    private float maxAmmoMultiplier = 1.3f;

    [SerializeField]
    private Player player;

    [SerializeField]
    private Weapon weapon;

    void OnEnable()
    {
        UpdateValues();
    }

    void UpdateValues()
    {
        healthText.text = "MAX HEALTH: " + player.stats.maxHealth.ToString();
        damageText.text = "DAMAGE: " + weapon.Damage.ToString();
        maxAmmoText.text = "MAX AMMO: " + weapon.ammo.maxAmmo.ToString();
    }

    public void UpgradeHealth()
    {
        if (GameMaster.UpgradePoints < 1)
        {
            AudioManager.instance.Play("NoPoints");
            return;
        }

        player.stats.maxHealth = (int)(player.stats.maxHealth*healthMultiplier);

        GameMaster.UpgradePoints -= 1;
        AudioManager.instance.Play("Points");

        UpdateValues();
    }

    public void UpgradeDamage()
    {
        if (GameMaster.UpgradePoints < 1)
        {
            AudioManager.instance.Play("NoPoints");
            return;
        }

        weapon.Damage = (int)(weapon.Damage * damageMultiplier);

        GameMaster.UpgradePoints -= 1;
        AudioManager.instance.Play("Points");

        UpdateValues();
    }

    public void UpgradeAmmo()
    {
        if (GameMaster.UpgradePoints < 1)
        {
            AudioManager.instance.Play("NoPoints");
            return;
        }

        weapon.ammo.maxAmmo = (int)(weapon.ammo.maxAmmo * maxAmmoMultiplier);

        GameMaster.UpgradePoints -= 1;
        AudioManager.instance.Play("Points");

        UpdateValues();
    }
}
