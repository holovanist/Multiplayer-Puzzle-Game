using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

namespace NewMovment
{
    //https://youtu.be/SsckrYYxcuM?si=N6UB4xPPwDyvYx0h&t=258
    public class Sliding : MonoBehaviour
    {
        [Header("References")]
        public Transform orientation;
        public Transform PlayerObj;
        private PlayerMovement1 pm;
        private Rigidbody rb;
        private StarterAssetsInputs it;

        [Header("Sliding")]
        public float maxSlideTime;
        public float slideForce;
        private float slideTimer;

        public float slideYScale;
        private float StartYScale;

        private float horizontalInput;
        private float verticalInput;

        private bool wasSliding;



        void Start()
        {
            pm = GetComponent<PlayerMovement1>();
            rb = GetComponent<Rigidbody>();
            it = GetComponent<StarterAssetsInputs>();

            StartYScale = PlayerObj.localScale.y;
        }

        void Update()
        {
            horizontalInput = it.move.x;
            verticalInput = it.move.y;

            if (it.Slide && (horizontalInput != 0 || verticalInput != 0) && !pm.sliding && !wasSliding && !it.jump && !pm.jumping)
            {
                StartSlide();
                wasSliding = true;
            }
            if (!it.Slide && pm.sliding || it.jump && pm.sliding)
            {
                StopSlide();
                
            }
            if(!it.Slide) wasSliding = false;
        }

        private void FixedUpdate()
        {
            if (pm.sliding)
                SlidingMovement();
        }

        private void StartSlide()
        {
            pm.sliding = true;

            PlayerObj.localScale = new Vector3(PlayerObj.localScale.x, slideYScale, PlayerObj.localScale.z);
            rb.AddForce(Vector3.down * 2f, ForceMode.Impulse);

            slideTimer = maxSlideTime;
        }

        private void SlidingMovement()
        {
            Vector3 inputDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

            if(!pm.OnSlope() || rb.linearVelocity.y > -0.1f)
            {
            rb.AddForce(inputDir.normalized * slideForce, ForceMode.Force);

            slideTimer -= Time.deltaTime;
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);


            }
            else
            {
                rb.AddForce(pm.GetSlopeMoveDirection(inputDir) * slideForce, ForceMode.Force);
            }

            if (slideTimer <= 0)
                StopSlide();
        }

        private void StopSlide()
        {
            pm.sliding = false;
            PlayerObj.localScale = new Vector3(PlayerObj.localScale.x, StartYScale, PlayerObj.localScale.z);
        }






    }
}