using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BatChase : MonoBehaviour
{
    private GameObject _player;

    [SerializeField] private Transform _startingPoint;
    [SerializeField] private float _agroRange;
    [SerializeField] private float _killRange;
    [SerializeField] private float _speed;

    [SerializeField] public bool _chase = false;

    private Animator _animator;

    void Start()
    {
        _player = GameObject.FindWithTag("Player");
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {

        if (_player == null)
            return;

        if(_chase == true)
        {
            Chase();
            Flip();
        }
        else
            ReturnStartPoint();

        Agro();
        Kill();
    }

    private void Agro()
    {
        if (Vector2.Distance(transform.position, _player.transform.position) <= _agroRange)
        {
            _speed = 1f;
            _animator.SetFloat("Speed", 1);
        }
    }

    private void Kill()
    {
        if (Vector2.Distance(transform.position, _player.transform.position) <= _killRange)
        {
            _animator.SetTrigger("Kill");
        }
    }

    private void Chase()
    {
        transform.position = Vector2.MoveTowards(transform.position, _player.transform.position, _speed * Time.deltaTime);

        _animator.SetFloat("Speed", 0);
    }

    private void ReturnStartPoint()
    {
        transform.position = Vector2.MoveTowards(transform.position, _startingPoint.position, _speed * Time.deltaTime);

        Flip();
    }

    private void Flip()
    {
        if (transform.position.x > _player.transform.position.x)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if(transform.position.x < _player.transform.position.x)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }
}
