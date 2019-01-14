using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewMenuEqpt : MP4_ScheduledMono {

    public override void Awake()
    {
        priority = 3000;
        base.Awake();
    }
}
