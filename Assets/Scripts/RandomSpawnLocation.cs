using UnityEngine;

public class RandomSpawnLocation {
	private Transform center;
	private float minRange;
	private float maxRange;

	public RandomSpawnLocation(Transform center, float minRange, float maxRange) {
		this.center = center;
		this.minRange = minRange;
		this.maxRange = maxRange;
	}

	public Vector3 Next() {
		var relativePosition = Random.insideUnitCircle * (maxRange - minRange); 

		var newPoint = this.center.TransformPoint(
			new Vector3(
				(relativePosition.x < 0f ? relativePosition.x - minRange : relativePosition.x + minRange),
				0,
				(relativePosition.y < 0f ? relativePosition.y - minRange : relativePosition.y + minRange)
				)
			);

		
		Debug.Log(string.Format("Point: ({0},{1}) (min: {2}, max: {3}, orig: ({4}, {5}))", newPoint.x, newPoint.z, minRange, maxRange, relativePosition.x, relativePosition.y));

		return newPoint;
	}
}