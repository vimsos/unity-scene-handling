using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace VV.SceneHandling
{
    public class SceneUnloadHandle : IOperationHandle
    {
        public string Description => $"SceneUnloadHandle_{SceneName}_{Id}";
        public float Progress => operation.progress;
        public bool IsDone => operation.isDone;
        public void OnComplete(Action action) => operation.completed += (op) => action();

        public int Id { get; private set; }
        public string SceneName { get; private set; }

        readonly AsyncOperation operation;

        internal SceneUnloadHandle(Scene scene, AsyncOperation operation)
        {
            Id = scene.handle;
            SceneName = scene.name;
            this.operation = operation;
        }

        public override string ToString() => Description;
    }
}