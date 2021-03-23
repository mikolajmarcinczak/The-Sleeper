using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    public static GameMaster gm;
    public int enemyCount;

    [SerializeField]
    private int startingUpgradePoints;
    public static int UpgradePoints;

    void Awake()
    {
        if (gm == null)
        {
            gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
        }
    }

    public Transform playerPrefab;
    public Transform spawnPoint;
    public int spawnDelay = 2;

    public CameraShake cameraShake;

    [SerializeField]
    private GameObject gameOverUI;

    [SerializeField]
    private GameObject gameWonUI;

    [SerializeField]
    private GameObject upgradeMenu;

    public delegate void UpgradeMenuCallback(bool active);
    public UpgradeMenuCallback onToggleUpgradeMenu;


    void Start()
    {
        enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
        if (cameraShake == null) 
        {
            Debug.LogError("No camera shake referenced.");
        }

        UpgradePoints = startingUpgradePoints;
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.U))
        {
            ToggleUpgradeMenu();
        }
        
        if(enemyCount == 0)
        {
            gm.EndWin();
        }
    }

    private void ToggleUpgradeMenu()
    {
        upgradeMenu.SetActive(!upgradeMenu.activeSelf);
        onToggleUpgradeMenu.Invoke(upgradeMenu.activeSelf);
    }

    public void EndGame()
    {
        Debug.Log("GAME OVER");
        gameOverUI.SetActive(true);
    }

    public void EndWin()
    {
        Debug.Log("Game Ended.");
        gameWonUI.SetActive(true);
    }

    public static void KillPlayer(Player player)
    {
        Destroy(player.gameObject);
        if(player.stats.curHealth <= 0)
        {
            gm.EndGame();
        }
    }

    public static void KillEnemy (Enemy enemy)
    {
        gm._KillEnemy(enemy);
    }

    public void _KillEnemy(Enemy _enemy)
    {
        UpgradePoints += _enemy.upgradeDrop;
        AudioManager.instance.Play("Points");

        GameObject _part = (GameObject)Instantiate(_enemy.deathParticles.gameObject, _enemy.transform.position, Quaternion.identity);
        Destroy(_part.gameObject, 5f);
        cameraShake.Shake(_enemy.shakeAmount, _enemy.shakeLength);
        Destroy(_enemy.gameObject);

        enemyCount -= 1;
    }
}
