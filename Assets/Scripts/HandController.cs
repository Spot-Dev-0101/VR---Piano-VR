using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HandController : MonoBehaviour
{
    public GameManager gm;

    public GameObject handPrefab;

    public float downDeadZone = 5;

    public float timeHandNeedsToBeDown = 3;

    private float timer = 0;

    private SkinnedMeshRenderer handMeshRender;

    // Start is called before the first frame update
    void Start()
    {
        handMeshRender = handPrefab.GetComponent<SkinnedMeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gm.startMenu.active)
        {
            Vector3 rotation = transform.localRotation.eulerAngles;//UnityEditor.TransformUtils.GetInspectorRotation(transform);
            if (gm.hasSurface == false && handMeshRender.enabled)
            {
                if (rotation.x < downDeadZone && rotation.x > -downDeadZone && rotation.z < downDeadZone && rotation.z > -downDeadZone)
                {
                    //print("Down");
                    handPrefab.GetComponent<Renderer>().material.SetColor("_Color", Color.yellow);
                    timer += Time.deltaTime;
                    if (timer >= timeHandNeedsToBeDown)
                    {
                        gm.hasSurface = true;
                        gm.spawnPiano(new Vector3(transform.position.x, transform.position.y - 0f, transform.position.z), new Vector3(0, 0, 0));
                    }
                }
                else
                {
                    //print("Up");
                    handPrefab.GetComponent<Renderer>().material.SetColor("_Color", Color.red);
                    timer = 0;
                }
            }
        } else if (gm.startMenuInstructions.active)
        {
            handPrefab.GetComponent<Renderer>().material.SetColor("_Color", Color.green);
        }
    }
}
