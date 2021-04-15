using UnityEngine;

namespace VV.SceneHandling
{
    public class SceneRoot : MonoBehaviour
    {
        public SceneLoadHandle Handle { get; internal set; } = null;
        public object Payload { get; internal set; } = null;

        void OnValidate()
        {
            transform.position = Vector3.zero;
        }
    }
}