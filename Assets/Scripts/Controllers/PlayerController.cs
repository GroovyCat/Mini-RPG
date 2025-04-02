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

    Vector3 _destPos; // 도착지점
    PlayerStat _pStat;
    Animator anim;
    NavMeshAgent nav;

    int _clickMask = (1 << (int)Define.Layer.Ground) | (1 << (int)Define.Layer.Moster);

    private void Start()
    {
        anim = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();
        _pStat = gameObject.GetOrAddComponent<PlayerStat>();

        Managers.Input.KeyAction -= OnKeyBoard;
        Managers.Input.KeyAction += OnKeyBoard;
        Managers.Input.MouseAction -= OnMouseClicked;
        Managers.Input.MouseAction += OnMouseClicked;
    }

    private void Update()
    {
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
        if (Input.GetKey(KeyCode.W))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.forward), 0.2f);
            transform.position += Vector3.forward * _pStat.MoveSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.back), 0.2f);
            transform.position += Vector3.back * _pStat.MoveSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.left), 0.2f);
            transform.position += Vector3.left * _pStat.MoveSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.right), 0.2f);
            transform.position += Vector3.right * _pStat.MoveSpeed * Time.deltaTime;
        }
        _state = PlayerState.Idle;
    }

    void OnMouseClicked(Define.MouseEvent evt)
    {
        if (_state == PlayerState.Ready)
            return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100.0f, _clickMask))
        {
            _destPos = hit.point; // 마우스로 찍은 좌표가 도착지점
            _state = PlayerState.Moving;

            if (hit.collider.gameObject.layer == (int)Define.Layer.Moster)
            {
                Debug.Log("Monster Click");
            }
            else
            {
                Debug.Log("Ground Click");
            }
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

        nav.destination = _destPos;
        anim.SetFloat("speed", _pStat.MoveSpeed);

        if (nav.destination == transform.position)
        {
            _state = PlayerState.Idle;
            nav.isStopped = true; // 멈추는 기능
            nav.ResetPath(); // 경로 리셋
        }
        if (Physics.Raycast(transform.position, dir, 1.0f, LayerMask.GetMask("Block")))
        {
            _state = PlayerState.Idle;
            nav.isStopped = true; // 멈추는 기능
            nav.ResetPath(); // 경로 리셋
            return;
        }
    }

    void UpdateSkill()
    {

    }
}
