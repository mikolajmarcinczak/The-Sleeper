using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradePointsUI : MonoBehaviour
{
    private Text upgradesText;

    void Awake()
    {
        upgradesText = GetComponent<Text>();
    }

    void Update()
    {
        upgradesText.text = "UPGRADES: " + GameMaster.UpgradePoints.ToString();
    }
}
