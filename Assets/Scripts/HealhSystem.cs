using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealhSystem : MonoBehaviour
{
    public int Healh;
    public int NumberOfLives;
    public Image[] lives;
    public Sprite FullLive;
    public Sprite EmptyLive;

    public void HealhUpdate() {
        if (Healh > NumberOfLives)
        {
            Healh = NumberOfLives;
        }

        for (int i = 0; i < lives.Length; i++)
        {
            if (i < Healh)
            {
                lives[i].sprite = FullLive;
            }
            else
            {
                lives[i].sprite = EmptyLive;
            }

            if (i < NumberOfLives)
            {
                lives[i].enabled = true;
            }
            else
            {
                lives[i].enabled = false;
            }
        }
    }

    public void RestoreFullHp()
    {
        Healh = NumberOfLives;
    }
}
