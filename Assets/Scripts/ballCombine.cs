using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ballCombine : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    public int level;

    public bool spawn = true;

    public bool canLose = false;

    [SerializeField]
    List<GameObject> objects;

    private GameManager gm;

    private void Start()
    {
        gm = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameManager>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        ballCombine bc = collision.gameObject.GetComponent<ballCombine>();

        if (bc)
        {
            canLose = true;
            if (bc.level == level)
            {
                //Prevent double spawning
                bc.spawn = false;
                if (spawn)
                {
                    //Calculate midpoint to spawn at
                    Vector3 bcPos = bc.gameObject.transform.position;
                    Vector3 spawnLocation = new Vector3(bcPos.x + (transform.position.x - bcPos.x) / 2, bcPos.y + (transform.position.y - bcPos.y) / 2, bcPos.z + (transform.position.z - bcPos.z) / 2);

                    //Delete both and spawn new one
                    Destroy(bc.gameObject);
                    Instantiate(objects[Mathf.Min(level, objects.Count - 1)], spawnLocation, Quaternion.identity); //dont index out of range
                    gm.scoreAdd(Mathf.Min(level, objects.Count - 1) * 100);
                    Destroy(gameObject);
                }
            }
            
        }
    }
}
