using System;
using UnityEngine;
using UnityEngine.Events;

namespace VV.SceneHandling
{
    public class SceneRoot : MonoBehaviour
    {
        public SceneLoadHandle Handle { get; internal set; } = null;
        public object Payload { get; internal set; } = null;

        [SerializeField] UnityEvent OnActivate = null;
        [SerializeField] UnityEvent OnDeactivate = null;

        internal void Activate()
        {
            try
            {
                OnActivate.Invoke();
            }
            catch (Exception ex)
            {
                Debug.Log(ex.ToString());
            }
            finally
            {
                gameObject.SetActive(true);
            }
        }

        void Awake()
        {
            gameObject.SetActive(false);
        }

        void OnDisable()
        {
            OnDeactivate.Invoke();
        }

        void OnValidate()
        {
            transform.position = Vector3.zero;
        }
    }
}