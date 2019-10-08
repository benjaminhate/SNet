using UnityEngine;

namespace SNet.Core
{
    public class NetworkScene
    {
        public static GameObject Spawn(GameObject go, Vector3 position, Quaternion rotation)
        {
            return Object.Instantiate(go, position, rotation);
        }
    }
}