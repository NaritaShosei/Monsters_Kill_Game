using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudGenerator : MonoBehaviour
{
    [SerializeField] GameObject[] _cloud;
    [SerializeField] float _moveSpeed;
    [SerializeField] float _waitTime;
    Rigidbody2D _rb2d;
    int _randomIndex;
    // Start is called before the first frame update
    void Start()
    {
        var cloud = _cloud[0].GetComponentsInChildren<Rigidbody2D>();
        foreach (var c in cloud)
        {
            if (c != null)
            {
                c.velocity = Vector2.left * _moveSpeed;
            }
        }
        _randomIndex = Random.Range(1, _cloud.Length);
        Instantiate(_cloud[_randomIndex], transform.position + new Vector3(0, Random.Range(0f, 4.05f), 0), Quaternion.identity);
        StartCoroutine(StartCloudMove());
    }

    // Update is called once per frame
    void Update()
    {
        _randomIndex = Random.Range(1, _cloud.Length);
        var cloud = FindObjectsOfType<Rigidbody2D>();
        foreach (var c in cloud)
        {
            if (c != null)
            {
                c.velocity = Vector2.left * _moveSpeed;
                if (c.transform.position.x <= -15)
                {
                    Destroy(c.gameObject);
                }
            }
        }
    }
    IEnumerator StartCloudMove()
    {
        yield return new WaitForSeconds(_waitTime);
        Instantiate(_cloud[_randomIndex], transform.position + new Vector3(0, Random.Range(0.3520594f, 4.05f), 0), Quaternion.identity);
        StartCoroutine(StartCloudMove());
    }
}
