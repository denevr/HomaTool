using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiatePrefab : MonoBehaviour
{
    [SerializeField]
    Vector3 m_offset;
    [SerializeField]
    GameObject m_prefab;

    bool triggered = false;

    public void DoCopyParent()
    {
        Instantiate(m_prefab, transform.parent.position + m_offset, transform.parent.rotation, transform.parent.parent);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!triggered)
        {
            DoCopyParent();
            triggered = true;
        }
    }
}
