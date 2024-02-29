using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class canvasItem : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Text PlayerName;

    private int index;

    private int OwnerActorNumber;
    private float CurrentHealth;

    public void Initialize(string nickname, int actornumber, Sprite sprite)
    {
        OwnerActorNumber = actornumber;
        SetPlayerName(nickname);
    }

    public int GetOwnerActorNumber()
    {
        return OwnerActorNumber;
    }

    public void SetPlayerName(string nickname)
    {
        PlayerName.text = nickname;
    }

    public void SetHealthUI(float health)
    {
        CurrentHealth = health;
    }

    public void IncreaseHealthUI(float health)
    {
        CurrentHealth = CurrentHealth + health;
    }

    public void ChangeHealthUI(float health)
    {
        CurrentHealth = health;
    }

    public void DecreaseHealthUI(float damage)
    {
        CurrentHealth = CurrentHealth - damage;
        if (CurrentHealth < 0f)
        {
            CurrentHealth = 0f;
        }
    }

    public int getActorNumber()
    {
        return OwnerActorNumber;
    }
}
