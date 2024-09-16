using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BringerOfDeathIsAttack : MonoBehaviour
{
    CinemachineVirtualCamera _camera;
    [SerializeField] float _waitTime = 2;
    [SerializeField] GameObject[] _spark;
    [SerializeField] GameObject _longRangeAttack;
    [SerializeField] Vector3 _instantiatePosition;
    BringerOfDeath _bringerOfDeath;
    PlayerManager _player;
    [SerializeField] AudioSource _deathAudio;
    [SerializeField] AudioSource _bossArearAudio;
    [SerializeField] AudioSource _attackAudio;
    // Start is called before the first frame update
    void Start()
    {
        _bringerOfDeath = FindObjectOfType<BringerOfDeath>();
        _player = FindObjectOfType<PlayerManager>();
        _camera = GameObject.Find("Boss Camera").GetComponent<CinemachineVirtualCamera>();
    }
    void LongRangeAttack()
    {
        Instantiate(_longRangeAttack, _player.transform.position + _instantiatePosition, Quaternion.identity);
    }
    void IsLongRangeAttacking()
    {
        _bringerOfDeath.IsLongRangeAttacking = false;
    }
    void IsAttackingTrue()
    {
        _bringerOfDeath.IsAttacking = true;
    }
    void IsAttackTrue()
    {
        _bringerOfDeath.IsAttack = true;
    }
    void AttackAudio()
    {
        _attackAudio.Play();
    }
    void IsAttackFalse()
    {
        _bringerOfDeath.IsAttack = false;
        _bringerOfDeath.IsAttacking = false;
    }
    void IsHitFalse()
    {
        _bringerOfDeath.IsHit = false;
    }
    void Death()
    {
        var sr = GetComponent<SpriteRenderer>();
        sr.enabled = false;
    }
    void DeathAudio()
    {
        _deathAudio.Play();
        Destroy(_bringerOfDeath.gameObject, 2.3f);
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
        var gm = FindObjectOfType<GameManager>();
        gm.IsMovie = false;
        foreach (var spark in _spark)
        {
            var s = spark.GetComponent<MeshRenderer>();
            s.enabled = false;
            Destroy(spark.gameObject, 2);
        }
    }
}
