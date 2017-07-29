using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour {

    public Text healText;
    public Slider healthSlider;
    public Image healthFillImage;
    public Text shells_a;
    public Image zhunXin_a;

    private void Awake ()
    {
        
        GameplayStatics.LocalPlayer.health.AddChangedListener(ChangeHealth);
        GameplayStatics.LocalPlayer.health.AddChangedListener(RefreshHealthHUD);
        GameplayStatics.LocalPlayer.shells.AddChangedListener(ChangeShells);
        GameplayStatics.LocalPlayer.zhunXin.AddChangedListener(ChangeZhunxin);
    }
    void ChangeHealth()
    {
        healText.text = "生命值： "+GameplayStatics.LocalPlayer.health.Get().ToString();

    }
    public void RefreshHealthHUD()
    {
        healthSlider.value = GameplayStatics.LocalPlayer.health.Get();
       
    }
    public void ChangeShells()
    {
        shells_a.text = "子弹数剩余：" + GameplayStatics.LocalPlayer.shells.Get();

    }
    public void ChangeZhunxin()  
    {
        Debug.Log("4");
        float v = GameplayStatics.LocalPlayer.zhunXin.Get();
        Debug.Log(v);
        zhunXin_a.transform.localScale = Vector3.Lerp(new Vector3(v, v, v), zhunXin_a.transform.localScale, Time.deltaTime);
        //zhunXin_a.transform.localScale=new Vector3(v, v, v);
    }

    void Update () {

        
    }
}
