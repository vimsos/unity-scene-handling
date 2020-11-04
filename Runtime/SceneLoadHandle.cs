using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace VV.SceneHandling
{
    [Serializable]
    public class SceneLoadHandle : IOperationHandle
    {
        public string Description => $"SceneLoadHandle_{sceneName}_{Id}";
        public int Id => Scene.handle;
        public string SceneName => sceneName;
        public LoadSceneMode Mode => mode;
        public float Progress => operation?.progress ?? 1f;
        public bool IsDone => operation?.isDone ?? true;
        public void OnComplete(Action action) => operation.completed += (op) => action();

        public Scene Scene { get; internal set; }
        public SceneRoot Root { get; internal set; }

        readonly string sceneName;
        readonly LoadSceneMode mode;
        readonly AsyncOperation operation;

        internal SceneLoadHandle(string sceneName, LoadSceneMode mode, AsyncOperation operation)
        {
            this.sceneName = sceneName;
            this.mode = mode;
            this.operation = operation;
        }

        public override string ToString() => Description;
    }
}