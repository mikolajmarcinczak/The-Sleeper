using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class AmmoUI : MonoBehaviour
{
    private Text ammoText;

    [SerializeField]
    private Weapon weapon;

    void Awake()
    {
        ammoText = GetComponent<Text>();
    }

    void Update()
    {
        ammoText.text = "AMMO: " + weapon.ammo.curAmmo.ToString();

        if(weapon.ammo.curAmmo <= 5)
        {
            ammoText.color = new Color(1f, 0.3f, 0.3f);
        }
    }
}

