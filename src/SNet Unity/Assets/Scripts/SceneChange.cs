using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    public void OnSceneChange()
    {
        SceneManager.LoadScene("TestSNetEvent", LoadSceneMode.Single);
    }
}
