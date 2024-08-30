using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Rift : MonoBehaviour, IPause
{
    [SerializeField] GameObject[] _targetPos;
    [SerializeField] float _moveSpeed = 1;
    PlayerManager _player;
    bool _isPause;
    int _targetPosNumber;
    public void Pause()
    {
        _isPause = true;
    }

    public void Resume()
    {
        _isPause = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        _player = FindObjectOfType<PlayerManager>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_player.IsDeath)
        {

        }
        if (!_isPause && !_player.IsDeath)
        {
            float dis = Vector2.Distance(_targetPos[_targetPosNumber].transform.position, transform.position);
            if (dis > 0.3f)
            {
                Vector2 dir = (_targetPos[_targetPosNumber].transform.position - transform.position).normalized * _moveSpeed;
                transform.Translate(dir * Time.deltaTime);
            }
            else
            {
                _targetPosNumber = (_targetPosNumber + 1) % _targetPos.Length;
            }
        }
    }
}
