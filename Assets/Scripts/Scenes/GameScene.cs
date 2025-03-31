using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : BaseScene
{
    protected override void Init()
    {
        base.Init();

        SceneType = Define.Scene.Game;
        Managers.UI.ShowSceneUI<UIInventory>();

        StartCoroutine("ExplodeAfterSeconds", 4.0f);
    }

    IEnumerator ExplodeAfterSeconds(float second)
    {
        Debug.Log("Explode Enter");
        yield return new WaitForSeconds(second);
        Debug.Log("Explode Execute!!!");
    }

    public override void Clear()
    {
        
    }
}
