using FMODUnity;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(EagleEyeMarkerVisualizer))]
[RequireComponent(typeof(PointerChanger))]
public class InteractableObject : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] EventReference _clickSound;

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        PlaySoundEffect();
    }

    protected virtual void PlaySoundEffect() 
    {
        AudioManager.Instance.PlayOneShot(_clickSound, transform.position);
        Debug.Log("Click");
    }
}
