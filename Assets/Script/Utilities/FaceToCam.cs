using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceToCam : MonoBehaviour
{
    Transform target;
    private void Start() {
        target = Camera.main.transform;
    }
    public void SetTarget(Transform _target){
        target = _target;
    }
    // Update is called once per frame
    void Update()
    {
        transform.LookAt(target);
    }
}
