using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : Activator
{
	public BoxCollider2D _collider;
	public LayerMask _affectors;

	private void FixedUpdate()
	{
		Vector3 pos = 
			transform.position + 
			(Vector3)_collider.offset * 
			transform.localScale.x;
		Vector2 size = _collider.size * transform.localScale.x;

		var c = Physics2D.OverlapBox(pos, size, 0.0f, _affectors);
		Value = c != null ? 1.0f : 0.0f;
	}
}
