using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour
{
    #region Fields and Properties
    NavMeshAgent _agent;
    SpriteRenderer _renderer;
    Animator _animator;

    Vector3 _minScale;
    Vector3 _maxScale;

    float _cameraSize;

    #endregion

    #region Methods

    void Start() {
        _agent = GetComponent<NavMeshAgent>();
        _agent.updateRotation = false;
        _agent.updateUpAxis = false;

        _renderer = GetComponentInChildren<SpriteRenderer>();
        _animator = GetComponentInChildren<Animator>();
        
        _minScale = LevelScaling.Instance.MinScale;
        _maxScale = LevelScaling.Instance.MaxScale;

        _cameraSize = Camera.main.orthographicSize;
        Debug.Log(_cameraSize);
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKey(KeyCode.Mouse0)) {
            var targetPosition = GetMouseWorldPosition();
            _agent.SetDestination(targetPosition);

            var xDistance = targetPosition.x - transform.position.x;
            _renderer.flipX = xDistance < 0;
        }

        ToggleWalkAnimation();
        LerpScale();
    } 

    Vector3 GetMouseWorldPosition() {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    void ToggleWalkAnimation() {
        _animator.SetBool("isWalking", _agent.velocity != Vector3.zero);
    }

    void LerpScale() {
        var normalizedYPosition = (transform.position.y + _cameraSize) / (2 * _cameraSize);
        var scale = Mathf.Lerp(_maxScale.x, _minScale.x, normalizedYPosition);
        transform.localScale = new Vector3(scale, scale, 1);

    }

    #endregion
}
