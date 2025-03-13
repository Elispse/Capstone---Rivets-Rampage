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
        HandleDuplicates();

        // Handle EventSystem persistence
        if (TryGetComponent(out EventSystem eventSystem))
        {
            if (persistentEventSystem != null)
            {
                Destroy(gameObject);
                return;
            }

            persistentEventSystem = eventSystem;
            DontDestroyOnLoad(gameObject);
        }

        // Handle Canvas persistence
        if (TryGetComponent(out Canvas canvas))
        {
            if (persistentCanvas != null)
            {
                Destroy(gameObject);
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

        // Subscribe to scene loaded event for cleanup & duplicate checks
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // First, ensure this object is still valid before running HandleDuplicates
        if (this == null) return;

        HandleDuplicates(); // Run this first to avoid checking a destroyed object

        // Destroy persistent objects if the scene isn't listed in persistentScenes
        if (!IsScenePersistent(scene.name))
        {
            if (IsEventSystem()) persistentEventSystem = null;
            if (IsCanvas()) persistentCanvas = null;

            SceneManager.sceneLoaded -= OnSceneLoaded; // Unsubscribe early
            Destroy(gameObject);
            return; // Immediately exit to prevent executing further code on a destroyed object
        }
    }


    private void HandleDuplicates()
    {
        // Destroy any duplicates in the scene that lack the PersistentObject script
        GameObject[] duplicates = FindObjectsOfType<GameObject>();

        foreach (GameObject obj in duplicates)
        {
            if (obj != gameObject && obj.name == gameObject.name)
            {
                if (!obj.GetComponent<PersistentObject>())
                {
                    Destroy(obj); // Destroy duplicate objects without persistence logic
                }
            }
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
