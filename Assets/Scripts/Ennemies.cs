using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ennemies : MonoBehaviour
{
    private Animator _animator;
    public Player _player;
    private GameObject _attackArea = default;
    private float _attackRange = 2f;
    private float _attackAngle = 30f;
    private LayerMask _playerLayer;
    private float startPos;
    private float endPos;
    private float speed = 5f;
    private bool movingForward = true;
    private bool isPlayerInFront = false;
    private bool _walk;
    void Start()
    {
        _animator = GetComponent<Animator>();
        _attackArea = transform.GetChild(0).gameObject;
        _playerLayer = LayerMask.GetMask("Player");
        startPos = transform.position.x;
        endPos = startPos + 10f;
        _walk = true;
    }
    void FixedUpdate()
    {
        Walk(_walk);
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _attackRange, _playerLayer);

        foreach (Collider2D collider in colliders)
        {
            Vector2 directionToPlayer = collider.transform.position - transform.position;
            Vector2 forwardDirection = transform.right;
            if (Vector2.Dot(directionToPlayer.normalized, forwardDirection) > Mathf.Cos(_attackAngle * Mathf.Deg2Rad))
            {
                isPlayerInFront = true;
                break;
            }
        }

        if(colliders.Length > 0 && isPlayerInFront)
        {
            _walk = false;
            _player.Side = movingForward ? 1f : -1f;
            _animator.SetTrigger("Attack");
            Walk(_walk);
            StartCoroutine(PlayAttack());
            _attackArea.SetActive(true);
        }
        else
        {
            _attackArea.SetActive(false);
        }

        if(movingForward == true && _walk == true)
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(endPos, transform.position.y), speed * Time.deltaTime);

            if(transform.position.x >= endPos)
            {
                movingForward = false;
                Utils.Flip2D_Object(gameObject, 0f, 180f);
            }
        }
        else if(movingForward == false && _walk == true)
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(startPos, transform.position.y), speed * Time.deltaTime);
            
            if(transform.position.x <= startPos)
            {
                movingForward = true;
                Utils.Flip2D_Object(gameObject);
            }
        }
    }

    private IEnumerator PlayAttack()
    {
        _animator.SetTrigger("Attack");
        yield return new WaitForSeconds(0.5f);
        _attackArea.SetActive(false);
        _walk = true;
    }

    private void Walk(bool value)
    {
        if (value == true)
        {
            _animator.SetFloat("Speed", 0.5f);
        }
        else _animator.SetFloat("Speed", 0f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 3)
        {
            Damage.instance.DamagePlayer(1);
        }
    }
}
