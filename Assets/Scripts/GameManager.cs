using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public bool hasSurface = false;

    public GameObject pianoGameObject;

    public GameObject auditoriumGameObject;

    public GameObject centerEye;

    private Animator pianoAnim;
    private Animator auditoriumAnim;

    // Start is called before the first frame update
    void Start()
    {
        pianoAnim = pianoGameObject.GetComponent<Animator>();
        auditoriumAnim = auditoriumGameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void spawnPiano(Vector3 location, Vector3 rotation)
    {
        print("SPAWNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNN");
        pianoAnim.SetBool("Spawn", true);
        auditoriumAnim.SetBool("Spawn", true);
        pianoGameObject.transform.position = location;
        pianoGameObject.transform.LookAt(new Vector3(centerEye.transform.position.x, location.y, centerEye.transform.position.z));

        auditoriumGameObject.transform.position = new Vector3(location.x, location.y-2, location.z);
        auditoriumGameObject.transform.LookAt(new Vector3(centerEye.transform.position.x, auditoriumGameObject.transform.position.y, centerEye.transform.position.z));
        //pianoGameObject.transform.rotation.Set(0,);
    }
}
