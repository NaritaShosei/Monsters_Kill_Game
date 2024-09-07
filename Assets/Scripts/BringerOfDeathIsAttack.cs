using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BringerOfDeathIsAttack : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera _camera;
    [SerializeField] float _waitTime = 2;
    [SerializeField] GameObject[] _spark;
    BringerOfDeath _bringerOfDeath;
    PlayerManager _player;
    // Start is called before the first frame update
    void Start()
    {
        _bringerOfDeath = FindObjectOfType<BringerOfDeath>();
        _player = FindObjectOfType<PlayerManager>();
    }
    void IsAttackTrue()
    {
        _bringerOfDeath.IsAttack = true;
    }
    void IsAttackFalse()
    {
        _bringerOfDeath.IsAttack = false;
    }
    void IsHitFalse()
    {
        _bringerOfDeath.IsHit = false;
    }
    void Death()
    {
        Destroy(_bringerOfDeath.gameObject);
    }
    void StartMove()
    {
        _camera.Priority = 0;
        StartCoroutine(StartIsMove());
        var cameraShake = GetComponent<CinemachineImpulseSource>();
        cameraShake.GenerateImpulse();
        foreach (var spark in _spark)
        {
            spark.SetActive(true);
        }
    }
    IEnumerator StartIsMove()
    {
        yield return new WaitForSeconds(_waitTime);
        _bringerOfDeath.IsStopping = false;
        _player.IsStopping = false;
        var gm = FindObjectOfType<GameManager>();
        gm.IsMovie = false;    
        foreach (var spark in _spark)
        {
            Destroy(spark.gameObject);
        }
    }
}
