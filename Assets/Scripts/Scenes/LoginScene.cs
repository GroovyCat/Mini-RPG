using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginScene : BaseScene
{

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Managers.SceneEX.LoadScene(Define.Scene.Game);
        }
    }

    protected override void Init()
    {
        base.Init();

        SceneType = Define.Scene.Login;

        List<GameObject> list = new List<GameObject>();
        for (int i = 0; i < 5; i++)
        {
            list.Add(Managers.Resource.Instantiate("UnityChan"));
        }

        foreach (GameObject go in list)
        {
            Managers.Resource.Destroy(go);
        }
    }

    public override void Clear()
    {

    }
}
