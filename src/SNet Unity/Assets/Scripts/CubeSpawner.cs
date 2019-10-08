using System;
using System.Collections;
using System.Collections.Generic;
using SNet.Core;
using UnityEngine;

public class CubeSpawner : MonoBehaviour
{
    public GameObject cube;
    public float distance = 5;

    private Camera _camera;
    private bool _isServer;
    
    private void Start()
    {
        _camera = Camera.main;
        _isServer = SNetManager.Instance.IsServerActive;
    }

    private void Update()
    {
        if (_isServer && Input.GetMouseButtonDown(0))
        {
            var pos = Input.mousePosition;
            pos.z = distance;
            SpawnCube(_camera.ScreenToWorldPoint(pos));
        }
    }

    private void SpawnCube(Vector3 position)
    {
        NetworkScene.Spawn(cube, position, Quaternion.identity);
    }
}
