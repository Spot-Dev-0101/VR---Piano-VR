using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using OculusSampleFramework;

public class GameManager : MonoBehaviour
{

    public bool hasSurface = false;

    public GameObject pianoGameObject;

    public GameObject auditoriumGameObject;

    public GameObject centerEye;

    public GameObject startMenu;
    public GameObject startMenuStart;
    public GameObject startMenuInstructions;

    [Serializable]
    public struct KeyGroup
    {
        public GameObject c;
        public GameObject db;
        public GameObject d;
        public GameObject eb;
        public GameObject e;
        public GameObject f;
        public GameObject gb;
        public GameObject g;
        public GameObject ab;
        public GameObject a;
        public GameObject bb;
        public GameObject b;

        public GameObject get(String name)
        {

            switch (name)
            {
                case "c":
                    return c;
                case "db":
                    return db;
                case "d":
                    return d;
                case "eb":
                    return eb;
                case "e":
                    return e;
                case "f":
                    return f;
                case "gb":
                    return gb;
                case "g":
                    return g;
                case "ab":
                    return ab;
                case "a":
                    return a;
                case "bb":
                    return bb;
                case "b":
                    return b;
            }

            return null;
        }
    }
    
    public KeyGroup[] keys;

    private Animator pianoAnim;
    private Animator auditoriumAnim;

    private bool recenteringMenu = false;

    private bool playAlongMode = false;

    public GameObject notePrefab;

    [Serializable]
    public struct songNote
    {
        public int Group;
        public String key;
        public float time;
    }

    public songNote[] songNotes;

    private List<GameObject> notePool = new List<GameObject>();
    private bool playingSong = false;
    private int noteIndex = 0;
    private float timer = 0;

    // Start is called before the first frame update
    void Start()
    {
        pianoAnim = pianoGameObject.GetComponent<Animator>();
        auditoriumAnim = auditoriumGameObject.GetComponent<Animator>();
        startMenu.transform.position = centerEye.transform.forward / 1.75f;

        for (int i = 0; i < songNotes.Length; i++)
        {
            GameObject tempNote = Instantiate(notePrefab, new Vector3(0, -9999, 0), Quaternion.identity);
            notePool.Add(tempNote);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (startMenu.active)
        {
            Vector3 forwardPos = centerEye.transform.position + centerEye.transform.forward / 1.75f;

            float distance = Vector3.Distance(forwardPos, startMenu.transform.position);



            if (distance > 0.2f && recenteringMenu == false)
            {
                recenteringMenu = true;
            }

            if (recenteringMenu == true)
            {
                Vector3 newMenuPosition = Vector3.Lerp(startMenu.transform.position, forwardPos, 0.02f);

                startMenu.transform.position = newMenuPosition;

                if (distance < 0.01f)
                {
                    recenteringMenu = false;
                }
            }

            startMenu.transform.LookAt(centerEye.transform.position);
        }

        if(playingSong == true)
        {
            songNote currentNote = songNotes[noteIndex];
            if (currentNote.time <= timer)
            {
                GameObject key = keys[currentNote.Group].get(currentNote.key);
                print(key);
                notePool[noteIndex].transform.position = new Vector3(key.transform.position.x, key.transform.position.y + 1, key.transform.position.z);
                noteIndex++;
            }

            if (noteIndex >= songNotes.Length)
            {
                playingSong = false;
            }

            timer += Time.deltaTime;
        }

        foreach (GameObject note in notePool)
        {

            if (note.transform.position.y < pianoGameObject.transform.position.y)
            {
                note.transform.position = new Vector3(note.transform.position.x, -9999, note.transform.position.z);
            }
            else
            {
                note.transform.position = new Vector3(note.transform.position.x, note.transform.position.y - 0.001f, note.transform.position.z);
            }
        }

    }

    public void spawnPiano(Vector3 location, Vector3 rotation)
    {
        //print("SPAWNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNN");
        pianoAnim.SetBool("Spawn", true);
        auditoriumAnim.SetBool("Spawn", true);
        pianoGameObject.transform.position = location + centerEye.transform.forward/12;
        pianoGameObject.transform.LookAt(new Vector3(centerEye.transform.position.x, centerEye.transform.position.y, centerEye.transform.position.z));
        pianoGameObject.transform.localRotation = Quaternion.Euler(pianoGameObject.transform.localRotation.x, pianoGameObject.transform.localRotation.y, 0);

        auditoriumGameObject.transform.position = new Vector3(location.x, location.y-2, location.z);
        auditoriumGameObject.transform.LookAt(new Vector3(centerEye.transform.position.x, auditoriumGameObject.transform.position.y, centerEye.transform.position.z));
        //pianoGameObject.transform.rotation.Set(0,);

        startMenu.SetActive(false);

        if (playAlongMode == true)
        {
            playingSong = true;
        }

        hasSurface = true;

    }

    public void startFreePlay(InteractableStateArgs obj)
    {
        if (obj.NewInteractableState == InteractableState.ActionState)
        {
            startMenuStart.SetActive(false);
            startMenuInstructions.SetActive(true);
        }
        
    }

    public void startPlayAlong(InteractableStateArgs obj)
    {
        if (obj.NewInteractableState == InteractableState.ActionState)
        {
            startMenuStart.SetActive(false);
            startMenuInstructions.SetActive(true);

            playAlongMode = true;
        }
    }
}
