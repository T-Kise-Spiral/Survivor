using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Suv
{
    public class Billboard : MonoBehaviour
    {
        void Update()
        {
            Vector3 lookPos = Camera.main.transform.position;
            //lookPos.x = transform.position.x;
            lookPos.y = transform.position.y;
            //lookPos.z = transform.position.z;
            transform.LookAt(lookPos);
        }
    }
}