using SNet.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    public void OnSceneChange()
    {
        SNetScene.ChangeScene("TestSNetEvent");
    }
}
