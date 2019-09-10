using System.Collections;
using SNet.Core;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Renderer))]
public class ChangeColorController : MonoBehaviour
{
    [SerializeField] private float secondsToChange = .5f;

    private Renderer _renderer;
    private SNetEntity _sNetEntity;

    private void Start()
    {
        _renderer = GetComponent<Renderer>();
        _sNetEntity = GetComponent<SNetEntity>();
        var server = _sNetEntity.IsServer;

        if(server)
            StartCoroutine(nameof(ChangeColor));
    }

    private IEnumerator ChangeColor()
    {
        while (true)
        {
            var newColor = Random.ColorHSV();

            _sNetEntity.ServerBroadcast(newColor);
            yield return new WaitForSeconds(secondsToChange);
        }
    }

    public void ChangeColorClient(Color serverColor)
    {
        _renderer.material.color = serverColor;
    }
}
