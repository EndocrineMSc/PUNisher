using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour
{
    #region Fields and Properties
    NavMeshAgent _agent;
    SpriteRenderer _renderer;

    #endregion

    #region Methods

    void Start() {
        _agent = GetComponent<NavMeshAgent>();
        _agent.updateRotation = false;
        _agent.updateUpAxis = false;

        _renderer = GetComponentInChildren<SpriteRenderer>();
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKey(KeyCode.Mouse0)) {
            var targetPosition = GetMouseWorldPosition();
            _agent.SetDestination(targetPosition);

            var xDistance = targetPosition.x - transform.position.x;
            _renderer.flipX = xDistance < 0;
        }
    } 

    Vector3 GetMouseWorldPosition() {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    #endregion
}
