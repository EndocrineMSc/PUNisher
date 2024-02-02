using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using System.Collections;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    #region Fields and Properties

    public static AudioManager Instance { get; private set; }

    readonly List<EventInstance> _eventInstances = new();
    readonly List<StudioEventEmitter> _eventEmitters = new();

    public Bus _masterBus;
    public Bus _musicBus;
    public Bus _sfxBus;

    public float currentMasterVolume;
    public float currentMusicVolume;
    public float currentSFXVolume;


    bool banksLoadedManually = false;
    bool banksLoaded = false;
    public bool bussesInitialized = false;

    #endregion

    #region Methods

    void Awake() 
    {
        if (Instance == null) 
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else 
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        banksLoaded = FMODUnity.RuntimeManager.HaveAllBanksLoaded;
        if (!banksLoaded)
        {
            LoadBanks();
            Debug.Log($"loaded: {FMODUnity.RuntimeManager.HaveAllBanksLoaded}");
        }
        else if (!bussesInitialized)
            SetBusses();

        Debug.Log($"banksLoaded {banksLoaded}, manually {banksLoadedManually}, bussesInitialized {bussesInitialized}");
    }
    void LoadBanks()
    {

        if (FMODUnity.RuntimeManager.HaveAllBanksLoaded && !banksLoadedManually)
        {
            Debug.Log("~ Debug Bot says: Banks have loaded before manually loading");
            SetBusses();
            banksLoadedManually = true;
        }
        else
        {
            Debug.Log("~ Debug Bot says: Banks have not loaded yet");
            FMODUnity.RuntimeManager.LoadBank("Master.strings");
            FMODUnity.RuntimeManager.LoadBank("Master");
            FMODUnity.RuntimeManager.LoadBank("Music");
            FMODUnity.RuntimeManager.LoadBank("SFX"); 
        }
    }

    public void SetBusses() {
        _masterBus = RuntimeManager.GetBus("bus:/");
        _musicBus = RuntimeManager.GetBus("bus:/Music");
        _sfxBus = RuntimeManager.GetBus("bus:/SFX");
        bussesInitialized = true;
            //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void SetVolume(VolumeType type, float value) {
        if (value > 1 || value < 0) {
            Debug.LogError("Volume value outside expected range (0 - 1)");
            return;
        }
         
        switch (type) {
            case VolumeType.MASTER:
                _masterBus.setVolume(value);
                currentMasterVolume = value;
                break;
            case VolumeType.MUSIC:
                _musicBus.setVolume(value);
                currentMusicVolume = value;
                break;
            case VolumeType.SFX:
                _sfxBus.setVolume(value);
                currentSFXVolume = value;
                break;
            default: 
                Debug.LogError("Volume Type is not known!");
                break;
        }
    }

    public float GetVolume(VolumeType type) {
        switch (type) {
            case VolumeType.MASTER:
                _masterBus.getVolume(out float masterVolume);
                return masterVolume;
            case VolumeType.MUSIC:
                _musicBus.getVolume(out float musicVolume);
                return musicVolume;
            case VolumeType.SFX:
                _sfxBus.getVolume(out float sfxVolume);
                return sfxVolume;
            default: 
                Debug.LogError("Volume Type is not known!");
                return -1;
        }
    }

    /// <summary>
    /// Plays a sound effect once at a given position in world space.
    /// </summary>
    /// <param name="sound">The event reference to the sound to be played</param>
    /// <param name="worldPosition">Point of origin for the sound</param>
    public void PlayOneShot(EventReference sound, Vector3 worldPosition) {
        RuntimeManager.PlayOneShot(sound, worldPosition);
    }

    /// <summary>
    /// Creates an EventInstance for looping sound effects, such as footsteps,
    /// and returns it. Also adds the event instance to an internal list for
    /// easy cleanup, for easy bulk event instance stop and release.
    /// </summary>
    /// <param name="eventReference">The event reference to the sound to be played</param>
    /// <returns>An event instance that keeps playing the sound on a loop</returns>
     EventInstance eventInstance;
    public EventInstance CreateInstance(EventReference eventReference) {
       
        if (bussesInitialized){
             eventInstance = RuntimeManager.CreateInstance(eventReference);
            _eventInstances.Add(eventInstance);
        }
        Debug.Log($"AudioManager created instance for eventReference {eventReference}");
        return eventInstance;
    }

    /// <summary>
    /// Sets the sound for an emitter in the scene, adding the emitter to the clean up list
    /// for easy bulk event emitter stopping.
    /// </summary>
    /// <param name="eventReference">The event reference to the sound to be played</param>
    /// <param name="emitterGameObject">The gameobject that has the FMOD Studio Event Emitter attached</param>
    /// <returns>The modified emitter</returns>
    public StudioEventEmitter InitializeEventEmitter(EventReference eventReference, GameObject emitterGameObject) {
        StudioEventEmitter emitter = emitterGameObject.GetComponent<StudioEventEmitter>();
        emitter.EventReference = eventReference;
        _eventEmitters.Add(emitter);
        return emitter;
    }

    /// <summary>
    /// Changes the parameter of an event instance by name
    /// </summary>
    /// <param name="eventInstance"></param>
    /// <param name="parameterName"></param>
    /// <param name="value"></param>
    public void SetInstanceParameter(EventInstance eventInstance, string parameterName, float value) {
        FMOD.RESULT result = eventInstance.setParameterByName(parameterName, value);
        if (result != FMOD.RESULT.OK)
            Debug.LogError("Failed to set parameter. Check spelling or if parameter exists. " + result.ToString());
    }

    /// <summary>
    /// Stops and releases all event instances and -emitters in the scene
    /// </summary>
    public void CleanUp(bool fadeOut) {
        foreach (var eventInstance in _eventInstances) {
            if (fadeOut)
                eventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            else
                eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);

            eventInstance.release();
        }

        foreach (var emitter in _eventEmitters) {
            emitter.Stop();  
        }

        _eventInstances.Clear();
        _eventEmitters.Clear();
    }

    void OnDestroy() {
        CleanUp(false);
    }

    #endregion
}

public enum VolumeType {
    MASTER,
    MUSIC,
    SFX
}
