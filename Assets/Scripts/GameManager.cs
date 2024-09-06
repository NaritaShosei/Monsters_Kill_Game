using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class GameManager : MonoBehaviour
{
    [SerializeField] Transform _bossStartPosition;
    [SerializeField] float _waitTime;
    [SerializeField] CinemachineVirtualCamera _camera;
    bool _isPause;
    bool _isStart;
    PlayerManager _player;
    BringerOfDeath _bringer;


    // Start is called before the first frame update
    void Start()
    {
        _player = FindObjectOfType<PlayerManager>();
        _bringer = FindObjectOfType<BringerOfDeath>();  
    }

    // Update is called once per frame
    void Update()
    {
        var pos = _bossStartPosition.transform.position;
        if (Input.GetKeyDown(KeyCode.Escape) && !_player.IsDeath)
        {
            PauseResume();
            Debug.Log(_isPause);
        }
        if (pos.x <= _player.transform.position.x && !_isStart)
        {
            _isStart = true;
            _camera.Priority = 100;
            var velo = _player._rb2d.velocity;
            velo.x = 0;
            _player._rb2d.velocity = velo;
            _player.IsStopping = true;
            _bringer._animator.Play("Cast-NoEffect");
        }
    }
    void PauseResume()
    {
        Debug.Log("PauseResume");
        _isPause = !_isPause;
        var obj = FindObjectsOfType<GameObject>();
       foreach (var objects in obj)
        {
            var pause = objects.GetComponent<IPause>();
            if (_isPause && pause != null)
            {
                pause.Pause();
            }
            else if (!_isPause && pause != null)
            {
                pause.Resume();
            }
        }
    }
}
