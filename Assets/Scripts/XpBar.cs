using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class XpBar : MonoBehaviour
{
   public Slider slider;
    public Gradient gradient;
    public Image fill;

    public TMP_Text xpText;
    public TMP_Text lvlText;

    public void setMaxXp(int xp){

        slider.maxValue = xp;
    
    }

    public void setXp(int xp, int max){

        slider.value = xp;
        fill.color = gradient.Evaluate(slider.normalizedValue);

        xpText.text = slider.value.ToString() + "/" + max.ToString();
       

    }

    public void lvlUp(int newLvl, int newMax){

        lvlText.text = newLvl.ToString();
        
        setXp(0, newMax);
        setMaxXp(newMax);

    }
}
