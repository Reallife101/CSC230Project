using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class flash : MonoBehaviour
{
    Color originalColor;

    private IEnumerator coroutine;

    SpriteRenderer sr;


    // Start is called before the first frame update
    void Start()
    {
        //sr = GetComponent<SpriteRenderer>();
        //originalColor = sr.color;
        originalColor = new Color(255, 255, 255, 0);
    }

    public void stopCouritine()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
    }
    public void spawnFlash()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        coroutine = spawnCoroutine();
        StartCoroutine(coroutine);

    }

    private IEnumerator spawnCoroutine()
    {

        sr = GetComponent<SpriteRenderer>();

        sr.color = new Color(255, 255, 255, 0);
        originalColor = new Color(255, 255, 255, 0);
        yield return new WaitForSeconds(2f);
        /*
        sr.color = Color.red;
        yield return new WaitForSeconds(.5f);
        sr.color = originalColor;
        yield return new WaitForSeconds(.5f);
        sr.color = Color.red;
        //1 second
        yield return new WaitForSeconds(.5f);
        sr.color = originalColor;
        yield return new WaitForSeconds(.5f);
        
        */

        // 2 seconds
        yield return new WaitForSeconds(.5f);
        sr.color = originalColor;
        yield return new WaitForSeconds(.5f);
        sr.color = Color.red;
        // 3 seconds
        yield return new WaitForSeconds(.25f);
        sr.color = originalColor;
        yield return new WaitForSeconds(.25f);
        sr.color = Color.red;
        yield return new WaitForSeconds(.25f);
        sr.color = originalColor;
        yield return new WaitForSeconds(.25f);
        sr.color = Color.red;
        //4 Seconds
        yield return new WaitForSeconds(.125f);
        sr.color = originalColor;
        yield return new WaitForSeconds(.125f);
        sr.color = Color.red;
        yield return new WaitForSeconds(.125f);
        sr.color = originalColor;
        yield return new WaitForSeconds(.125f);
        sr.color = Color.red;
        yield return new WaitForSeconds(.125f);
        sr.color = originalColor;
        yield return new WaitForSeconds(.125f);
        sr.color = Color.red;
        yield return new WaitForSeconds(.125f);
        sr.color = originalColor;
        yield return new WaitForSeconds(.125f);


    }
}
