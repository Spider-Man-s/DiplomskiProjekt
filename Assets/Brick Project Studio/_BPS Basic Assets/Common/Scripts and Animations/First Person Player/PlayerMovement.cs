using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SojaExiles

{
    public class PlayerMovement : MonoBehaviour
    {

        public CharacterController controller;

        public float speed = 5f;
        public float gravity = -15f;

        Vector3 velocity;
        Vector2 moveInput;

        

        
        void Update()
        {

            Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
            controller.Move(move * speed * Time.deltaTime);

            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);

        }
        public void OnMove(InputAction.CallbackContext context)
        {
            moveInput = context.ReadValue<Vector2>();
        }
    }
}