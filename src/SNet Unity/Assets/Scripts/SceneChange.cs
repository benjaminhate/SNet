using SNet.Core.Models;
using UnityEngine;

public class SceneChange : MonoBehaviour
{
    public void OnSceneChange()
    {
        SNetScene.ChangeScene("TestSNetEvent");
    }
}
