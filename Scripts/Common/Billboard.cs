using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Suv
{
    public class Billboard : MonoBehaviour
    {
        void Update()
        {
            transform.LookAt(Camera.main.transform.position);
        }
    }
}