using SNet.Core;
using SNet.Core.Models;
using UnityEngine;

public class CubeSpawner : MonoBehaviour
{
    public GameObject cube;
    public GameObject sphere;
    public float distance = 5;

    private Camera _camera;
    private bool _isServer;
    
    private void Start()
    {
        _camera = Camera.main;
        _isServer = SNetManager.IsServer;
        
        SNetScene.RegisterPrefab(sphere);
    }

    private void Update()
    {
        if (_isServer && Input.GetMouseButtonDown(0))
        {
            var pos = Input.mousePosition;
            pos.z = distance;
            SpawnCube(_camera.ScreenToWorldPoint(pos));
        }

        if (_isServer && Input.GetMouseButtonDown(1))
        {
            var pos = Input.mousePosition;
            pos.z = distance;
            SpawnSphere(_camera.ScreenToWorldPoint(pos));
        }
    }

    private void SpawnCube(Vector3 position)
    {
        SNetScene.Spawn(cube, position, Quaternion.identity);
    }
    
    private void SpawnSphere(Vector3 position)
    {
        SNetScene.Spawn(sphere, position, Quaternion.identity);
    }
}
