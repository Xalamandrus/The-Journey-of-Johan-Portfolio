using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WolfAI : MonoBehaviour
{
    [Header("Object")]
    [SerializeField] GameObject _wolf;
    [SerializeField] Animator _animator;

    [Header("Options")]
    [SerializeField] Transform[] _patrolPoints;
    [SerializeField] float _speed;
    [SerializeField] int _pauseTime;

    [Header("Raycast")]
    [SerializeField] LayerMask _player;
    [SerializeField] float _viewingDistance;

    private int _pointIndex;
    private float _raycastIndex;

    private float _waitTime;
    private bool _isWaiting = false;

    private bool _isAttack;
    private bool _dead;
    private int _attackTime = 1;

    void Start()
    {
        _raycastIndex = 1f;
    }

    void Update()
    {
        if (_isAttack)
        {
            _waitTime += Time.deltaTime;
            if (_waitTime >= _attackTime)
            {
                if(_dead)
                {
                    _animator.SetTrigger("Attack");
                } 
                else
                {
                    Follow();
                    _animator.SetTrigger("Run");
                }
            }
            else
            {
                _animator.SetTrigger("Trigger");
            }
        }
        else
        {
            Walk();
            RaycastHit();
        }

        RaycastBack();
    }

    private void Walk()
    {
        if (_isWaiting)
        {
            _animator.SetFloat("Speed", 0);

            _waitTime += Time.deltaTime;
            if (_waitTime >= _pauseTime)
            {
                _isWaiting = false;
                _waitTime = 0f;

                SwitchPoint();
            }
        }
        else
        {
            Vector2 move = (_patrolPoints[_pointIndex].position - transform.position).normalized;
            _wolf.transform.Translate(move * _speed * Time.deltaTime);

            _animator.SetFloat("Speed", 1);

            if (Vector2.Distance(_wolf.transform.position, _patrolPoints[_pointIndex].position) < 0.1f)
            {
                _isWaiting = true;
            }
        }
    }

    private void SwitchPoint()
    {
        Flip();

        switch (_pointIndex)
        {
            case 0:
                _raycastIndex = -1f;
                break;
            case 1:
                _raycastIndex = 1f;
                break;
        }

        _pointIndex = (_pointIndex + 1) % _patrolPoints.Length;
    }

    private void Flip()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    private void RaycastHit()
    {
        RaycastHit2D hit = Physics2D.Raycast(_wolf.transform.position, Vector2.right * new Vector2(_raycastIndex, 0f), _viewingDistance, _player);
        Debug.DrawRay(_wolf.transform.position, Vector2.right * _viewingDistance * new Vector2(_raycastIndex, 0f), Color.green);

        if (hit.collider != null)
        {
            _isAttack = true;
        }
    }

    private void RaycastBack()
    {
        RaycastHit2D back = Physics2D.Raycast(_wolf.transform.position, -Vector2.right * new Vector2(_raycastIndex, 0f), 0.6f, _player);

        if (back.collider != null)
        {
            Vector3 localScale = transform.localScale;

            if (_raycastIndex < 0)
            {
                localScale.x = 1f;
            }
            else
            {
                localScale.x = -1f;
            }

            transform.localScale = localScale;

            _isAttack = true;
        }
    }

    private void Follow()
    {
        GameObject playerObject = GameObject.FindWithTag("Player");

        if (playerObject != null)
        {
            _speed = 1f;

            Vector3 playerDirection = playerObject.transform.position - _wolf.transform.position;
            _wolf.transform.Translate(playerDirection.normalized * _speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_player == (_player | (1 << other.gameObject.layer)))
        {
            _dead = true;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(_patrolPoints[0].transform.position, 0.1f);
        Gizmos.DrawWireSphere(_patrolPoints[1].transform.position, 0.1f);
        Gizmos.DrawLine(_patrolPoints[0].transform.position, _patrolPoints[1].transform.position);
    }
}