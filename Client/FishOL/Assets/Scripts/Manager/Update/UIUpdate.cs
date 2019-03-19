using FairyGUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIUpdate : Window {

    private const float duration=0.3f;
    GTextField MainText;
    GProgressBar MainProBar;

    GComponent checkBox;
    GTextField checkBoxText1;
    GTextField checkBoxText2;
    GButton btnCancel;
    GButton btnConfirm;
    // 如果修改了UIUpdate的结构或者组件 这里一定要兼容老的版本 否则造成还没进入游戏界面就报错
    protected override void OnInit()
    {
        this.contentPane.fairyBatching = true;
        this.sortingOrder = 99999;
        this.SetSize(GRoot.inst.width, GRoot.inst.height);
        MainProBar=contentPane.GetChild("ProBar").asProgress;
        MainText = contentPane.GetChild("MainText").asTextField;
        checkBox = contentPane.GetChild("checkBox").asCom;
        checkBoxText1 = checkBox.GetChild("text1").asTextField;
        checkBoxText2 = checkBox.GetChild("text2").asTextField;
        btnCancel = checkBox.GetChild("btnCancel").asButton;
        btnConfirm = checkBox.GetChild("btnConfirm").asButton;
        checkBox.visible = false;

        MainProBar.value = 0;
        MainText.text = "";
    }
    public void SetText(string Text)
    {
        MainText.text = Text;
    }
    public void SetProgress(double value)
    {
        MainProBar.value = value;
    }

    public void ShowCheckBox(string tip1, string tip2, System.Action cancelAction, System.Action confirmAction)
    {
        checkBoxText1.text = tip1;
        checkBoxText2.text = tip2;
        checkBox.visible = true;

        btnCancel.onClick.Set(delegate ()
        {
            checkBox.visible = false;
            if (cancelAction != null)
                cancelAction();
        });
        btnConfirm.onClick.Set(delegate ()
        {
            checkBox.visible = false;
            if (confirmAction != null)
                confirmAction();
        });
    }
}
