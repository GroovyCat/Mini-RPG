using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    public enum PlayerState
    {
        Die,
        Idle,
        Moving,
        Skill,
    }
    [SerializeField]
    PlayerState _state = PlayerState.Idle;

    public PlayerState State 
    { 
        get { return _state; }
        set
        {
            _state = value;

            anim = GetComponent<Animator>();
            switch (_state)
            {
                case PlayerState.Die:
                    anim.SetBool("attack", false);
                    break;
                case PlayerState.Idle:
                    anim.SetFloat("speed", 0.0f);
                    anim.SetBool("attack", false);
                    break;
                case PlayerState.Moving:
                    anim.SetBool("attack", false);
                    break;
                case PlayerState.Skill:
                    anim.SetBool("attack", true);
                    break;
                default:
                    break;
            }

        }
    }


    Vector3 _destPos; // 도착지점
    Vector3 dir; // 키보드 이동 시 방향
    PlayerStat _pStat;
    Animator anim;
    NavMeshAgent nav;
    GameObject _lockTarget;

    float gravity = -50.0f;
    float yVelocity = 0;
    bool isJumping = false; // 점프 중복 체크
    int _clickMask = (1 << (int)Define.Layer.Ground);
    int _clickMonster = (1 << (int)Define.Layer.Monster);

    private void Start()
    {
        anim = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();
        _pStat = gameObject.GetOrAddComponent<PlayerStat>();

        Managers.Input.KeyAction -= OnKeyBoard;
        Managers.Input.KeyAction += OnKeyBoard;
        Managers.Input.MouseAction -= OnMouseEvent;
        Managers.Input.MouseAction += OnMouseEvent;
    }

    private void Update()
    {

        switch (State)
        {
            case PlayerState.Die:
                UpdateDie();
                break;
            case PlayerState.Idle:
                UpdateIdle();
                break;
            case PlayerState.Moving:
                UpdateMoving();
                break;
            case PlayerState.Skill:
                UpdateSkill();
                break;
            default:
                break;
        }
    }

    void OnKeyBoard()
    {
        var h = Input.GetAxis("Horizontal");
        var v = Input.GetAxis("Vertical");

        dir = new Vector3(h, 0, v);
        dir = dir.normalized;

        anim.SetFloat("speed", _pStat.MoveSpeed);
        dir = Camera.main.transform.TransformDirection(dir); // 카메라가 바라본 방향
        transform.rotation = Quaternion.Euler(0, Mathf.Atan2(h, v) * Mathf.Rad2Deg, 0);
        transform.position += dir * _pStat.MoveSpeed; // 해당 방향으로 이동

        if (Input.GetButtonDown("Jump") && !isJumping)
        {
            yVelocity = _pStat.JumpPower;
            isJumping = true;
        }
        yVelocity += gravity * Time.deltaTime; // 중력 가속도 계산
        dir.y = yVelocity;

        if (TryGetComponent(out CharacterController cc))
        {
            cc.Move(dir * _pStat.MoveSpeed * Time.deltaTime); // 이동
            if (isJumping && cc.collisionFlags == CollisionFlags.Below)
            {
                isJumping = false;
                yVelocity = 0.0f;
            }
        }
    }

    void OnMouseEvent(Define.MouseEvent evt)
    {
        if (State == PlayerState.Die)
            return;

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        bool raycastHitGround = Physics.Raycast(ray, out hit, 100.0f, _clickMask);
        bool raycastHitMonster = Physics.Raycast(ray, out hit, 100.0f, _clickMonster);

        switch (evt)
        {
            case Define.MouseEvent.Press:
                {
                    if (_lockTarget != null)
                    {
                        _destPos = _lockTarget.transform.position;
                    }
                    else if (raycastHitGround)
                        _destPos = hit.point;
                }
                break;
            case Define.MouseEvent.PointerDown:
                {
                    if (raycastHitGround)
                    {
                        State = PlayerState.Idle;
                    }
                }
                break;
            case Define.MouseEvent.Click:
                if (raycastHitMonster)
                {
                    State = PlayerState.Skill;
                }
                break;
            default:
                break;
        }
    }

    void UpdateDie()
    {

    }
    void UpdateIdle()
    {

    }

    void UpdateMoving()
    {
        if (_lockTarget != null)
        {
            float distance = (_destPos - transform.position).magnitude;
            if (distance <= 1)
            {
                State = PlayerState.Skill;
                return;
            }
        }
    }

    void UpdateSkill()
    {

    }

    void OnHitEvent()
    {
        Debug.Log("OnHitEvent");
        State = PlayerState.Moving;
    }
}
