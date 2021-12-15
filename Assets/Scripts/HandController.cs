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
        Vector3 editorRoation = UnityEditor.TransformUtils.GetInspectorRotation(transform);
        if (gm.hasSurface == false && handMeshRender.enabled)
        {
            if (editorRoation.x < downDeadZone && editorRoation.x > -downDeadZone && editorRoation.z < downDeadZone && editorRoation.z > -downDeadZone)
            {
                //print("Down");
                handPrefab.GetComponent<Renderer>().material.SetColor("_Color", Color.yellow);
                timer += Time.deltaTime;
                if (timer >= timeHandNeedsToBeDown)
                {
                    gm.hasSurface = true;
                    gm.spawnPiano(transform.position, new Vector3(0, 0, 0));
                }
            }
            else
            {
                //print("Up");
                handPrefab.GetComponent<Renderer>().material.SetColor("_Color", Color.red);
                timer = 0;
            }
        } else
        {
            handPrefab.GetComponent<Renderer>().material.SetColor("_Color", Color.green);
        }
        
    }
}
