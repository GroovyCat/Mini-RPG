using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    Define.CursorType crType = Define.CursorType.None;
    Texture2D attackCur;
    Texture2D handCur;

    int _clickMask = (1 << (int)Define.Layer.Ground) | (1 << (int)Define.Layer.Monster);

    // Start is called before the first frame update
    void Start()
    {
        attackCur = Managers.Resource.Load<Texture2D>("Textures/Cursor/Attack");
        handCur = Managers.Resource.Load<Texture2D>("Textures/Cursor/Hand");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
            return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100.0f, _clickMask))
        {
            if (hit.collider.gameObject.layer == (int)Define.Layer.Monster)
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
