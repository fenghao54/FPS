using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour {

    public Text healText;
    public Slider healthSlider;
    public Image healthFillImage;


    private void Awake ()
    {
        
        GameplayStatics.LocalPlayer.health.AddChangedListener(changeHealth);
        GameplayStatics.LocalPlayer.health.AddChangedListener(RefreshHealthHUD);
    }
    void changeHealth()
    {
        healText.text = "生命值： "+GameplayStatics.LocalPlayer.health.Get().ToString();

    }
    public void RefreshHealthHUD()
    {
        healthSlider.value = GameplayStatics.LocalPlayer.health.Get();
       
    }


    void Update () {
       

    }
}
