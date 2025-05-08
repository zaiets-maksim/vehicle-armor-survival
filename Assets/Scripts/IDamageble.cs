using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageble
{
    public int Health { get; }
    
    public event Action<int> OnTakeDamage;
}
