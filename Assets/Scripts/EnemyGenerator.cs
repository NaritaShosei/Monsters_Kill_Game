using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    [SerializeField] GameObject[] _generateStartPosition;
    [SerializeField] GameObject[] _enemy;
    [SerializeField] GameObject[] _spawnPosition;
    int _spawnCount = 0;
    PlayerManager _player;
    EnemyGenerator _enemyGenerator;
    // Start is called before the first frame update
    void Start()
    {
        _player = FindObjectOfType<PlayerManager>();
        _enemyGenerator = GetComponent<EnemyGenerator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_player.transform.position.x >= _generateStartPosition[_spawnCount].transform.position.x)
        {
            Instantiate(_enemy[_spawnCount], _spawnPosition[_spawnCount].transform.position, Quaternion.identity);
            _spawnCount += 1;
            if (_spawnCount >=  _enemy.Length)
            {
                Destroy(_enemyGenerator);
            }
        }
    }
}
