using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZZ
{
    public class PlayerLocomotion : MonoBehaviour
    {
        Transform cameraObject;
        InputHandler inputHandler;
        Vector3 moveDirection;

        [HideInInspector]
        public Transform myTransform;
        [HideInInspector]
        public AnimatorHandler animatorHandler;

        public new Rigidbody rigidbody;
        public GameObject normalCamera;

        [Header("Stats")]
        [SerializeField]
        float movementSpeed = 5;
        [SerializeField]
        float rotationSpeed = 10;

        void Start()
        {
            // Log to ensure Start method is being called
            Debug.Log("PlayerLocomotion Start");
            rigidbody = GetComponent<Rigidbody>();
            if (rigidbody == null)
                Debug.LogError("Rigidbody component missing.");

            inputHandler = GetComponent<InputHandler>();
            if (inputHandler == null)
                Debug.LogError("InputHandler component missing.");

            animatorHandler = GetComponent<AnimatorHandler>();
            if (animatorHandler == null)
            {
                Debug.LogError("AnimatorHandler component missing.");
            }
            else
            {
                // Only initialize if the component was found
                animatorHandler.Initialize();
            }

            cameraObject = Camera.main.transform;
            if (cameraObject == null)
                Debug.LogError("Main Camera not found.");
            myTransform = transform;
        }

        public void Update()
        {
            float delta = Time.deltaTime;

            inputHandler.TickInput(delta);

            moveDirection = cameraObject.forward * inputHandler.vertical;
            moveDirection += cameraObject.right * inputHandler.horizontal;
            moveDirection.Normalize();
            moveDirection.y = 0;

            float speed = movementSpeed;
            moveDirection *= speed;

            Vector3 projectedVelocity = Vector3.ProjectOnPlane(moveDirection, normalVector);
            rigidbody.velocity = projectedVelocity;

            animatorHandler.UpdateAnimatorValues(inputHandler.moveAmount, 0);

            if (animatorHandler != null && animatorHandler.canRotate)
            {
                HandleRotation(delta);
            }
        }

        #region Movement
        Vector3 normalVector;
        Vector3 targetPosition;

        private void HandleRotation(float delta)
        {
            Vector3 targetDir = Vector3.zero;
            float MoveOverride = inputHandler.moveAmount;

            targetDir = cameraObject.forward * inputHandler.vertical;
            targetDir += cameraObject.right * inputHandler.horizontal;

            targetDir.Normalize();
            targetDir.y = 0;

            if (targetDir == Vector3.zero)
            {
                targetDir = myTransform.forward;
            }

            float rs = rotationSpeed;

            Quaternion tr = Quaternion.LookRotation(targetDir);
            Quaternion targetRotation = Quaternion.Slerp(myTransform.rotation, tr, rs * delta);

            myTransform.rotation = targetRotation;
        }

        #endregion

    }
}
