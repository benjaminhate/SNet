using System.Collections;
using SNet.Core.Events;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class ChangeColorController : MonoBehaviour
{
    [SerializeField] private float secondsToChange = .5f;
    [SerializeField] private SNetColorEvent sNetColorEntity;

    private Renderer _renderer;

    private void Start()
    {
        _renderer = GetComponent<Renderer>();

        //sNetColorEntity.serv

        if (sNetColorEntity?.IsServer == true)
            StartCoroutine(nameof(ChangeColor));
    }

    private IEnumerator ChangeColor()
    {
        if (sNetColorEntity == null) yield return null;
        while (true)
        {
            var newColor = Random.ColorHSV();

            _renderer.material.color = newColor;
            sNetColorEntity.ServerBroadcast(newColor);
            yield return new WaitForSeconds(secondsToChange);
        }
    }

    public void ChangeColorClient(Color serverColor)
    {
        _renderer.material.color = serverColor;
    }
}
