using UnityEngine;
using UnityEngine.SceneManagement;

public class PersistentObject : MonoBehaviour
{
    // Array of scene names where the GameObject should persist
    [SerializeField]
    private string[] persistentScenes;

    private void Awake()
    {
        // Make the GameObject persistent
        DontDestroyOnLoad(gameObject);

        // Subscribe to the scene loaded event
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Check if the current scene is in the list of persistent scenes
        bool shouldPersist = false;
        foreach (string sceneName in persistentScenes)
        {
            if (scene.name == sceneName)
            {
                shouldPersist = true;
                break;
            }
        }

        // Destroy the GameObject if it shouldn't persist in the current scene
        if (!shouldPersist)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded; // Unsubscribe to avoid memory leaks
            Destroy(gameObject);
        }
    }
}