/*
 * If you want a First Person in your game, you'll probably want to use the Unity default,
 * 	but this is a simpler version for the purpose of learning.
 * 
 * To set up:
 * 	1) Create a GameObject named 'Character'
 *  2) Give it a collision component, a CharacterController and this script.
 * 	3) Parent the camera with the tag 'Main Camera' to the Gameobject.
 * 	4) Place the camera where the character's face would be.
 */

using UnityEngine;
using System.Collections;

[RequireComponent (typeof (CharacterController))]
public class FirstPersonMovementExample : MonoBehaviour
{
	private Transform mainCam; // reference to our 'Main Camera'
	private CharacterController characterController; // reference to our character controller

	private float mouseX = 0.0f; // Horizontal mouse input
	private float mouseY = 0.0f; // Vertical mouse input
	private Vector3 moveDirection = Vector3.zero; // Horizontal & vertical input

	[Header("Mouse Sensitivity")]
	public float mouseSpeedX = 2.0f; // Horizontal mouse sensitivity
	public float mouseSpeedY = 2.0f; // Vertical mouse sensitivity

	[Header("Movement")]
	public float moveSpeed = 6.0f; // Movement speed
	public float jumpSpeed = 8.0f; // Power of jump

	[Header("Gravity")]
	public float gravity = 20.0f; // Power of gravity

	// This function runs at the beginning of a level.
	void Start ()
	{
		/*
		 * REFERENCING OBJECTS
		 * 
		 * Every gameobject is made up of one or more components (ex. Transform, CharacterController, SpriteRenderer)
		 * 
		 * If you want to access a compoenent you must use this function:
		 * 	'Gameobject' . GetComponent< 'NameOfComponent' >();
		 * 
		 * To get the Character Controller on the same Gameobject that this script is on, we can say:
		 * 	this.GetComponent<CharacterController>();
		 * 
		 * Let's say we wanted to reference a Gameobject other than the one this script is attached to.
		 * To get a reference to the other Gameobject, we can say:
		 * 	Gameobject.Find(" Name of Gameobect ");
		 * 
		 * So to get a reference to the GameObject named 'Test', we can say:
		 * 	Gameobject.Find("Test");
		 * 
		 * So if we want the SpriteRenderer on the Gameobject called 'Test2', we can say:
		 * 	Gameobject.Find("Test2").GetComponent<SpriteRenderer>();
		 */

		mainCam = Camera.main.transform; // Get reference to object with tag 'Main Camera'
		characterController = this.GetComponent<CharacterController>(); // Get CharacterController Component
	}

	// This function runs every frame.
	void Update ()
	{
		/*
		 * GETTING INPUT
		 * 
		 * Unity has 3 ways of collecting input:
		 * 
		 * 1) Input.GetKeyDown(" Name of Key ")
		 * 		Does something when the key is clicked.
		 * 2) Input.GetKey(" Name of Key ");
		 * 		Does something while the key is held down.
		 * 3) Input.GetKeyUp(" Name of Key ");
		 * 		Does something when the key is released.
		 * 
		 * Each of these returns a bool. To do something when the 'h' key is pressed, we can say:
		 * 	if (Input.GetKeyDown("h"))
		 * 	{
		 * 		// Do something.
		 * 		print("h key pressed");
		 * 	}
		 * 
		 * If you navigate in Unity to 'Edit > ProjectSettings > Input' you can find what are called Buttons and Axes.
		 * 
		 * A Button is kind of like a virtual key that we can set up.
		 * Instead of saying GetKeyDown("Space") OR GetKeyDown("UpArrow") OR GetKeyDown("JoystickButton0")...
		 * What we can do is setup one button called "Jump" that responds to "Space", "UpArrow" and "JoystickButton0".
		 * Now if we want to jump, all we have to say is:
		 * 	if(Input.GetButtonDown("Jump"))
		 * 	{
		 * 		// Do something.
		 * 		print("jump button pressed");
		 * 	}
		 * 
		 * Lastly, we can set up Axes as well as Buttons.
		 * An Axis has negative keys and positive keys.
		 * When the negative keys are pressed, it returns '-1' and when the positive keys are pressed, it returns '1'.
		 * 
		 * Unity has some built in Axes, one of which is called "Horizontal".
		 * Horizontal returns '-1' if 'a' if the left arrow key is pressed and '1' if 'd' or the right arrow key is pressed.
		 * Axes are generally used for movement and mouse control.
		 */

		mouseX += mouseSpeedX * Input.GetAxis("Mouse X"); // Get horizontal mouse movement
		mouseY -= mouseSpeedY * Input.GetAxis("Mouse Y"); // Get vertical mouse movement
		mouseY = Mathf.Clamp (mouseY, -90, 90); // Do not let the player do sick flips

		transform.eulerAngles = new Vector3(0.0f, mouseX, 0.0f); // Rotate the character based on horizontal mouse input
		mainCam.transform.eulerAngles = new Vector3(mouseY, mainCam.transform.eulerAngles.y, 0.0f); // Rotate the camera based on vertical mosue input

		// CharacterController comes with a built in bool 'isGrounded'!!
		if(characterController.isGrounded)
		{
			// Combine the WASD and Arrow Key input into one Vector3.
			moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
			// Move relative to where the camera is looking, so if the camera is facing left, pressing UP will move the character left.
			moveDirection = transform.TransformDirection(moveDirection);
			moveDirection *= moveSpeed;

			// This is a relatively complicated set of lines. Feel free to ignore it.
			// The first 2 lines check below the character to get information about what we are standing on.
			// Because we are moving relative to where are camera is facing, if you click 'W' are looking up, the character will try to move upwards.
			// When the player clicks 'W', we want the character to move forwards, not upwards.
			// So ProjectOnPlane takes our movement input, and projects it to what we are standing on.
			RaycastHit hitInfo;
			Physics.SphereCast(transform.position, characterController.radius, Vector3.down, out hitInfo, characterController.height/2f);
			moveDirection = Vector3.ProjectOnPlane(moveDirection, hitInfo.normal);


			if (Input.GetButton("Jump"))
				moveDirection.y = jumpSpeed;
		}

		// CharacterController does not come with gravity by default.
		moveDirection.y -= gravity * Time.deltaTime;
		characterController.Move(moveDirection * Time.deltaTime);

		// When we click on the screen, lock the cursor so that we cannot accidentily click off of our application
		// If you can still see the cursor in the center of the screen, try activating 'Maximize on Play'
		// You can click 'Escape' to get the cursor back.
		if(Input.GetButtonDown("Fire1"))
		{
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}
	}
}
