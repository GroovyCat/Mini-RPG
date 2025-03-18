using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    float _speed = 20.0f;

    bool _moveToDest = false; // 도착지점까지 이동했는지 여부
    Vector3 _destPos; // 도착지점

    private void Start()
    {
        Managers.Input.KeyAction -= OnKeyBoard;
        Managers.Input.KeyAction += OnKeyBoard;
        Managers.Input.MouseAction -= OnMouseClicked;
        Managers.Input.MouseAction += OnMouseClicked;
    }

    private void Update()
    {
        if (_moveToDest)
        {
            Vector3 dir = _destPos - transform.position; // 도착지점으로부터의 거리(위치 벡터) 값
            if (dir.magnitude < 0.0001f) // 벡터의 길이가 0.0001보다 작다면
            {
                _moveToDest = false; // 해당 벡터 길이 만큼 이동하지 않는 걸로 간주
            }
            else
            {
                float moveDist = Mathf.Clamp(_speed * Time.deltaTime, 0, dir.magnitude);
                transform.position += dir.normalized * moveDist; // 정규 위치 벡터 거리 만큼 이동
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 20 * Time.deltaTime);
                transform.LookAt(_destPos); // 도착 지점 좌표로 게임 오브젝트가 바라보게 한다
            }
        }
    }

    void OnKeyBoard()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.forward), 0.2f);
            transform.position += Vector3.forward * _speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.back), 0.2f);
            transform.position += Vector3.back * _speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.left), 0.2f);
            transform.position += Vector3.left * _speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.right), 0.2f);
            transform.position += Vector3.right * _speed * Time.deltaTime;
        }

        _moveToDest = false;
    }

    void OnMouseClicked(Define.MouseEvent evt)
    {
        if (evt != Define.MouseEvent.Click)
            return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        LayerMask mask = LayerMask.GetMask("Ground");

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100.0f, mask))
        {
            _destPos = hit.point; // 마우스로 찍은 좌표가 도착지점
            _moveToDest = true;
        }
    }
}
