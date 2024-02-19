using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    [SerializeField]
    float speed;

    [SerializeField]
    float maxRight;

    [SerializeField]
    float maxLeft;

    [SerializeField]
    List<GameObject> objects;

    [SerializeField]
    List<GameObject> UIObjects;

    [SerializeField]
    float spawnRate;

    public int nextObjectIndex;

    private Rigidbody2D rb;

    private float spawnTimer;

    private GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spawnTimer = 0f;
        nextObjectIndex = Random.Range(0, objects.Count);
        showNextUI();
        gm = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal")*speed *Time.deltaTime;

        if ((horizontal < 0 && transform.position.x>maxLeft)|| (horizontal > 0 && transform.position.x < maxRight))
        {
            transform.Translate(horizontal, 0, 0);
        }

        if (Input.GetKey(KeyCode.Space) && spawnTimer > spawnRate)
        {
            spawnTimer = 0f;
            Instantiate(objects[nextObjectIndex], transform.position+ new Vector3(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f), 0), Quaternion.identity);
            gm.scoreAdd(nextObjectIndex * 100);
            nextObjectIndex = Random.Range(0, objects.Count);
            showNextUI();
        }

        //update spawn timer
        spawnTimer += Time.deltaTime;
    }

    void showNextUI()
    {
        foreach (GameObject go in UIObjects)
        {
            go.SetActive(false);
        }
        UIObjects[nextObjectIndex].SetActive(true);
    }
}
