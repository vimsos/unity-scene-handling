using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using System.Collections.Generic;

namespace VV.SceneHandling
{
    public static class SceneHandler
    {
        public static IEnumerable<SceneLoadHandle> Current => current;

        static Dictionary<SceneLoadHandle, object> cargo = new Dictionary<SceneLoadHandle, object>();
        static List<SceneLoadHandle> current = new List<SceneLoadHandle>();
        static bool firstSceneIsLoaded = false;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        static void Initialize()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            object payload = null;

            // match the scene being loaded to a previously created handle
            var handle = cargo.Keys.FirstOrDefault((h) => h.SceneName == scene.name && h.Mode == mode);

            // if the handle is null the scene was not loaded using this handler, possibly it is the first scene being loaded
            if (handle == null)
            {
                handle = new SceneLoadHandle(scene.name, mode, null);
                handle.Scene = scene;

                if (!firstSceneIsLoaded)
                {
                    firstSceneIsLoaded = true;
                }
                else
                {
                    Debug.LogWarning($"[SceneHandler] a scene named {scene.name} loaded with LoadSceneMode.{(mode == LoadSceneMode.Additive ? "Additive" : "Single")} was not found inside the dictionary");
                }
            }

            // retrieve the payload
            cargo.TryGetValue(handle, out payload);
            cargo.Remove(handle);

            // add scene to the list
            handle.Scene = scene;
            current.Add(handle);

            // insert payload into the scene's root object
            foreach (GameObject go in scene.GetRootGameObjects())
            {
                var root = go.GetComponent<SceneRoot>();

                if (root != null)
                {
                    handle.Root = root;
                    root.Handle = handle;
                    root.Payload = payload;
                    root.Activate();
                    break;
                }
            }

            SceneManager.SetActiveScene(scene);
        }

        public static SceneLoadHandle Load(string sceneName, object payload = null)
        {
            var operation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            var handle = new SceneLoadHandle(sceneName, LoadSceneMode.Additive, operation);
            cargo.Add(handle, payload);

            return handle;
        }

        public static SceneUnloadHandle Unload(SceneLoadHandle loadedHandle)
        {
            var operation = SceneManager.UnloadSceneAsync(loadedHandle.Scene);

            if (operation == null)
            {
                Debug.LogError($"cannot unload \"{loadedHandle.SceneName}\" scene as it is the only loaded scene currently. returning null handle.");
                return null;
            }

            var unloadHandle = new SceneUnloadHandle(loadedHandle.Scene, operation);
            operation.completed += (op) => { current.Remove(loadedHandle); };

            return unloadHandle;
        }
    }
}