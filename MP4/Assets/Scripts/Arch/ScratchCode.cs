using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScratchCode : MonoBehaviour {

    public Vector3 inputDir;
    public Vector3 eulers;
    public float scale;

    public Vector3 output;

    public GameObject thingToFollow;
    private GameObject gO;

    private void Start()
    {
        gO = new GameObject();
        gO.transform.SetParent(GameObject.Find("Canvas").transform);

        Image im = gO.AddComponent<Image>();

        im.sprite = Resources.Load<Sprite>("Textures/ProgressRing");
    }

    //void Start () {
    //       for (int j = 0; j < 3; j++)
    //       {
    //           Debug.Log("J is " + j);
    //	    for (int i = 0; i < 100; i++)
    //           {
    //               if (i != 30)
    //               {
    //                   Debug.Log(i);
    //                   continue;
    //               }
    //               if (i == 30)
    //               {
    //                   Debug.Log("Found 30");
    //                   break;
    //               }
    //               if (i == 98)
    //               {
    //                   Debug.Log("Break did nothing. Going to return.");
    //                   return;
    //               }
    //           }
    //       }

    //       ServiceLocator.Instance.TaskManager.StartTask(new TestTask(new TestTask(new TestTask(null))));
    //}

    private void Update()
    {
        //output = Quaternion.Euler(eulers) * inputDir * scale;
        Vector3 scale = new Vector3(Screen.width, Screen.height, 0);
        gO.transform.localPosition = Vector3.Scale(Camera.main.WorldToViewportPoint(thingToFollow.transform.position), scale);
    }
}
