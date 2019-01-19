using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScratchCode : MonoBehaviour {

    public Vector3 inputDir;
    public Vector3 eulers;
    public float scale;

    public Vector3 output;

	void Start () {
        for (int j = 0; j < 3; j++)
        {
            Debug.Log("J is " + j);
		    for (int i = 0; i < 100; i++)
            {
                if (i != 30)
                {
                    Debug.Log(i);
                    continue;
                }
                if (i == 30)
                {
                    Debug.Log("Found 30");
                    break;
                }
                if (i == 98)
                {
                    Debug.Log("Break did nothing. Going to return.");
                    return;
                }
            }
        }

        ServiceLocator.Instance.TaskManager.StartTask(new TestTask(new TestTask(new TestTask(null))));
	}

    private void Update()
    {
        output = Quaternion.Euler(eulers) * inputDir * scale;
    }
}
