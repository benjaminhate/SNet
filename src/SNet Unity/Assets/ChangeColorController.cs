using System.Collections;
using SNet.Core;
using SNet.Core.Events;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Renderer))]
public class ChangeColorController : MonoBehaviour
{
    [SerializeField] private float secondsToChange = .5f;
    [SerializeField] private SNetColorEvent sNetColorEvent;

    private Renderer _renderer;

    private void Start()
    {
        _renderer = GetComponent<Renderer>();
        var server = sNetColorEvent.IsServer;

        if(server)
            StartCoroutine(nameof(ChangeColor));
    }

    private IEnumerator ChangeColor()
    {
        while (true)
        {
            var newColor = Random.ColorHSV();

            _renderer.material.color = newColor;
            sNetColorEvent.ServerBroadcast(newColor);
            yield return new WaitForSeconds(secondsToChange);
        }
    }

    public void ChangeColorClient(Color serverColor)
    {
        _renderer.material.color = serverColor;
    }
}
