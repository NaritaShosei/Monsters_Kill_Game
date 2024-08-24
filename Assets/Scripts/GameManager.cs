using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    bool _isPause;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseResume();
            Debug.Log(_isPause);
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
