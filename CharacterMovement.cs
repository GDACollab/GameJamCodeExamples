using UnityEngine;
using System.Collections;

[RequireComponent (typeof(Rigidbody2D))]
public class CharacterMovement : MonoBehaviour
{
	private Rigidbody2D rigidBody;

	[Header("Movement")]
	public float jumpForce = 20.0f;
	public float moveSpeed = 10.0f;

	[Header("Utility")]
	public float groundCheckDistance = 1.0f;

	void Start ()
	{
		rigidBody = GetComponent<Rigidbody2D> ();
	}

	void Update ()
	{
		float horizontalInput = Input.GetAxisRaw ("Horizontal");
		Vector2 moveDirection = new Vector2 (horizontalInput * moveSpeed, 0);
		rigidBody.AddForce (moveDirection);

		//print (isGrounded());

		if(Input.GetButtonDown("Jump") && isGrounded())
		{
			rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpForce);
		}
	}


	bool isGrounded()
	{
		Vector2 groundCheckPos = new Vector2 (transform.position.x, transform.position.y - groundCheckDistance);

		bool result =  Physics2D.Linecast(transform.position, groundCheckPos);

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