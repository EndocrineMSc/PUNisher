using UnityEngine.EventSystems;

public class PunObject : InteractableObject, IPointerClickHandler
{
    #region Methods

    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);
        //todo player feedback of some kind?
        PunManager.Instance.PunFound();
        Destroy(gameObject);
    }

    protected override void PlaySoundEffect()
    {
        //todo: connect new, individual sound effect
    }

    #endregion
}
