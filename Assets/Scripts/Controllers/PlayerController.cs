using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    public enum PlayerState
    {
        Ready,
        Idle,
        Moving,
        Skill,
    }
    PlayerState _state = PlayerState.Idle;
    Define.CursorType crType = Define.CursorType.None;

    Vector3 _destPos; // ��������
    Vector3 dir; // Ű���� �̵� �� ����
    PlayerStat _pStat;
    Animator anim;
    NavMeshAgent nav;
    Texture2D attackCur;
    Texture2D handCur;
    GameObject _lockTarget;

    float gravity = -50.0f;
    float yVelocity = 0;
    bool isJumping = false; // ���� �ߺ� üũ
    int _clickMask = (1 << (int)Define.Layer.Ground) | (1 << (int)Define.Layer.Moster);

    private void Start()
    {
        attackCur = Managers.Resource.Load<Texture2D>("Textures/Cursor/Attack");
        handCur = Managers.Resource.Load<Texture2D>("Textures/Cursor/Hand");

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
        UpdateMouseCursor();

        switch (_state)
        {
            case PlayerState.Ready:
                UpdateReady();
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
        dir = Camera.main.transform.TransformDirection(dir); // ī�޶� �ٶ� ����
        transform.rotation = Quaternion.Euler(0, Mathf.Atan2(h, v) * Mathf.Rad2Deg, 0);
        transform.position += dir * _pStat.MoveSpeed; // �ش� �������� �̵�

        if (Input.GetButtonDown("Jump") && !isJumping)
        {
            yVelocity = _pStat.JumpPower;
            isJumping = true;
        }
        yVelocity += gravity * Time.deltaTime; // �߷� ���ӵ� ���
        dir.y = yVelocity;

        if (TryGetComponent(out CharacterController cc))
        {
            cc.Move(dir * _pStat.MoveSpeed * Time.deltaTime); // �̵�
            if (isJumping && cc.collisionFlags == CollisionFlags.Below)
            {
                isJumping = false;
                yVelocity = 0.0f;
            }
        }
    }

    void OnMouseEvent(Define.MouseEvent evt)
    {
        if (_state == PlayerState.Ready)
            return;

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        bool raycastHit = Physics.Raycast(ray, out hit, 100.0f, _clickMask);

        switch (evt)
        {
            case Define.MouseEvent.Press:
                {
                    if (_lockTarget != null)
                    {
                        _destPos = _lockTarget.transform.position;
                    }
                    else if (raycastHit)
                        _destPos = hit.point;
                }
                break;
            case Define.MouseEvent.PointerDown:
                {
                    if (raycastHit)
                    {
                        _destPos = hit.point; // ���콺�� ���� ��ǥ�� ��������
                        _state = PlayerState.Moving;

                        if (hit.collider.gameObject.layer == (int)Define.Layer.Moster)
                            _lockTarget = hit.collider.gameObject;
                        else
                            _lockTarget = null;
                    }
                }
                break;
            case Define.MouseEvent.PointerUp:
                _lockTarget = null;
                break;
            case Define.MouseEvent.Click:
                break;
            default:
                break;
        }
    }

    void UpdateReady()
    {

    }
    void UpdateIdle()
    {
        anim.SetFloat("speed", 0.0f);
    }

    void UpdateMoving()
    {
        Vector3 dir = _destPos - transform.position;

        nav.destination = _destPos; // �������� �̵��ϴ� �κ� (NavMeshAgent)
        anim.SetFloat("speed", _pStat.MoveSpeed);

        if (nav.destination == transform.position)
        {
            _state = PlayerState.Idle; 
            nav.isStopped = true; // ���ߴ� ���
            nav.ResetPath(); // ��� ����
        }
        if (Physics.Raycast(transform.position, dir, 1.0f, LayerMask.GetMask("Block"))) // ��ֹ��� ����� �� ��� ����
        {
            if (Input.GetMouseButton(0) == false)
            {
                _state = PlayerState.Idle;
                nav.isStopped = true; // ���ߴ� ���
                nav.ResetPath(); // ��� ����
            }
            return;
        }
    }

    void UpdateSkill()
    {

    }

    void UpdateMouseCursor()
    {
        if (Input.GetMouseButton(0))
            return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100.0f, _clickMask))
        {
            if (hit.collider.gameObject.layer == (int)Define.Layer.Moster)
            {
                if (crType != Define.CursorType.Attack)
                {
                    Cursor.SetCursor(attackCur, new Vector2(attackCur.width / 5, 0), CursorMode.Auto);
                    crType = Define.CursorType.Attack;
                }
            }
            else
            {
                if (crType != Define.CursorType.Hand)
                {
                    Cursor.SetCursor(handCur, new Vector2(handCur.width / 3, 0), CursorMode.Auto);
                    crType = Define.CursorType.Hand;
                }
            }
        }
    }
}
