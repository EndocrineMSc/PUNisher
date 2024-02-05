using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    [SerializeField] private VolumeType volumeType;
    private Slider _volumeSlider;
    [SerializeField] Canvas settingsCanvas;

    void Awake() {
        _volumeSlider = GetComponent<Slider>();
    }

    private void Update()
    {
        if(FMODUnity.RuntimeManager.HaveAllBanksLoaded && settingsCanvas.enabled) { 
            if(_volumeSlider.value != AudioManager.Instance.GetVolume(volumeType))
            {
                AudioManager.Instance.SetVolume(volumeType, _volumeSlider.value);
            }
            Debug.Log($"{volumeType} Slider value = {_volumeSlider.value}, {volumeType} value at AudioManager: {AudioManager.Instance.GetVolume(volumeType)}");
        }
    }
}
