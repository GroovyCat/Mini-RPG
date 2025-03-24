using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UIInventory : UIScene
{

    enum GameObjects
    {
        GridPanel
    }
    void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();
        Bind<GameObject>(typeof(GameObject));

        GameObject gridPanel = Get<GameObject>((int)GameObjects.GridPanel);
        foreach (Transform chile in gridPanel.transform)
            Managers.Resource.Destroy(chile.gameObject);

        for (int i = 0; i < 6; i++)
        {
            GameObject item = Managers.UI.MakeSubItem<UIInventoryItem>(gridPanel.transform).gameObject;
            UIInventoryItem invenItem = item.GetOrAddComponent<UIInventoryItem>();
            invenItem.SetInfo($"집행검{i}번");
        }
    }
    
}
