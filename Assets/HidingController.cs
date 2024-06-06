using System.Collections.Generic;
using UnityEngine;

public class HidingController : MonoBehaviour
{
    public List<GameObject> hideObjects = new List<GameObject>();
    private static HidingController _instance;
    public static HidingController Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    public List<GameObject> GetHidingObjects()
    {
        return hideObjects;
    }
}