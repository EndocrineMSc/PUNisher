using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(EagleEyeMarkerVisualizer))]
[RequireComponent(typeof(PointerChanger))]
public class InteractableObject : MonoBehaviour, IPointerClickHandler
{
    public virtual void OnPointerClick(PointerEventData eventData)
    {
        PlaySoundEffect();
    }

    protected virtual void PlaySoundEffect() 
    {
        //todo: default sound effect for non-special items
    }
}
