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
    }

    public override void Clear()
    {

    }
}
