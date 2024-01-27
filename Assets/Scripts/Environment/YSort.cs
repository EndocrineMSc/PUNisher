using UnityEngine;

//Attack to GameObject with SpriteRenderer, that needs to be sorted
public class YSort : MonoBehaviour
{
    SpriteRenderer _renderer;

    private void Awake() {
        _renderer = GetComponent<SpriteRenderer>();
    }

    private void Update() {
        _renderer.sortingOrder = Mathf.RoundToInt(transform.position.y);
    }
}
