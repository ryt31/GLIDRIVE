using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEventSwitchable
{
    void SwitchEvent(string flagName, bool flag);
}
