using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZZ
{
    public class InputHandler : MonoBehaviour
    {
        public float horizontal;
        public float vertical;
        public float moveAmount;
        public float mouseX;
        public float mouseY;

        public bool LightAttackInput;
        public bool HeavyAttackInput;

        PlayerControl inputActions;
        CameraHandler cameraHandler;
        AnimatorHandler animatorHandler;

        Vector2 movementInput;
        Vector2 cameraInput;

        private void Awake()
        {
            cameraHandler = CameraHandler.singleton;

            animatorHandler = GetComponent<AnimatorHandler>();
        }

        private void FixedUpdate()
        {
            float delta = Time.deltaTime;

            if (cameraHandler != null )
            {
                cameraHandler.FollowTarget(delta);
                cameraHandler.HandleCameraRotation(delta,mouseX,mouseY);
            }
        }

        public void OnEnable()
        {
            if (inputActions == null)
            {
                inputActions = new PlayerControl();
                inputActions.PlayerMovement.Movement.performed += inputActions => movementInput = inputActions.ReadValue<Vector2>();
                inputActions.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();

            }

            inputActions.Enable();
        }

        private void OnDisable()
        {
            inputActions.Disable();
        }

        public void TickInput(float delta)
        {
            MoveInput(delta);
            AttackInput(delta);
        }

        private void MoveInput(float delta)
        {
            horizontal = movementInput.x;
            vertical = movementInput.y;
            moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
            mouseX = cameraInput.x;
            mouseY = cameraInput.y;
        }

        private void AttackInput(float delta)
        {
            inputActions.PlayerAction.LightAttack.performed += i => LightAttackInput = true;
            inputActions.PlayerAction.HeavyAttack.performed += i => HeavyAttackInput = true;

            if(LightAttackInput)
            {
                animatorHandler.animator.SetTrigger("LightAttack");
                LightAttackInput = false;
            }

            if(HeavyAttackInput)
            {
                animatorHandler.animator.SetTrigger("HeavyAttack");
                HeavyAttackInput = false;
            }
        }
    }

}