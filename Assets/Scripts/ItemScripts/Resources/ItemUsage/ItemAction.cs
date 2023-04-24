using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAction : MonoBehaviour
{
    public string Key;

    public virtual void useItem(Slot_UI slot) {}
    public virtual GameObject getItem() { return null; }
}
