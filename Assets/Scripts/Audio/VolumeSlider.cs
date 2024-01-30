using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    [SerializeField] private VolumeType volumeType;
    private Slider _volumeSlider;

    void Awake() {
        _volumeSlider = GetComponent<Slider>();
    }

    void OnEnable() {
        if (AudioManager.Instance != null)
            _volumeSlider.value = AudioManager.Instance.GetVolume(volumeType);

        _volumeSlider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    void Start() {
        _volumeSlider.value = AudioManager.Instance.GetVolume(volumeType);
    }

    void OnDisable() {
        _volumeSlider.onValueChanged.RemoveAllListeners();
    }

    public void OnSliderValueChanged(float value) {
        AudioManager.Instance.SetVolume(volumeType, value);
    }
}
