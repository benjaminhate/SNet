using SNet.Core.Events;
using UnityEngine;

public class SendValueController : MonoBehaviour
{
    [SerializeField] private SNetFloatEvent sNetFloatEvent;
    
    private ChangeColorController _changeColorController;
        
    private void Start()
    {
        _changeColorController = GetComponent<ChangeColorController>();
    }

    public void SendValueToServer(float value)
    {
        sNetFloatEvent.ServerBroadcast(value);
    }

    public void SendValueToClient(float value)
    {
        sNetFloatEvent.ClientSend(value);
    }

    public void ChangeFloatServer(float clientValue)
    {
        if(_changeColorController != null)
            _changeColorController.secondsToChange = clientValue;
    }
}