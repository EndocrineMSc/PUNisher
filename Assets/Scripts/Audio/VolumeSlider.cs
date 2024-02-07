using FMODUnity;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    [SerializeField] private VolumeType volumeType;
    private Slider _volumeSlider;
    [SerializeField] Canvas settingsCanvas;

    [SerializeField] EventReference SFXTestSound;
    [SerializeField] FMOD.Studio.EventInstance SFXTestEvent;

    void Awake() {
        _volumeSlider = GetComponent<Slider>();
        if (volumeType == VolumeType.SFX) SFXTestEvent = FMODUnity.RuntimeManager.CreateInstance(SFXTestSound);
    }

    public void SFXVolumeTest()
    {
        //AudioManager.Instance.PlayOneShot(SFXTestSound, transform.position);
        
        FMOD.Studio.PLAYBACK_STATE PbState;
        SFXTestEvent.getPlaybackState(out PbState);
        if(PbState != FMOD.Studio.PLAYBACK_STATE.PLAYING)
        {
            SFXTestEvent.start();
        }
    }

    private void Update()
    {
        if(FMODUnity.RuntimeManager.HaveAllBanksLoaded && settingsCanvas.enabled) { 
            if(_volumeSlider.value != AudioManager.Instance.GetVolume(volumeType))
            {
                AudioManager.Instance.SetVolume(volumeType, _volumeSlider.value);
            }
            //Debug.Log($"{volumeType} Slider value = {_volumeSlider.value}, {volumeType} value at AudioManager: {AudioManager.Instance.GetVolume(volumeType)}");
        }
    }
}
