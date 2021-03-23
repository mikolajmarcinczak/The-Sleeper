using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class HealthUI : MonoBehaviour
{
    private Text healthText;

    [SerializeField]
    private Player player;

    void Awake()
    {
        healthText = GetComponent<Text>();
    }

    void Update()
    {
        healthText.text = "HEALTH: " + player.stats.curHealth.ToString();

        if(player.stats.curHealth <= 20)
        {
            healthText.color = new Color(1f, 0.3f, 0.3f);
        }
    }
}
