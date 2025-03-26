using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UIManager
{
    int _order = 10;
    
    Stack<UIPopup> _popupStack = new Stack<UIPopup>();
    UIScene _sceneUI = null;

    public GameObject Root 
    { 
        get 
        {
            GameObject root = GameObject.Find("@UIRoot");
            if (root == null)
                root = new GameObject { name = "@UIRoot" };
            return root;
        }  
    }

    public void SetCanvas(GameObject go, bool sort = true)
    {
        Canvas canvas = Util.GetOrAddComponent<Canvas>(go);
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.overrideSorting = true;

        if (sort)
        {
            canvas.sortingOrder = _order;
            _order++;
        }
        else
            canvas.sortingOrder = 0;
    }

    public T ShowPopupUI<T>(string name = null) where T : UIPopup
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = Managers.Resource.Instantiate($"UI/Popup/{name}");
        T popupUI = Util.GetOrAddComponent<T>(go);
        _popupStack.Push(popupUI);

        go.transform.SetParent(Root.transform);

        return popupUI;
    }

    public T ShowSceneUI<T>(string name = null) where T : UIScene
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = Managers.Resource.Instantiate($"UI/Scene/{name}");
        T sceneUI = Util.GetOrAddComponent<T>(go);
        _sceneUI = sceneUI;

        go.transform.SetParent(Root.transform);
        return sceneUI;
    }

    public T MakeSubItem<T>(Transform parent = null, string name = null) where T : UIBase
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = Managers.Resource.Instantiate($"UI/SubItem/{name}");
        if (parent != null)
            go.transform.SetParent(parent);

        return Util.GetOrAddComponent<T>(go);
    }

    public void ClosePopupUI()
    {
        if (_popupStack.Count == 0) return;

        UIPopup popup = _popupStack.Pop();
        Managers.Resource.Destroy(popup.gameObject);
        popup = null;
    }

    public void ClosePopupUI(UIPopup popup)
    {
        if (_popupStack.Count == 0) return;

        if (_popupStack.Peek() != popup)
        {
            return;
        }
        ClosePopupUI();
    }

    public void CloseAllPopupUI()
    {
        while (_popupStack.Count > 0)
            ClosePopupUI();
    }

    public void Clear()
    {
        CloseAllPopupUI();
        _sceneUI = null;
    }
}
