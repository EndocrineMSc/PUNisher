using UnityEngine.EventSystems;
using UnityEngine;
using FMODUnity;

public class PunObject : InteractableObject, IPointerClickHandler
{
    #region Fields and Properties

    [SerializeField] private float _activationDistance = 1f;
    private RectTransform _rectTransform;
    private Transform _playerTransform;

    #endregion

    #region Methods

    private void Awake() {
        _rectTransform = GetComponent<RectTransform>();
    }

    private void Start() {
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);
        PlaySoundEffect();

        var playerPosition = Camera.main.WorldToScreenPoint(_playerTransform.position);


        var distance = Vector2.Distance(playerPosition, _rectTransform.position);

        Debug.Log(distance);
        //if (distance < _activationDistance) {
            PunManager.Instance.PunFound();
            Destroy(gameObject);
        //}
    }
    #endregion
}
