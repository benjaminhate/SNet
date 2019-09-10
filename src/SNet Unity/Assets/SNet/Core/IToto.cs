using System;
using UnityEngine;

namespace SNet.Core
{
    interface IServerManager
    {
        void SendData<T>(object player, T data, string eventKey);
    }

    interface IClientManager
    {
        void RegisterEvent<T>(string eventKey, Action<T> callback);

        void Spawn(Pose pose);
        void Destroy();
    }
}