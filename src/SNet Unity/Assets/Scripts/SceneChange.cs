using SNet.Core;
using SNet.Core.Models;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    public void OnSceneChange()
    {
        SNetScene.ChangeScene("TestSNetEvent");
    }
}
