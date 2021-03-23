using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets._2D;

[RequireComponent(typeof(Platformer2DUserControl))]
public class Player : MonoBehaviour
{
    [System.Serializable]
    public class PlayerStats
    {
        public int maxHealth = 100;

        private int _curHealth;
        public int curHealth
        {
            get { return _curHealth; }
            set { _curHealth = Mathf.Clamp(value, 0, maxHealth); }
        }

        public float regenRate = 2;

        public void Init()
        {
            curHealth = maxHealth;
        }
    }

    [SerializeField]
    private Weapon weapon;


    public PlayerStats stats = new PlayerStats();

    [SerializeField]
    private StatusIndicator statusIndicator;


    void Start()
    {
        stats.Init();

        if (statusIndicator == null)
        {
            Debug.LogError("No status indicator referenced on Player");
        }
        else
        {
            statusIndicator.SetHealth(stats.curHealth, stats.maxHealth);
        }

        GameMaster.gm.onToggleUpgradeMenu += OnUpgradeMenuToggle;

        InvokeRepeating("RegenHealth", 1f / stats.regenRate, 1f / stats.regenRate);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Crate"))
        {
            Destroy(other.gameObject);
            weapon.ammo.curAmmo += 10;
        }
    }

    void RegenHealth()
    {
        stats.curHealth += 1;
        statusIndicator.SetHealth(stats.curHealth, stats.maxHealth);
    }

    void OnUpgradeMenuToggle(bool active)
    {
        GetComponent<Platformer2DUserControl>().enabled = !active;
        Weapon _weapon = GetComponentInChildren<Weapon>();
        if(_weapon != null)
        {
            _weapon.enabled = !active;
        }
    }

    public void DamagePlayer(int damage)
    {
        stats.curHealth -= damage;
        AudioManager.instance.Play("Hit");
        if(stats.curHealth <= 0)
        {
            GameMaster.KillPlayer(this);
        }

        statusIndicator.SetHealth(stats.curHealth, stats.maxHealth);
    }
}
