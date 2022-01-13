using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapController : MonoBehaviour
{
    [SerializeField]
    Define.CameraMode _mode = Define.CameraMode.topView;

    [SerializeField]
    Vector3 _delta = new Vector3(0.0f, 20.0f, 0.0f);

    [SerializeField]
    GameObject _player = null;

    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = _player.transform.position + _delta;
        transform.LookAt(_player.transform);
    }
}
