using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public enum SpawnState { SPAWNING, WAITING };

    [System.Serializable]
    public class Spawn
    {
        public Transform enemy;
        public int count;
        public float rate;
    }

    public Spawn spawner;

    private SpawnState state = SpawnState.WAITING;

    void Start()
    {
        
    }

    void Update()
    {
        if (state != SpawnState.SPAWNING)
        {
            StartCoroutine(SpawnNow());
        }
    }

    IEnumerator SpawnNow()
    {
        state = SpawnState.SPAWNING;

        for (int i=0; i < spawner.count; i++)
        {
            SpawnEnemy(spawner.enemy);
        }

        state = SpawnState.WAITING;

        yield break;
    }

    void SpawnEnemy(Transform _enemy)
    {
        Debug.Log("Spawning enemy " + _enemy.name);
        Instantiate(_enemy, transform.position, transform.rotation);
    }
}
