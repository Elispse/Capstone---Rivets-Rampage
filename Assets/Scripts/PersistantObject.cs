using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PersistentObject : MonoBehaviour
{
    // Array of scene names where the GameObject should persist
    [SerializeField]
    private string[] persistentScenes;

    private static EventSystem persistentEventSystem;
    private static Canvas persistentCanvas;
    private void Awake()
    {
        // Check if this object is an EventSystem
        if (TryGetComponent(out EventSystem eventSystem))
        {
            // Handle EventSystem persistence
            if (persistentEventSystem != null && persistentEventSystem != eventSystem)
            {
                Destroy(gameObject); // Destroy duplicate EventSystem
                return;
            }

            persistentEventSystem = eventSystem;
            DontDestroyOnLoad(gameObject);
        }
        else if (TryGetComponent(out Canvas canvas))
        {
            if (persistentCanvas != null && persistentCanvas != canvas)
            {
                Destroy(gameObject); // Destroy duplicate Canvas
                return;
            }

            persistentCanvas = canvas;
            DontDestroyOnLoad(gameObject);

            canvas.sortingOrder = 0;
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (!IsEventSystem() && !IsCanvas() && !IsScenePersistent(scene.name))
        {
            SceneManager.sceneLoaded -= OnSceneLoaded; // Unsubscribe to avoid memory leaks
            Destroy(gameObject);
        }
    }

    private bool IsScenePersistent(string sceneName)
    {
        foreach (string persistentScene in persistentScenes)
        {
            if (sceneName == persistentScene)
                return true;
        }
        return false;
    }

    private bool IsEventSystem()
    {
        return GetComponent<EventSystem>() != null;
    }

    private bool IsCanvas()
    {
        return GetComponent<EventSystem>() != null;
    }
}