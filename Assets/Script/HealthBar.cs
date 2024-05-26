using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Stat stat;
    public Image fillImageHp;
    public Image fillImageSp;
    private Slider hpSlider;
    private Slider spSlider;

    private void Awake()
    {
        // Find sliders as child objects
        hpSlider = transform.Find("HpBar")?.GetComponent<Slider>();
        spSlider = transform.Find("SpBar")?.GetComponent<Slider>();

        // Debug logs to verify the sliders
        if (hpSlider == null)
        {
            Debug.LogError("HpSlider not found on HealthBar.");
        }

        if (spSlider == null)
        {
            Debug.LogError("SpSlider not found on HealthBar.");
        }
    }

    public void Initialize(Stat stat)
    {
        this.stat = stat;

        if (hpSlider != null)
        {
            // Initialize HP slider
            hpSlider.maxValue = stat.maxHp;
            hpSlider.value = stat.currentHp;
            UpdateFillImageHp();
        }

        if (spSlider != null)
        {
            // Initialize SP slider
            spSlider.maxValue = stat.maxEnergy;
            spSlider.value = stat.currentEnergy;
            UpdateFillImageSp();
        }
    }

    public void UpdateHealth()
    {
        if (hpSlider != null)
        {
            hpSlider.value = stat.currentHp;
            UpdateFillImageHp();
        }
    }

    public void UpdateEnergy()
    {
        if (spSlider != null)
        {
            spSlider.value = stat.currentEnergy;
            UpdateFillImageSp();
        }
    }

    private void UpdateFillImageHp()
    {
        if (fillImageHp != null && hpSlider != null)
        {
            fillImageHp.enabled = hpSlider.value > hpSlider.minValue;
        }
    }

    private void UpdateFillImageSp()
    {
        if (fillImageSp != null && spSlider != null)
        {
            fillImageSp.enabled = spSlider.value > spSlider.minValue;
        }
    }

    private void Update()
    {
        UpdateHealth();
        UpdateEnergy();

        if (hpSlider.value <= hpSlider.maxValue / 3)
        {
            fillImageHp.color = Color.red;
        }
        else if (hpSlider.value >= hpSlider.maxValue / 3)
        {
            fillImageHp.color = Color.green;
        }
        if (spSlider.value == spSlider.maxValue)
        {
            fillImageSp.color = Color.yellow;
        }
        else
        {
            fillImageSp.color = Color.blue;
        }
    }
}
