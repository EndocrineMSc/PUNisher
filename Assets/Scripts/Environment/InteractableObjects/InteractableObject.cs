using FMODUnity;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(EagleEyeMarkerVisualizer))]
[RequireComponent(typeof(PointerChanger))]
public class InteractableObject : MonoBehaviour, IPointerClickHandler
{
    #region Fields and Properties

    [SerializeField] EventReference _clickSound;

    #endregion

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        
        JamEvents.TriggerInteractableObjectClicked(this);
    }

    protected virtual void PlaySoundEffect() 
    {
        AudioManager.Instance.PlayOneShot(_clickSound, transform.position);
    }

    public virtual void TriggerObjectFeedback() {
        PlaySoundEffect();
    }
}
