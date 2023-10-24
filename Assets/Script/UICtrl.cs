using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UICtrl : MonoBehaviour
{
    private VisualElement Bt_List;

    private VisualElement BtsList;
    
    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        Bt_List = root.Q<Button>("List_Bt");
        BtsList = root.Q<VisualElement>("BtList");

        Bt_List.RegisterCallback<ClickEvent>(ListOpen);

    }

    private void ListOpen(ClickEvent evt)
    {
        if (BtsList.ClassListContains("BtList-On"))
        {
            BtsList.style.display = DisplayStyle.None;
            BtsList.RemoveFromClassList("BtList-On");
        }
        else
        {
            BtsList.style.display = DisplayStyle.Flex;
            BtsList.AddToClassList("BtList-On");
        }   
    }
}
