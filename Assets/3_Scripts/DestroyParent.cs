using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinRace : MonoBehaviour
{
    public void DoDestroyParent()
    {
        Destroy(transform.parent.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        DoDestroyParent();
    }
}
