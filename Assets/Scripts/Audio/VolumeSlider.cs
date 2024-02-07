using FMODUnity;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    [SerializeField] private VolumeType volumeType;
    private Slider _volumeSlider;
    [SerializeField] Canvas settingsCanvas;

    [SerializeField] EventReference SFXTestSound;
     FMOD.Studio.EventInstance SFXTestEventInstance;

    void Awake() {
        _volumeSlider = GetComponent<Slider>();
    }

    #region SFX Sound on Slider value change
    bool instanciated = false;
    bool IsPlaying(FMOD.Studio.EventInstance instance)
    {
        FMOD.Studio.PLAYBACK_STATE state;
        instance.getPlaybackState(out state);
        return state != FMOD.Studio.PLAYBACK_STATE.STOPPED;
    }

    public void SFXVolumeTest()
    {
        if (settingsCanvas.enabled) 
        { 
            if (!instanciated)
            {
                SFXTestEventInstance = AudioManager.Instance.CreateInstance(SFXTestSound);
                instanciated = true;
            }
            if (!IsPlaying(SFXTestEventInstance))
            {
                SFXTestEventInstance.start();
            }

            //FMOD.Studio.PLAYBACK_STATE PbState;
            //SFXTestEvent.getPlaybackState(out PbState);
            //if(PbState != FMOD.Studio.PLAYBACK_STATE.PLAYING)
            //{
            //    SFXTestEventInstance.start();
            //}
        }
    }
    #endregion

    private void Update()
    {
        if(FMODUnity.RuntimeManager.HaveAllBanksLoaded && settingsCanvas.enabled) { 
            if(_volumeSlider.value != AudioManager.Instance.GetVolume(volumeType))
            {
                AudioManager.Instance.SetVolume(volumeType, _volumeSlider.value);
                if (volumeType == VolumeType.SFX)
                    SFXVolumeTest();
            }
            //Debug.Log($"{volumeType} Slider value = {_volumeSlider.value}, {volumeType} value at AudioManager: {AudioManager.Instance.GetVolume(volumeType)}");
        }
    }
}
