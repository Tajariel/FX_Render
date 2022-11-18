using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class WallJump : MonoBehaviour
{
	//[Header("Wall Jump")]
	[Tooltip("Input name used for jumping (From InputManager)")]
	public string wallJumpInputName = "Jump";
	[Tooltip("Force applied when jumping")]
	public Vector3 wallJumpForce = new Vector3(0f, 250f, -350f);
	[Tooltip("Which Layers are considered as ground ?")]
	public LayerMask wallJumpLayerMask = 1;
	[Tooltip("Does the GameObject look towards the other way when wall jumping ? Very useful for consecutive wall jumps")]
	[SerializeField] private bool invertLookDirectionOnWallJump = true;

	//[Header("Friction")]
	[Range(1, 10f)]
	[Tooltip("Slowdown effect applied when wall sliding. 1 = No effect")]
	[SerializeField] private float gravityDampenWhileOnWall = 2f;

	//[Header("Move")]
	[Tooltip("Do we prevent moving for some time after wall Jumping ?")]
	public bool preventMovingAfterWallJump = true;
	[Tooltip("Duration of movement prevention")]
	[SerializeField] private float movePreventionDuration = 0.25f;
	[Tooltip("Reference to the Mover Component")]
	[SerializeField] private Mover mover;

	//[Header("Jump")]
	[Tooltip("Do we reset the jump count on Wall Jump ?")]
	public bool resetJumpCountOnWallJump = true;
	[Tooltip("Reference to the Jumper Component")]
	[SerializeField] Jumper jumper = null;

	//[Header("Collision Check")]
	[Tooltip("Estimated distance between the pivot and the wall when wall jumping")]
	[SerializeField] private float collisionCheckDistance = 0.6f;
	[Tooltip("Minimal height required to perform a wall jump. Helps preventing wall jumps while on the ground.")]
	[SerializeField]
	public float minimalHeightAllowedToWallJump = 1.5f;
	[Tooltip("Offset from the pivot when detecting collision")]
	[SerializeField]
	public Vector3 collisionOffset = new Vector3(0, 0.5f, 0);

	//[Header("FX")]
	[Tooltip("Visual/Sound FX Instantiated on jump")]
	public GameObject jumpFX = null;
	[Tooltip("Offset from the pivot when Instantiating FX")]
	[SerializeField] private Vector3 fxOffset = Vector3.zero;
	[Tooltip("Lifetime of the Instantiated FX. 0 = Do not destroy")]
	[SerializeField] private float timeBeforeDestroyFX = 3f;

	//[Header("Animation")]
	[Tooltip("Reference to the Animator Component")]
	public Animator animator;
	[Tooltip("Name of the Trigger parameter called when wall jumping")]
	public string wallJumpTriggerName = "wallJump";

	private Rigidbody rigid;
	public int inputChoiceIndex;

	private void Update ()
	{
		if (!GroundCheck(transform.forward, collisionCheckDistance)) return;
		
		WallSlide();
		
		if (GroundCheck(Vector3.down, minimalHeightAllowedToWallJump) == false)
		{
			WallJumpCheck();
		}
	}

	private void OnDrawGizmosSelected ()
	{
		Transform transform1 = transform;
		Vector3 position = transform1.position;
		Debug.DrawLine(position + collisionOffset, position + collisionOffset + transform1.forward * (collisionCheckDistance), Color.red);
	}

	private bool GroundCheck (Vector3 dir, float collisionCheckDist)
	{
		RaycastHit hit;

		return Physics.Raycast(transform.position + collisionOffset, dir, out hit, collisionCheckDist, wallJumpLayerMask, QueryTriggerInteraction.Ignore);
	}

	private void WallSlide()
	{
		Vector3 gravityDampen = Physics.gravity - Physics.gravity / gravityDampenWhileOnWall;
		if(rigid.velocity.y < 0)
		{
			rigid.AddForce(gravityDampen * Time.deltaTime * -50f, ForceMode.Force);
		}
	}

	private void WallJumpCheck ()
	{
		if (!Input.GetButtonDown(wallJumpInputName)) return;
		
		if (rigid != null)
		{
			RaycastHit hit;
			if (Physics.Raycast(transform.position + collisionOffset, transform.forward, out hit, collisionCheckDistance, wallJumpLayerMask, QueryTriggerInteraction.Ignore))
			{
				if(jumpFX != null)
				{
					GameObject fx = Instantiate(jumpFX, hit.point, jumpFX.transform.rotation);
					fx.transform.up = hit.normal;

					fx.GetComponent<ParticleSystem>().Play();

					if(timeBeforeDestroyFX > 0)
					{
						Destroy(fx, timeBeforeDestroyFX);
					}
				}
			}

			WallJumpTrigger();
		}
		else
		{
			Debug.LogWarning("No Rigidbody on this GameObject !", gameObject);
		}
	}

	private void WallJumpTrigger ()
	{
		if (animator != null)
		{
			animator.SetTrigger(wallJumpTriggerName);
		}

		if(resetJumpCountOnWallJump && jumper != null)
		{
			jumper.currentJumpAmount = jumper.jumpAmount;
		}

		rigid.velocity = Vector3.zero;
		
		Transform transform1 = transform;
		Vector3 force = transform1.right * wallJumpForce.x + transform1.up * wallJumpForce.y + transform1.forward * wallJumpForce.z;
		rigid.AddForce(force);

		if (invertLookDirectionOnWallJump)
		{
			transform.forward *= -1f;
		}

		if (preventMovingAfterWallJump)
		{
			StartCoroutine(PreventMovement());
		}
	}

	private IEnumerator PreventMovement ()
	{
		mover.canMove = false;

		yield return new WaitForSeconds(movePreventionDuration);

		mover.canMove = true;
	}

	private void Start ()
	{
		InitialRefCheck();
	}

	private void InitialRefCheck ()
	{
		if (rigid == null)
		{
			rigid = GetComponent<Rigidbody>();

			if (rigid == null)
			{
				Debug.LogWarning("No Rigidbody Component found on this GameObject !", gameObject);
			}
		}

		if (resetJumpCountOnWallJump && jumper == null)
		{
			jumper = GetComponent<Jumper>();

			if (jumper == null)
			{
				Debug.LogWarning("No Jumper Component found on this GameObject !", gameObject);
			}
		}

		if (preventMovingAfterWallJump && mover == null)
		{
			mover = GetComponent<Mover>();
			if (mover == null)
			{
				Debug.LogWarning("No Mover Component found on this GameObject !", gameObject);
			}
		}

		if (animator == null)
		{
			animator = GetComponent<Animator>();
		}
	}
}
