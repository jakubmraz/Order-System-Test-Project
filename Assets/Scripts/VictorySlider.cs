using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VictorySlider : MonoBehaviour
{
    private Slider theSlider;
    private UI theUI;

    void Awake()
    {
        theSlider = GetComponent<Slider>();
        theUI = GetComponentInParent<UI>();
    }

    public void IncreaseValue()
    {
        theSlider.value++;
        if(theSlider.value >= theSlider.maxValue)
            theUI.ShowVictoryScreen();
    }

    public void DecreaseValue()
    {
        theSlider.value--;
        if(theSlider.value <= theSlider.minValue)
            theUI.ShowLossScreen();
    }
}
