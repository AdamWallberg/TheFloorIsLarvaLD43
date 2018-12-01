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
	public float _movementSizeReductionRate = 1.0f;

	// Splitting
	public MaggotChunk _maggotMassPrefab;
	public int _splitCost = 1;
	[HideInInspector]public float _timeAtSplit;
	public float _pushTimeThreshold = 1.0f;

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
		if(_amount <= 0)
		{
			Die();
		}

		// Handle movement
		MovementSizeReduction();

		// Push away from other masses
		PushAway();
	}

	void MovementSizeReduction()
	{
		Vector3 posDelta = transform.position - _lastFramePos;
		_amount -= posDelta.magnitude * _movementSizeReductionRate;
		_lastFramePos = transform.position;
	}

	void PushAway()
	{
		if(Time.time - _timeAtSplit < _pushTimeThreshold)
		{
			foreach(var mc in FindObjectsOfType<MaggotChunk>())
			{
				if (mc == this)
					continue;

				Vector3 diff = transform.position - mc.transform.position;

				if (diff.magnitude < 
					mc._coll.radius * mc.transform.localScale.x + 
					_coll.radius * transform.localScale.x)
				{
					diff.Normalize();
					_rb.AddForce(diff * Time.deltaTime * 100.0f);
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
		if(_amount > _splitCost)
		{
			MaggotChunk mm = Instantiate(_maggotMassPrefab);
			mm.transform.position = transform.position;
			mm._amount = 2;
			mm._timeAtSplit = _timeAtSplit = Time.time;
			_amount -= 2;
			_rb.AddForce(Vector2.up * 1.0f, ForceMode2D.Impulse);
			mm._rb.AddForce(Vector2.down * 1.0f, ForceMode2D.Impulse);
		}
	}

	public void Drag(Vector2 drag)
	{
		_rb.AddForce(drag * Time.deltaTime * 100.0f);
	}
}
