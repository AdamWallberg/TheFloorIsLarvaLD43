using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
	public Activator[] _activators;
	bool _open = false;
	bool _openLastFrame;

	public Collider2D _collider;
	public SpriteRenderer _renderer;

	void Update ()
	{
		if (_activators.Length <= 0)
			return;

		_open = true;
		foreach (var a in _activators)
			if (!a.On)
				_open = false;

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
