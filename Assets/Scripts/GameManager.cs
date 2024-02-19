using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    TMP_Text scoreText;

    private int score;

    

    private void Start()
    {
        score = 0;
        //scoreText.text = "Score: " + score;

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        ballCombine bc = collision.gameObject.GetComponent<ballCombine>();

        if (bc && bc.canLose)
        {
            Debug.Log("lose");
        }

    }



    public void scoreAdd(int i)
    {
        score += i;
        //scoreText.text = "Score: " + score;
    }
}
