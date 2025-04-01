using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    float _speed = 20.0f;

    Vector3 _destPos; // ��������
    Animator anim;
    public enum PlayerState
    {
        Ready,
        Idle,
        Moving,
    }

    PlayerState _state = PlayerState.Idle;

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
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
            default:
                break;
        }
    }

    void OnKeyBoard()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.forward), 0.2f);
            transform.position += Vector3.forward * _speed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.back), 0.2f);
            transform.position += Vector3.back * _speed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.left), 0.2f);
            transform.position += Vector3.left * _speed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.right), 0.2f);
            transform.position += Vector3.right * _speed * Time.deltaTime;
        }
        _state = PlayerState.Idle;
    }

    void OnMouseClicked(Define.MouseEvent evt)
    {
        if (_state == PlayerState.Ready)
            return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        LayerMask mask = LayerMask.GetMask("Ground");

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100.0f, mask))
        {
            _destPos = hit.point; // ���콺�� ���� ��ǥ�� ��������
            _state = PlayerState.Moving;
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
        Vector3 dir = _destPos - transform.position; // �����������κ����� �Ÿ�(��ġ ����) ��
        if (dir.magnitude < 0.0001f) // ������ ���̰� 0.0001���� �۴ٸ�
        {
            _state = PlayerState.Idle; // �ش� ���� ���� ��ŭ �̵����� �ʴ� �ɷ� ����
        }
        else
        {
            float moveDist = Mathf.Clamp(_speed * Time.deltaTime, 0, dir.magnitude);
            transform.position += dir.normalized * moveDist; // ���� ��ġ ���� �Ÿ� ��ŭ �̵�
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 20 * Time.deltaTime);
        }

        anim.SetFloat("speed", 1.0f);
    }
}
