﻿using System.Collections;
using SNet.Core;
using SNet.Core.Events;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class ChangeColorController : MonoBehaviour
{
    [SerializeField] private float secondsToChange = .5f;
    [SerializeField] private SNetColorEvent sNetColorEntity;
    [SerializeField] private SNetFloatEvent sNetFloatEvent;

    private Renderer _renderer;

    private void Start()
    {
        _renderer = GetComponent<Renderer>();
        
        if (SNetManager.IsServer)
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
        if (_renderer != null)
            _renderer.material.color = serverColor;

        sNetFloatEvent.ClientSend(Random.Range(0f, 2f));
    }

    public void ChangeFloatServer(float clientValue)
    {
        secondsToChange = clientValue;
    }
}
