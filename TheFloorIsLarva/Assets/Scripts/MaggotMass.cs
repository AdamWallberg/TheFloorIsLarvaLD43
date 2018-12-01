using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MaggotMass : MonoBehaviour
{
	// Physics
	Rigidbody2D _rb;

	// Size n' shit
	public int _maxAmount = 8;
	public int _amount = 8;

	// Splitting
	public MaggotMass _maggotMassPrefab;
	public int _splitCost = 2;

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
	}

	public void Split()
	{
		if(_amount >= _splitCost)
		{
			MaggotMass mm = Instantiate(_maggotMassPrefab);
			mm.transform.position = transform.position;
			mm._amount = 2;
			_amount -= 2;
		}
	}
}
