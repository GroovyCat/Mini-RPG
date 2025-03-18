using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    Define.CameraMode _mode = Define.CameraMode.BackView;
    [SerializeField]
    Vector3 _delta = new Vector3(0.0f, 2.0f, -4.0f);
    [SerializeField]
    GameObject _player = null;


    private void LateUpdate() // Update 다음에 실행되는 Update문
    {
        if (_mode == Define.CameraMode.BackView)
        {
            RaycastHit hit;
            if (Physics.Raycast(_player.transform.position, _delta, out hit, _delta.magnitude, LayerMask.GetMask("Wall")))
            {
                float dist = (hit.point - _player.transform.position).magnitude * 0.9f;
                transform.position = _player.transform.position + _delta.normalized * dist;
            }
            else
            {
                transform.position = _player.transform.position + _delta;
                transform.LookAt(_player.transform);
            }
        }
    }

    public void SetBackView(Vector3 delta)
    {
        _mode = Define.CameraMode.BackView;
        _delta = delta;
    }
}
