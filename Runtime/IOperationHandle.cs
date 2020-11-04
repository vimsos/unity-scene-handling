using System;

namespace VV.SceneHandling
{
    public interface IOperationHandle
    {
        string Description { get; }
        float Progress { get; }
        bool IsDone { get; }
        void OnComplete(Action action);
    }
}