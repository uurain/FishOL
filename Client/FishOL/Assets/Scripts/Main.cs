using UnityEngine;
using System.Collections;
using FairyGUI;


public class Main : MonoBehaviour {

    private void Awake()
    {
        GameObject.DontDestroyOnLoad(gameObject);
        GameObject.DontDestroyOnLoad(Camera.main.gameObject);
    }

    void Start() {        
        InitUI();
    }

    void InitUI()
    {
        GRoot.inst.SetContentScaleFactor(1334, 750);
        UIConfig.defaultFont = "afont";
        FontManager.RegisterFont(FontManager.GetFont("txjlzy"), "Tensentype JiaLiZhongYuanJ");
        FontManager.RegisterFont(FontManager.GetFont("STXINWEI_1"), "华文新魏");

        UIObjectFactory.SetLoaderExtension(typeof(pg.MyGLoader));
    }
}    
