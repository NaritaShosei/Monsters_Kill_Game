using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BringerOfDeathIsAttack : MonoBehaviour
{
    BringerOfDeath _bringerOfDeath;
    // Start is called before the first frame update
    void Start()
    {
        _bringerOfDeath = FindObjectOfType<BringerOfDeath>();
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
    // Update is called once per frame
    void Update()
    {

    }
}
