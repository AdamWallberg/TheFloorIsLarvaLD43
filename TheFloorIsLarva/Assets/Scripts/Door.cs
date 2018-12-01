using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
	public Activator _activator;
	bool _open = false;
	bool _openLastFrame;

	public Collider2D _collider;
	public SpriteRenderer _renderer;

	void Update ()
	{
		if (!_activator)
			return;

		_open = _activator.On;

		if(_open && !_openLastFrame) // Open
		{
			_collider.enabled = false;
			_renderer.enabled = false;
		}
		else if(!_open && _openLastFrame) // Close
		{
			_collider.enabled = true;
			_renderer.enabled = true;
		}

		_openLastFrame = _open;
	}
}
