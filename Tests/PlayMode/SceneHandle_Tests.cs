using System.Collections;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using VV.SceneHandling;

public class SceneHandler_Tests
{
    string fixture = "FixtureScene";

    [UnityTest]
    public IEnumerator Load_The_Same_Scene_Twice_And_Get_Different_Objects()
    {
        var h1 = SceneHandler.Load(fixture);
        var h2 = SceneHandler.Load(fixture);

        yield return new WaitUntil(() => h1.IsDone && h2.IsDone);

        Assert.AreEqual(h1.SceneName, h2.SceneName);
        Assert.AreNotEqual(h1.Id, h2.Id);
        Assert.AreNotSame(h1.Scene, h2.Scene);
        Assert.AreNotSame(h1.Scene.handle, h2.Scene.handle);
    }

    [UnityTest]
    public IEnumerator Load_A_Scene_And_Get_Its_Payload()
    {
        // the loaded scene must have a SceneRoot component 
        var payload = "payload";
        var handle = SceneHandler.Load(fixture, payload);

        yield return new WaitUntil(() => handle.IsDone);

        Assert.IsNotNull(handle.Scene);
        Assert.IsNotNull(handle.Root);
        Assert.AreEqual(handle.Root.Payload, payload);
        Assert.AreEqual((string)handle.Root.Payload, "payload");
    }

    [UnityTest]
    public IEnumerator Load_A_Scene_Get_Its_Payload_Then_Unload_It()
    {
        // the loaded scene must have a SceneRoot component 
        var payload = 5;
        var loadHandle = SceneHandler.Load(fixture, payload);

        yield return new WaitUntil(() => loadHandle.IsDone);

        // assert that the handle is contained within the list of currently loaded scenes
        // assert that the payload has been correctly set
        Assert.IsTrue(SceneHandler.Current.Contains(loadHandle));
        Assert.IsTrue(loadHandle.Root);
        Assert.AreEqual((int)loadHandle.Root.Payload, payload);

        // unload the scene using the handle
        var unloadHandle = SceneHandler.Unload(loadHandle);
        yield return new WaitUntil(() => unloadHandle.IsDone);

        // assert that the handle has been removed from the list of currently loaded scenes
        Assert.IsFalse(SceneHandler.Current.Contains(loadHandle));
        Assert.IsFalse(loadHandle.Root);
    }
}
