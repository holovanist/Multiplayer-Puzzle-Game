using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace NewMovment
{
    public class MoveCamera : NetworkBehaviour
    {
        public Transform cameraPosition;
        void Update()
        {
            if (!IsOwner) return;
            transform.position = cameraPosition.position;
        }
    }
}