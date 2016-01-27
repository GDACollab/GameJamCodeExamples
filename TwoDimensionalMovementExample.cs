/*
 * Please note that this character uses the Rigidbody2D and not the CharacterController.
 * Both come with their own strengths and weaknesses:
 * 
 * Rigidbody2D:
 * 	-Comes with Physics
 * 	-Good for sliding around and realistic simulations
 * 	-Has no built in functionality for checking if the player is grounded
 * 	-Use Rigidbody2D.AddForce() to move
 * 
 * CharacterController:
 * 	-Comes with an isGrounded() function
 * 	-Good for percise, designer-controlled, movement
 * 	-Has no built in physics or gravity
 * 	-Uses CharacterController.Move() to move
 * 
 * To set up this script:
 * 	1) Create a GameObject named 'Character'.
 * 	2) Set the Gamobjects layer to 'Ignore Raycast'
 * 	3) Give it a collision component, a Rigidbody2D and this script.
 * 	4) Parent the scene's camera to the Gameobject.
 * 	5) Place the camera so that the character can be seen nicely.
 */

using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Rigidbody2D))]
public class TwoDimensionalMovementExample : MonoBehaviour
{
	private Rigidbody2D rigidBody; // A reference to the Rigidbody2D component on the character.

	[Header("Movement")]
	public float jumpForce = 20.0f; // How high we jump
	public float moveSpeed = 10.0f; // How fast we move side-to-side

	[Header("Utility")]
	public float groundCheckDistance = 1.0f; // How far below the character that we look for ground

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

		rigidBody = GetComponent<Rigidbody2D> ();
	}

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

		float horizontalInput = Input.GetAxis ("Horizontal"); // Get the horizontal input
		Vector2 moveDirection = new Vector2 (horizontalInput * moveSpeed, 0); // Multiply it by our speed
		rigidBody.AddForce (moveDirection); // Actually move the character

		//print (isGrounded()); // Draw the line to check for solid ground

		// Jump if we are told to jump
		if(Input.GetButtonDown("Jump") && isGrounded())
		{
			rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpForce);
		}
	}

	/* This is a custom function because Rigidbody2D does not come with a built in way to check for the ground.
	 * 
	 * It draws a line from the center of the character to the 'groundCheckPosition'.
	 * If that line collides with anything besides the character, we are grounded.
	 * 
	 * To ignore the character, it is imperative that the character's layer is set to 'Ignore Raycast'.
	 */
	bool isGrounded()
	{
		// Establish the position we want to draw the line to:
		Vector2 groundCheckPos = new Vector2 (transform.position.x, transform.position.y - groundCheckDistance);
		// Physics2D.Linecast returns 'true' if it found any collisions.
		bool result =  Physics2D.Linecast(transform.position, groundCheckPos);

		// If we hit anything, draw a green debug line. Otherwise, draw a red debug line.
		// Debug lines will not be seen in the Game View or the final product.
		if (result)
		{
			Debug.DrawLine(transform.position, groundCheckPos, Color.green);
		}
		else
		{
			Debug.DrawLine(transform.position, groundCheckPos, Color.red);
		}

		return result;
	}
}
