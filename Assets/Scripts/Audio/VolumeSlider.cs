using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    [SerializeField] private VolumeType volumeType;
    private Slider _volumeSlider;

    void Awake() {
        _volumeSlider = GetComponentInChildren<Slider>();
    }

    void OnEnable() {
        _volumeSlider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    void OnDisable() {
        _volumeSlider.onValueChanged.RemoveAllListeners();
    }

    void Update() {
        _volumeSlider.value = AudioManager.Instance.GetVolume(volumeType);
    }

    public void OnSliderValueChanged(float value) {
        AudioManager.Instance.SetVolume(volumeType, value);
    }
}
