using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class PersistentObject : MonoBehaviour
{
    [SerializeField] private string[] persistentScenes;

    // Singleton references for EventSystem and Canvas
    private static EventSystem persistentEventSystem;
    private static Canvas persistentCanvas;

    private void Awake()
    {
        // Handle EventSystem persistence
        if (TryGetComponent(out EventSystem eventSystem))
        {
            // If an EventSystem already exists, destroy this one
            if (persistentEventSystem != null)
            {
                Destroy(gameObject); // Destroy the newly loaded EventSystem
                return;
            }

            persistentEventSystem = eventSystem;
            DontDestroyOnLoad(gameObject);
        }

        // Handle Canvas persistence
        if (TryGetComponent(out Canvas canvas))
        {
            // If a Canvas already exists, destroy this one
            if (persistentCanvas != null)
            {
                Destroy(gameObject); // Destroy the newly loaded Canvas
                return;
            }

            persistentCanvas = canvas;
            DontDestroyOnLoad(gameObject);
        }

        // Handle other general persistent objects
        if (!eventSystem && !canvas)
        {
            DontDestroyOnLoad(gameObject);
        }

        // Subscribe to scene loaded event to check when to destroy objects
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Destroy if the current scene is NOT in the persistent list
        if (!IsScenePersistent(scene.name))
        {
            if (IsEventSystem())
                persistentEventSystem = null;  // Clear static reference before destruction

            if (IsCanvas())
                persistentCanvas = null;  // Clear static reference before destruction

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

    private bool IsEventSystem() => GetComponent<EventSystem>() != null;
    private bool IsCanvas() => GetComponent<Canvas>() != null;
}
