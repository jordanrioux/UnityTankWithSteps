using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIDirectionControl : MonoBehaviour
{
    [SerializeField] private bool useRelativeRotation = true;

    private Quaternion _relativeRotation;

    private void Start()
    {
        _relativeRotation = transform.parent.localRotation;
    }

    private void Update()
    {
        if (useRelativeRotation)
        {
            transform.rotation = _relativeRotation;
        }
    }
}
