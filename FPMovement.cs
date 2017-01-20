using UnityEngine;
using System.Collections;

[RequireComponent (typeof (CharacterController))]
public class FPMovement : MonoBehaviour
{
	private Transform mainCam;
	private CharacterController characterController;

	private float mouseX = 0.0f;
	private float mouseY = 0.0f;
	private Vector3 moveDirection = Vector3.zero;

	[Header("Mouse Sensitivity")]
	public float mouseSpeedX = 2.0f;
	public float mouseSpeedY = 2.0f;

	[Header("Movement")]
	public float moveSpeed = 6.0f;
	public float jumpSpeed = 8.0f;

	[Header("Gravity")]
	public float gravity = 20.0f;

	void Start ()
	{
		mainCam = Camera.main.transform;
		characterController = this.GetComponent<CharacterController>();
	}

	void Update ()
	{
		// MOUSE

		if(Input.GetButtonDown("Fire1"))
		{
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}

		mouseX += mouseSpeedX * Input.GetAxis("Mouse X");
		transform.eulerAngles = new Vector3(0.0f, mouseX, 0.0f);

		mouseY -= mouseSpeedY * Input.GetAxis("Mouse Y");
		mouseY = Mathf.Clamp (mouseY, -90, 90);

		mainCam.transform.eulerAngles = new Vector3(mouseY, mainCam.eulerAngles.y, 0.0f);

		// MOVEMENT

		if(characterController.isGrounded)
		{
			moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
			moveDirection = transform.TransformDirection(moveDirection);
			moveDirection *= moveSpeed;

			RaycastHit hitInfo;
			Physics.SphereCast(transform.position, characterController.radius, Vector3.down, out hitInfo, characterController.height/2f);
			moveDirection = Vector3.ProjectOnPlane(moveDirection, hitInfo.normal);

			if (Input.GetButton("Jump"))
				moveDirection.y = jumpSpeed;
		}

		// GRAVITY

		moveDirection.y -= gravity * Time.deltaTime;
		characterController.Move(moveDirection * Time.deltaTime);
	}
}