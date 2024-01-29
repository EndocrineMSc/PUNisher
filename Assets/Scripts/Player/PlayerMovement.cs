using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour
{
    #region Fields and Properties

    [SerializeField] private SceneName _sceneName;

    NavMeshAgent _agent;
    SpriteRenderer _renderer;
    Transform _visuals;
    Animator _animator;

    private LevelScalingData _levelScaling;

    private InteractableObject _targetObject = null;

    #endregion

    #region Methods

    void OnEnable() {
        JamEvents.OnEnteredDialogue += StopPlayerMovement;
        JamEvents.OnDialogueFinished += ResumePlayerMovement;
        JamEvents.OnInteractableObjectClicked += SetObjectTarget;
    }

    void OnDisable() {
        JamEvents.OnEnteredDialogue -= StopPlayerMovement;
        JamEvents.OnDialogueFinished -= ResumePlayerMovement;
        JamEvents.OnInteractableObjectClicked -= SetObjectTarget;
    }

    void Start() {
        _agent = GetComponent<NavMeshAgent>();
        _agent.updateRotation = false;
        _agent.updateUpAxis = false;

        _visuals = transform.GetChild(0);
        _renderer = GetComponentInChildren<SpriteRenderer>();
        _animator = GetComponentInChildren<Animator>();
        
        _levelScaling = GetLevelScaling();
    }

    void Update() {
        if ((Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKey(KeyCode.Mouse0)) && !PlayerStopHandler.dialogueEngaged && _targetObject == null) {
            var targetPosition = GetMouseWorldPosition();
            _agent.SetDestination(targetPosition);

            var xDistance = targetPosition.x - transform.position.x;
            _renderer.flipX = xDistance < 0;
        }

        if (_targetObject != null && _agent.velocity == Vector3.zero) {
            _targetObject.TriggerObjectFeedback();
            _targetObject = null;
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
        var normalizedYPosition = (transform.position.y - _levelScaling.MinTraversableY) / (_levelScaling.MaxTraversableY - _levelScaling.MinTraversableY);
        var scale = Mathf.Lerp(_levelScaling.MaxScale.x, _levelScaling.MinScale.x, normalizedYPosition);
        _visuals.localScale = new Vector3(scale, scale, 1);
    }

    LevelScalingData GetLevelScaling() {
        return _sceneName switch
        {
            SceneName.SecondDojo => LevelScaling.Instance.SecondDojoData,
            SceneName.ThirdDojo => LevelScaling.Instance.ThirdDojoData,
            SceneName.FourthDojo => LevelScaling.Instance.FourthDojoData,
            _ => null,
        };
    }

    void StopPlayerMovement() {
        _agent.isStopped = true;
        _agent.SetDestination(transform.position);
    }

    void ResumePlayerMovement(bool _) {
        _agent.isStopped = false;
    }

    void SetObjectTarget(InteractableObject interactableObject) {
        _agent.SetDestination(interactableObject.transform.position);
        _targetObject = interactableObject;
    }

    #endregion
}
