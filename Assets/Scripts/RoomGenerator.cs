using System.Collections;
using System.Collections.Generic;
using ObjectPool;
using UnityEngine;

public class RoomGenerator : Singleton<RoomGenerator>
{

	public Transform PlayerTransform;
	
	public GameObject[] availableRooms;
	public List<GameObject> currentRooms;
	private float screenWidthInPoints;
	
	public GameObject[] availableObjects;
	public List<GameObject> objects;

	public float objectsMinDistance = 5.0f;
	public float objectsMaxDistance = 10.0f;

	public float objectsMinY = -2.8f;
	public float objectsMaxY = 2.8f;

	public float objectsMinRotation = -45.0f;
	public float objectsMaxRotation = 45.0f;

	private Coroutine mainCoroutine;

	// Use this for initialization
	void Start () {
		float height = 2.0f * Camera.main.orthographicSize;
		screenWidthInPoints = height * Camera.main.aspect;
	}
	
	// Update is called once per frame
	void Update () {
	}

	private void AddRoom(float farthestRoomEndX)
	{
		int randomIndex = Random.Range(0, availableRooms.Length);
		GameObject room = PoolManager.SpawnObject(availableRooms[randomIndex]);
		float roomWidth = room.transform.Find("floor").localScale.x;
		float roomCenter = farthestRoomEndX + roomWidth * 0.5f;
		room.transform.position = new Vector3(roomCenter, 0, 0);
		currentRooms.Add(room);
	}
	
	void AddObject(float lastObjectX)
	{
		int randomIndex = Random.Range(0, availableObjects.Length);
		if (GameData.Instance.Difficulty == 2)
		{
			while (availableObjects[randomIndex].tag == "Health")
				randomIndex = Random.Range(0, availableObjects.Length);
		}
		GameObject obj = PoolManager.SpawnObject(availableObjects[randomIndex]);
		float objectPositionX = lastObjectX + Random.Range(objectsMinDistance, objectsMaxDistance);
		float randomY = Random.Range(objectsMinY, objectsMaxY);
		obj.transform.position = new Vector3(objectPositionX,randomY,0); 
		//float rotation = Random.Range(objectsMinRotation, objectsMaxRotation);
		//obj.transform.rotation = Quaternion.Euler(Vector3.forward * rotation);
		objects.Add(obj);            
	}

	private void GenerateRoomIfRequired()
	{
		List<GameObject> roomsToRemove = new List<GameObject>();
		bool addRooms = true;
		float playerX = PlayerTransform.position.x;
		float removeRoomX = playerX - screenWidthInPoints;
		float addRoomX = playerX + screenWidthInPoints;
		float farthestRoomEndX = 0;
		foreach (var room in currentRooms)
		{
			float roomWidth = room.transform.Find("floor").localScale.x;
			float roomStartX = room.transform.position.x - (roomWidth * 0.5f);
			float roomEndX = roomStartX + roomWidth;
			if (roomStartX > addRoomX)
			{
				addRooms = false;
			}
			if (roomEndX < removeRoomX)
			{
				roomsToRemove.Add(room);
			}
			farthestRoomEndX = Mathf.Max(farthestRoomEndX, roomEndX);
		}
		foreach (var room in roomsToRemove)
		{
			currentRooms.Remove(room);
			PoolManager.ReleaseObject(room);
		}
		if (addRooms)
		{
			AddRoom(farthestRoomEndX);
		}
	}
	
	void GenerateObjectsIfRequired()
	{
		float playerX = PlayerTransform.position.x;
		float removeObjectsX = playerX - screenWidthInPoints;
		float addObjectX = playerX + screenWidthInPoints;
		float farthestObjectX = 0;
		List<GameObject> objectsToRemove = new List<GameObject>();
		foreach (var obj in objects)
		{
			float objX = obj.transform.position.x;
			farthestObjectX = Mathf.Max(farthestObjectX, objX);
			if (objX < removeObjectsX) 
			{           
				objectsToRemove.Add(obj);
			}
		}
		foreach (var obj in objectsToRemove)
		{
			objects.Remove(obj);
			PoolManager.ReleaseObject(obj);
		}
		if (farthestObjectX < addObjectX)
		{
			AddObject(farthestObjectX);
		}
	}

	private IEnumerator GeneratorCheck()
	{
		var room = PoolManager.SpawnObject(availableRooms[0]);
		currentRooms.Add(room);
		while (!GameController.Instance.IsEnd)
		{
			if(!GameController.Instance.InMainMenu)
				GenerateObjectsIfRequired();
			GenerateRoomIfRequired();
			yield return new WaitForSeconds(0.25f);
		}
	}

	public void ClearScene()
	{
		//StopCoroutine(mainCoroutine);
		foreach (var room in currentRooms)
		{
			PoolManager.ReleaseObject(room);
		}
		currentRooms.Clear();
		foreach (var generatedObject in objects)
		{
			PoolManager.ReleaseObject(generatedObject);
		}
		objects.Clear();
	}

	public void StartGenerate()
	{
		mainCoroutine = StartCoroutine(GeneratorCheck());
	}
}
