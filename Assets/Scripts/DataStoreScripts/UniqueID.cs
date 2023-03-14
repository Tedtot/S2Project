using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[ExecuteInEditMode]
public class UniqueID : MonoBehaviour
{
    [ReadOnly, SerializeField] private string id;
    [SerializeField] private static SerializableDictionary<string, GameObject> idDatabase = new SerializableDictionary<string, GameObject>();
    public string ID => id;
    private void Awake() {
        if (idDatabase == null) idDatabase = new SerializableDictionary<string, GameObject> ();
        if (idDatabase.ContainsKey(id)) generate();
        else idDatabase.Add(ID, this.gameObject);
    }
    private void OnDestroy() {
        if(idDatabase.ContainsKey(id)) idDatabase.Remove(id);
    }
    private void generate() {
        id = Guid.NewGuid().ToString();
        idDatabase.Add(id, this.gameObject);
    }
}
