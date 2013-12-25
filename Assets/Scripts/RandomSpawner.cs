using UnityEngine;

public class RandomSpawner {
	private Transform center;
	private float minRange;
	private float maxRange;

	public RandomSpawner(Transform center, float minRange, float maxRange) {
		this.center = center;
		this.minRange = minRange;
		this.maxRange = maxRange;
	}

	public Vector3 Next() {
		var relativePosition = Random.insideUnitCircle * (maxRange - minRange); 
		relativePosition = relativePosition.normalized * (relativePosition.magnitude + minRange);

		var newPoint = this.center.TransformPoint(
			new Vector3(
				relativePosition.x,
				0,
				relativePosition.y
				)
			);

		return newPoint;
	}

	public GameObject Spawn(GameObject original) {
		var location = this.Next();
		
		var newObject = (GameObject)GameObject.Instantiate(original);
		newObject.transform.position = new Vector3(
			location.x,
			newObject.transform.position.y,
			location.z
			);

		return newObject;
	}
}