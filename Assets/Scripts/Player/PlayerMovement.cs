using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour
{
    #region Fields and Properties
    NavMeshAgent _agent;
    SpriteRenderer _renderer;
    Transform _visuals;
    Animator _animator;

    Vector3 _minScale;
    Vector3 _maxScale;

    float _minTraversableY;
    float _maxTraversableY;

    #endregion

    #region Methods

    void Start() {
        _agent = GetComponent<NavMeshAgent>();
        _agent.updateRotation = false;
        _agent.updateUpAxis = false;

        _visuals = transform.GetChild(0);
        _renderer = GetComponentInChildren<SpriteRenderer>();
        _animator = GetComponentInChildren<Animator>();
        
        _minScale = LevelScaling.Instance.MinScale;
        _maxScale = LevelScaling.Instance.MaxScale;
        _minTraversableY = LevelScaling.Instance.MinTraversableY;
        _maxTraversableY = LevelScaling.Instance.MaxTraversableY;
    }

    void Update() {
        if ((Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKey(KeyCode.Mouse0)) && !PlayerStopHandler.dialogueEngaged) {
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
        var normalizedYPosition = (transform.position.y - _minTraversableY) / (_maxTraversableY - _minTraversableY);
        var scale = Mathf.Lerp(_maxScale.x, _minScale.x, normalizedYPosition);
        _visuals.localScale = new Vector3(scale, scale, 1);
    }

    #endregion
}
