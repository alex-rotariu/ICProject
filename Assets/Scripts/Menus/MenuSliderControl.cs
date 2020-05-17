using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSliderControl : MonoBehaviour {
    private Animator controller;
    private bool isShowing = false;
    private List<MenuSliderControl> otherMenus = new List<MenuSliderControl>();

    public void ShowHideMenu() {

        controller = GetComponent<Animator>();
        if (OtherOpenedMenus())
            return;
        if (controller != null) {
            bool isShowingMenu = controller.GetBool("showMenu");
            controller.SetBool("showMenu", !isShowingMenu);
            isShowing = !isShowing;
        }
    }

    private bool OtherOpenedMenus() {
        foreach(MenuSliderControl menu in otherMenus) {
            if (menu.IsShowing())
                return true;
        }
        return false;
    }

    private void Start() {
        var menus = FindObjectsOfType<MenuSliderControl>();
        foreach (MenuSliderControl menu in menus){
            if (menu != this)
                otherMenus.Add(menu);
        }     
    }

    public void SkipSong()
    {
        FindObjectOfType<OptionsController>().SkipSong();
    }

    public bool IsShowing() {
        return isShowing;
    }
}
