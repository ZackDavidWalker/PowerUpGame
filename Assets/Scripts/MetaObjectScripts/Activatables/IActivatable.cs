﻿using UnityEngine;
using System.Collections;

public interface IActivatable
{
    void Activate();

    void Deactivate();

    bool IsActivated();
}