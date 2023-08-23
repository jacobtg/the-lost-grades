using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum SpiderAnimationState
{
    Idle,
    Walking,
    Attacking,
    Death
}

public class SpiderController : MonoBehaviour
{
    private NavMeshAgent _agent;
    private Animator _animator;
    private AudioSource _audioSource;

    private bool _isDead;
    private float _attackDist;

    private Vector3 _idlePosition;

    private SpiderAnimationState _animState;
    private string _animToPlay = "Idle"; 
    
    [SerializeField] private float health;
    [SerializeField] private float damage = 30.0f;

    [SerializeField] private float targetPlayerRange = 20.0f;

    [SerializeField] private Transform idleLocation;

    [SerializeField] private AudioClip deathSound;
    // Used to get death animation clip length
    [SerializeField] private AnimationClip deathAnimation;
    
    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponentInChildren<Animator>();
        _audioSource = GetComponent<AudioSource>();

        _attackDist = _agent.stoppingDistance + 1f;

        _idlePosition = idleLocation.position;
    }

    private void Update()
    {
        if (!_isDead)
        {
            if (IsInRangeToPlayer())
            {
                if (!_audioSource.isPlaying)
                {
                    _audioSource.Play();
                }
                
                _agent.SetDestination(PlayerController.Instance.transform.position);
                if (IsInAttackRangeToPlayer())
                {
                    _animState = SpiderAnimationState.Attacking;
                    AttackPlayer();
                }
                else
                {
                    SetWalkingOrIdle();
                }
            }
            else
            {
                if (_audioSource.isPlaying)
                {
                    _audioSource.Pause();
                }
                
                _agent.SetDestination(_idlePosition);
                SetWalkingOrIdle();
            }
        }
        else
        {
            _animState = SpiderAnimationState.Death;
        }
        
        PlayAnimation();
    }

    private bool IsInAttackRangeToPlayer()
    {
        return GetHorizontalDistanceToPlayer() < _attackDist;
    }

    private bool IsInRangeToPlayer()
    {
        var playerPos = PlayerController.Instance.transform.position;

        if (GetHorizontalDistanceToPlayer() < targetPlayerRange)
        {
            var verticalDistToPlayer = Mathf.Abs(transform.position.y - playerPos.y);
            if (verticalDistToPlayer < 1.5f)
            {
                return true;
            }
        }

        return false;
    }

    private float GetHorizontalDistanceToPlayer()
    {
        var playerPos = PlayerController.Instance.transform.position;
        
        var toPlayerVec = transform.position - playerPos;
        toPlayerVec.y = 0;
        
        return toPlayerVec.magnitude;
    }
    
    private void AttackPlayer()
    {
        Flashlight.Instance.DecreaseBatteryLevel(damage * Time.deltaTime);
    }

    private void SetWalkingOrIdle()
    {
        var isStopped = !(_agent.velocity.magnitude > 0f);
        _animState = isStopped ? SpiderAnimationState.Idle : SpiderAnimationState.Walking;
    }
    
    private void PlayAnimation()
    {
        switch (_animState)
        {
            case SpiderAnimationState.Idle:
                _animToPlay = "Idle";
                break;
            case SpiderAnimationState.Walking:
                _animToPlay = "Walk";
                break;
            case SpiderAnimationState.Attacking:
                _animToPlay = "Attack1";
                break;
            case SpiderAnimationState.Death :
                _animToPlay = "Death";
                break;
        }
        
        var animPlaying = _animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;

        if (animPlaying != _animToPlay)
        {
            _animator.Play(_animToPlay);
        }
    }
    
    public void Damage(float damageAmount)
    {
        health -= damageAmount;
        health = Mathf.Clamp(health, 0f, 100f);
        
        if (health == 0.0f)
        {
            _isDead = true;
            _agent.SetDestination(transform.position);
            _audioSource.PlayOneShot(deathSound);
            Destroy(gameObject, deathAnimation.length);
        }
    }
}
