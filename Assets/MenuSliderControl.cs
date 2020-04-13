using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSliderControl : MonoBehaviour
{
    private Animator controller;

    public void ShowHideMenu() {

        controller = GetComponent<Animator>();

        if (controller != null) {
            bool isShowingMenu = controller.GetBool("showMenu");
            controller.SetBool("showMenu", !isShowingMenu);
           
        }
    }
}
