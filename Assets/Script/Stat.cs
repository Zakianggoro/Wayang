using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stat : MonoBehaviour
{
    public string nama;
    public int atk;
    public int maxHp;
    public int currentHp;
    public int maxEnergy = 100;
    public int currentEnergy = 30;
    public int energyGain = 10; 

    public bool TakeDamage(int dmg)
    {
        currentHp -= dmg;
        GainEnergy(energyGain);
        Debug.Log(currentHp);

        return currentHp <= 0;
    }

    public void Heal(int amount)
    {
        currentHp += amount;
        if (currentHp > maxHp)
        {
            currentHp = maxHp;
        }
        GainEnergy(energyGain);
        Debug.Log(currentHp);
    }

    public void GainEnergy(int amount)
    {
        currentEnergy += amount;
        if (currentEnergy > maxEnergy)
        {
            currentEnergy = maxEnergy;
        }
        Debug.Log($"Current Energy: {currentEnergy}/{maxEnergy}. Energy needed for Ultimate: {maxEnergy - currentEnergy}");
    }
}
