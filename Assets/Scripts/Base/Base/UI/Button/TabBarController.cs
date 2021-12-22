using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabBarController : MonoBehaviour
{
    [SerializeField] private TabButtonController[] tabsController;
    public void Init()
    {
        for (int i = 0; i < tabsController.Length; i++)
        {
            int index = i;
            tabsController[index].OnClick.AddListener(() =>
            {
                EnableTab(index);
            });
        }
        
        EnableTab(0);
    }

    private void EnableTab(int tabIndex)
    {
        for (int i = 0; i < tabsController.Length; i++)
        {
            if (i == tabIndex)
            {
                tabsController[i].CanClick(false);
                continue;
            }

            tabsController[i].CanClick(true);
        }
    }
}