using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public GameObject pianoPrefab;

    public bool hasSurface = false;

    private GameObject pianoGameObject;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void spawnPiano(Vector3 location, Vector3 rotation)
    {
        pianoGameObject = Instantiate(pianoPrefab, location, Quaternion.identity);
    }
}
