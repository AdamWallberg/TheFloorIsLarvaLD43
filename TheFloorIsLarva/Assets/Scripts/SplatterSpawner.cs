using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: Deal with death
public class SplatterSpawner : MonoBehaviour
{
	public SpriteRenderer[] _splatters;
	public float _minDist = 0.3f;
	public float _maxDist = 1.0f;
	public float _randomizeScale = 0.5f;
	public float _scaleMultiplier = 1.0f;
	public float _randomizePosition = 0.0f;
	float _distance;
	float _distanceTraveled;
	float _movementDelta;
	Vector3 _lastPos;

	public float _despawnTime = 5.0f;
	public float _despawnRate = 5.0f;

	void Start ()
	{
		_lastPos = transform.position;
		RecalculateDistance();
	}
	
	void Update ()
	{
		_movementDelta = (_lastPos - transform.position).magnitude;

		_distanceTraveled += _movementDelta;

		if(_distanceTraveled > _distance)
		{
			var r = Instantiate(_splatters[Random.Range(0, _splatters.Length)]);
			r.transform.position = transform.position + 
				new Vector3(
					Random.Range(-_randomizePosition, _randomizePosition),
					Random.Range(-_randomizePosition, _randomizePosition), 
					0.0f) * transform.localScale.x;
			r.transform.Rotate(Vector3.forward, Random.Range(0.0f, 360.0f));
			r.transform.localScale = 
				transform.localScale * 
				Random.Range(1.0f - _randomizeScale, 1.0f + _randomizeScale) *
				_scaleMultiplier;
			StartCoroutine(Fade(r));
			_distanceTraveled = 0.0f;
			RecalculateDistance();
		}

		_lastPos = transform.position;
	}

	void RecalculateDistance()
	{
		_distance = Random.Range(_minDist, _maxDist) * transform.localScale.x;
	}

	IEnumerator Fade(SpriteRenderer r)
	{
		yield return new WaitForSeconds(_despawnTime);

		float alpha = r.color.a;
		for(float f = 1.0f; f > 0.0f; f -= Time.deltaTime / _despawnRate)
		{
			float a = f * alpha;
			r.color = new Color(r.color.r, r.color.g, r.color.b, a);
			yield return null;
		}

		Destroy(r.gameObject);
	}
}
