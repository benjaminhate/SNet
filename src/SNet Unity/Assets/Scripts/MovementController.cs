using System.Collections;
using SNet.Core;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    [SerializeField] private Vector3 finalPosition;
    [SerializeField] private float smoothTime = 0.3f;
    [SerializeField] private float precision = 0.01f;
    [SerializeField] private float waitTime = 0.5f;

    [SerializeField] private SNetTransform sNetTransform;

    private Vector3 _initialPosition;
    private Vector3 _reachingPosition;
    private Vector3 _velocity;

    private int _direction = 1;
    
    private void Start()
    {
        _initialPosition = transform.position;
        _reachingPosition = finalPosition;
        if(SNetManager.IsServer)
            StartCoroutine(AutoMove());

        sNetTransform.OnTransformChanged += () => Debug.Log("Test");
    }

    private IEnumerator AutoMove()
    {
        while (true)
        {
            var position = transform.position;
            if (Vector3.Distance(position, _reachingPosition) > precision)
            {
                transform.position = Vector3.SmoothDamp(position, _reachingPosition, ref _velocity, smoothTime);
            }
            else
            {
                _reachingPosition = _direction == 1 ? finalPosition : _initialPosition;
                _direction *= -1;
                yield return new WaitForSeconds(waitTime);
            }
            
            yield return null;
        }
    }
}