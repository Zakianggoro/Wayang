using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Stat stat;
    public Image fillImage;
    private Slider slider;

    private void Awake()
    {
        slider = GetComponent<Slider>();
    }

    public void Initialize(Stat stat)
    {
        this.stat = stat;
        slider.maxValue = stat.maxHp;
        slider.value = stat.currentHp;
        UpdateFillImage();
    }

    public void UpdateHealth()
    {
        slider.value = stat.currentHp;
        UpdateFillImage();
    }

    private void UpdateFillImage()
    {
        fillImage.enabled = slider.value > slider.minValue;
    }

    private void Update()
    {
        UpdateHealth();
    }
}