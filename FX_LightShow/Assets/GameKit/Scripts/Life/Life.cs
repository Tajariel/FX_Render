using UnityEngine;
using UnityEngine.Events;

public class Life : MonoBehaviour
{
	public int startLife = 10;
	public int maxLife = 10;

	public int currentLife = 10;

	public UnityEvent onDamageTaken;
	public UnityEvent onDeath;

	public bool displayDebugInfo;

	public int CurrentLife
	{
		get => currentLife;
		set => currentLife = Mathf.Clamp(value, 0, maxLife);
	}

	[Range(0f, 5f)]
	public float invincibilityDuration = 0.25f;

	private float invTimer = 0f;

	public Animator animator;
	[SerializeField] private string hitParameterName = "Hit";

	private void Awake ()
	{
		CurrentLife = startLife;
		if(maxLife < startLife)
		{
			maxLife = startLife;
		}

		if(animator == null)
		{
			animator = GetComponentInChildren<Animator>();
		}
	}

	private void Update ()
	{
		if(invTimer < invincibilityDuration)
		{
			invTimer += Time.deltaTime;
		}
	}

	public delegate void OnDmgTaken();

	public OnDmgTaken lifeChangeDelegate;
	
	public void ModifyLife(int lifeMod)
	{
		if(lifeMod < 0)
		{
			if(invTimer >= invincibilityDuration)
			{
				if(animator != null)
				{
					animator.SetTrigger(hitParameterName);
				}

				currentLife += lifeMod;
				lifeChangeDelegate?.Invoke();
				onDamageTaken.Invoke();

				invTimer = 0f;
			}
		}
		else
		{
			currentLife += lifeMod;
			onDamageTaken.Invoke();
			lifeChangeDelegate?.Invoke();
		}

		if(currentLife <= 0)
		{
			LoadSceneOnDestroy sceneOnDestroy = GetComponent<LoadSceneOnDestroy>();
			if (sceneOnDestroy != null)
			{
				sceneOnDestroy.LoadScene();
			}

			onDeath.Invoke();
		}
		else if(currentLife > maxLife)
		{
			currentLife = maxLife;
		}
	}

}
