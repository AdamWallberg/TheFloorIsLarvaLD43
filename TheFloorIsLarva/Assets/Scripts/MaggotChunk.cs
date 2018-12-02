using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MaggotChunk : MonoBehaviour
{
	// Physics
	[HideInInspector]public Rigidbody2D _rb;
	public CircleCollider2D _coll;

	// Size n' shit
	public float _maxAmount = 4.0f;
	public float _amount = 4.0f;
	Vector3 _lastFramePos;

	// Movement
	public float _movementSizeReductionRate = 1.0f;
	public float _maxMoveForce = 500.0f;
	public float _maxDragRange = 2.0f;

	// Splitting
	public MaggotChunk _maggotMassPrefab;
	public int _splitCost = 1;
	[HideInInspector]public float _timeAtSplit;
	public float _pushTimeThreshold = 1.0f;

	// Merging
	public bool _merging = false;

	// Visuals
	public SpriteRenderer _renderer;

	private void Awake()
	{
		_rb = GetComponent<Rigidbody2D>();
		_coll = GetComponent<CircleCollider2D>();
		_lastFramePos = transform.position;
	}
	
	void Update ()
	{
		// Update size
		_amount = Mathf.Clamp(_amount, 0.0f, _maxAmount);
		float f = _amount / _maxAmount;
		transform.localScale = new Vector3(f, f, f);

		// Die
		if(_amount <= 1.0f && !_merging)
		{
			Die();
		}

		// Handle movement
		MovementSizeReduction();

		// Interact with other chunks
		Interact();

		// Rotate sprite
		HandleSpriteRotation();
	}

	void MovementSizeReduction()
	{
		if(!_merging)
		{
			Vector3 posDelta = transform.position - _lastFramePos;
			if (posDelta.magnitude > 1.0f)
			{
				posDelta = Vector3.zero;
			}
			_amount -= posDelta.magnitude * _movementSizeReductionRate;
			_lastFramePos = transform.position;
		}
	}

	void Interact()
	{
		foreach (var other in FindObjectsOfType<MaggotChunk>())
		{
			if (other == this)
				continue;

			Vector3 diff = transform.position - other.transform.position;

			if (diff.magnitude <
				other._coll.radius * other.transform.localScale.x +
				_coll.radius * transform.localScale.x)
			{
				if (Time.time - _timeAtSplit < _pushTimeThreshold)
				{
					diff.Normalize();
					_rb.AddForce(diff * Time.deltaTime * 100.0f);
				}
				else
				{
					diff.Normalize();
					_rb.AddForce(-diff * Time.deltaTime * 50.0f);
					if (!other._merging && !_merging)
					{
						other._merging = true;
						_merging = true;
						if(other._amount < _amount)
							other.StartCoroutine(other.Merge(this));
						else
							StartCoroutine(Merge(other));
					}
				}
			}
		}
	}

	void Die()
	{
		Destroy(gameObject);
	}

	public void Split()
	{
		if(_amount > _splitCost + 1.0f)
		{
			MaggotChunk mm = Instantiate(_maggotMassPrefab);
			mm.transform.position = transform.position;
			mm._amount = _splitCost;
			mm._timeAtSplit = _timeAtSplit = Time.time;
			_amount -= _splitCost;
			_rb.AddForce(Vector2.up * 0.1f, ForceMode2D.Impulse);
			mm._rb.AddForce(Vector2.down * 0.1f, ForceMode2D.Impulse);
		}
	}

	public void Drag(Vector2 drag)
	{
		float d = Mathf.Clamp(drag.magnitude, 0.0f, _maxDragRange);
		float f = d / _maxDragRange;

		_rb.AddForce(drag.normalized * f * Time.deltaTime * _maxMoveForce);
	}

	public IEnumerator Merge(MaggotChunk other)
	{
		yield return new WaitForSeconds(0.5f);

		float newTotal = other._amount + _amount;
		const float growRate = 2.0f;

		while(other._amount < newTotal)
		{
			float delta = Time.deltaTime * growRate;
			other._amount += delta;
			_amount -= delta;
			yield return null;
		}
		other._merging = false;
		Destroy(gameObject);
		enabled = false;
	}

	void HandleSpriteRotation()
	{
		Vector3 direction = _rb.velocity.normalized;
		Transform t = _renderer.transform;
		t.up = direction;
	}
}
