﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartSail : BasherPart
{

    /// <summary>
    /// Called on basher initialisation
    /// </summary>
    public override void Initialise(ref Basher basher)
    {
        base.Initialise(ref basher);

        ID = 5;

        basher.SpeedMultiplier *= 1.2f;
    }
}
