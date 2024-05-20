using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplicationQuitListener : MonoBehaviour
{
    public static System.Action OnApplicationQuitEvent;

    private void OnApplicationQuit()
    {
        OnApplicationQuitEvent?.Invoke();
    }
}
