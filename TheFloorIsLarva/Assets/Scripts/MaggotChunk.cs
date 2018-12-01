using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MaggotChunk : MonoBehaviour
{
	// Physics
	Rigidbody2D _rb;

	// Size n' shit
	public int _maxAmount = 4;
	public int _amount = 4;

	// Splitting
	public MaggotChunk _maggotMassPrefab;
	public int _splitCost = 1;

	private void Awake()
	{
		_rb = GetComponent<Rigidbody2D>();
	}
	
	void Update ()
	{
		// Update size
		_amount = (int)Mathf.Clamp(_amount, 0.0f, _maxAmount);
		float f = _amount / (float)_maxAmount;
		transform.localScale = new Vector3(f, f, f);

		// Die
		if(_amount <= 0)
		{
			Die();
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
			_amount -= 2;
		}
	}

	public void Drag(Vector2 drag)
	{
		transform.position += (Vector3)drag.normalized * Time.deltaTime;
	}
}
