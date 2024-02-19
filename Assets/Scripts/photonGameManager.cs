using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class photonGameManager : MonoBehaviour
{

    photonBallCombine currentbc;

    private void OnTriggerStay2D(Collider2D collision)
    {
        photonBallCombine bc = collision.gameObject.GetComponent<photonBallCombine>();

        if (bc && bc.canLose)
        {
            currentbc = bc;
            bc.flash = true;
            currentbc.die = true;

        }
        else
        {
            if (currentbc != null)
            {
                currentbc.flash = false;
                currentbc.die = false;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(currentbc != null)
        {
            currentbc.flash = false;
            currentbc.die = false;
        }
    }


}
