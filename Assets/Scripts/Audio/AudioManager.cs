using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using UnityEngine.SceneManagement;
using System.IO;

public class AudioManager : MonoBehaviour
{
    [Header("Volume")]
    public FloatVariable masterVolume;
    public FloatVariable musicVolume;
    public FloatVariable sfxVolume;

    private Bus masterBus;
    private Bus musicBus;
    private Bus sfxBus;

    private List<EventInstance> eventInstances;
    private List<StudioEventEmitter> eventEmitters;

    private EventInstance musicEventInstance;

    private string saveFilePath;

    public static AudioManager instance { get; private set; }

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "Game")
        {
            InitMusic(FMODEvents.instance.gameMusic);
        }
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            InitMusic(FMODEvents.instance.menuMusic);
        }
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject); // Destroy duplicate
            return;
        }

        instance = this;
        eventInstances = new List<EventInstance>();
        eventEmitters = new List<StudioEventEmitter>();

        masterBus = RuntimeManager.GetBus("bus:/");
        musicBus = RuntimeManager.GetBus("bus:/Music");
        sfxBus = RuntimeManager.GetBus("bus:/SFX");

        // Set the save file path
        saveFilePath = Path.Combine(Application.persistentDataPath, "volumeSettings.json");

        // Load volume settings on start
        LoadVolumeSettings();
    }

    private void Update()
    {
        masterBus.setVolume(masterVolume.value);
        musicBus.setVolume(musicVolume.value);
        sfxBus.setVolume(sfxVolume.value);
    }

    private void InitMusic(EventReference musicEventReference)
    {
        musicEventInstance = CreateInstance(musicEventReference);
        musicEventInstance.start();
    }

    public void PlayOneShot(EventReference sound, Vector3 worldPos)
    {
        RuntimeManager.PlayOneShot(sound, worldPos);
    }

    public EventInstance CreateInstance(EventReference eventReference)
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
        eventInstances.Add(eventInstance);
        return eventInstance;
    }

    public StudioEventEmitter InitEventEmitter(EventReference eventReference, GameObject emitterGameObject)
    {
        StudioEventEmitter emitter = emitterGameObject.GetComponent<StudioEventEmitter>();
        emitter.EventReference = eventReference;
        eventEmitters.Add(emitter);
        return emitter;
    }

    private void CleanUp()
    {
        // stop and release any created instances
        foreach (EventInstance eventInstance in eventInstances)
        {
            eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            eventInstance.release();
        }

        foreach (StudioEventEmitter emitter in eventEmitters)
        {
            emitter.Stop();
        }
    }

    private void OnDestroy()
    {
        CleanUp();
    }

    // Save volume settings to a JSON file
    public void SaveVolumeSettings()
    {
        VolumeSettings settings = new VolumeSettings
        {
            masterVolume = masterVolume.value,
            musicVolume = musicVolume.value,
            sfxVolume = sfxVolume.value
        };

        string json = JsonUtility.ToJson(settings, true);
        File.WriteAllText(saveFilePath, json);
        Debug.Log("Volume settings saved to: " + saveFilePath);
    }

    // Load volume settings from a JSON file
    public void LoadVolumeSettings()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            VolumeSettings settings = JsonUtility.FromJson<VolumeSettings>(json);

            masterVolume.value = settings.masterVolume;
            musicVolume.value = settings.musicVolume;
            sfxVolume.value = settings.sfxVolume;

            Debug.Log("Volume settings loaded from: " + saveFilePath);
        }
        else
        {
            Debug.Log("No saved volume settings found. Using default values.");
        }
    }
}
