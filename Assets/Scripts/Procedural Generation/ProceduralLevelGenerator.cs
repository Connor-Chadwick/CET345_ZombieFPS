using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ProceduralLevelGenerator : MonoBehaviour
{
	NavMeshSurface[] navMeshSurfaces;
	public Room startRoomPrefab, endRoomPrefab;
	public List<Room> roomPrefabs = new List<Room>();
	public Vector2 iterationRange = new Vector2(3, 10);
	public int roomCount = 0;
	List<Doorway> availableDoorways = new List<Doorway>();

	StartRoom startRoom;
	EndRoom endRoom;
	List<Room> placedRooms = new List<Room>();

	public LayerMask roomLayerMask;
	public GameObject eleDoor;
	public IEnumerator cachedCoro;
	private GameObject tempCurrentRoom;
	public bool newBool = false;
	GameController gc;
	public GameObject testAI;
	public GameObject zombie;

	/*
	void OnGUI()
	{
		int w = Screen.width, h = Screen.height;

		GUIStyle style = new GUIStyle();

		Rect rect = new Rect(0, 0, w, h * 2 / 100);
		style.alignment = TextAnchor.UpperLeft;
		style.fontSize = h * 2 / 50;
		style.normal.textColor = new Color(0.0f, 0.0f, 0.5f, 1.0f);
		string text = roomCount.ToString();
		GUI.Label(rect, text, style);
	}
	*/

	private void Awake()
	{
		gc = FindObjectOfType<GameController>();
		
	}
	void Start()
	{
		StartCoroutine("GenerateLevel");
		//cachedCoro = GenerateLevel();

		//eleDoor = GameObject.Find("eledoor");
		//roomLayerMask = LayerMask.GetMask("Room");
		//StartCoroutine(GenerateLevel());

		//StartCoroutine(cachedCoro);
	}

	IEnumerator GenerateLevel()
	{
		WaitForSeconds startup = new WaitForSeconds(1);
		WaitForFixedUpdate interval = new WaitForFixedUpdate();

		yield return startup;

		// Place start room
		PlaceStartRoom();
		yield return interval;

		// Random iterations
		int iterations = Random.Range((int)iterationRange.x, (int)iterationRange.y);

		for (int i = 0; i < iterations; i++)
		{
			// Place random room from list
			PlaceRoom();
			yield return interval;
			//yield return new WaitForSeconds(0.1f);
		}

		// Place end room
		PlaceEndRoom();
		yield return interval;

		// Level generation finished
		Debug.Log("Level generation finished");

		//Bake navmesh
		foreach (Room room in placedRooms)
		{
			NavMeshSurface[] roomObjs = room.GetComponentsInChildren<NavMeshSurface>();
			foreach (NavMeshSurface roomObj in roomObjs)
			{
				if (roomObj.gameObject.activeSelf)
				{
					roomObj.transform.GetComponent<NavMeshSurface>().BuildNavMesh();
				}
			}
		}



		//Instantiate(testAI, startRoom.transform.position, startRoom.transform.rotation);

		ProcGenSpawnPoint[] spawns = FindObjectsOfType<ProcGenSpawnPoint>();
		foreach (ProcGenSpawnPoint a in spawns)
		{
			GameObject zombie1 = Instantiate(zombie, a.transform.position, a.transform.rotation);
			zombie1.GetComponent<ZombieAI>().enabled = true;
		}

		gc.OpenDoor();

	}


	void PlaceStartRoom()
	{
		// Instantiate room
		startRoom = Instantiate(startRoomPrefab) as StartRoom;
		startRoom.transform.parent = this.transform;

		// Get doorways from current room and add them randomly to the list of available doorways
		AddDoorwaysToList(startRoom, ref availableDoorways);

		// Position room
		startRoom.transform.position = new Vector3(10,-4,98);
		startRoom.transform.rotation = Quaternion.Euler(0,90,0);
	}

	void AddDoorwaysToList(Room room, ref List<Doorway> list)
	{
		foreach (Doorway doorway in room.doorways)
		{
			int r = Random.Range(0, list.Count);
			list.Insert(r, doorway);
		}
	}

	void PlaceRoom()
	{
		// Instantiate room
		Room currentRoom = Instantiate(roomPrefabs[Random.Range(0, roomPrefabs.Count)]) as Room;
		currentRoom.transform.parent = this.transform;
		tempCurrentRoom = currentRoom.gameObject;

		// Create doorway lists to loop over
		List<Doorway> allAvailableDoorways = new List<Doorway>(availableDoorways);
		List<Doorway> currentRoomDoorways = new List<Doorway>();
		AddDoorwaysToList(currentRoom, ref currentRoomDoorways);

		// Get doorways from current room and add them randomly to the list of available doorways
		AddDoorwaysToList(currentRoom, ref availableDoorways);

		bool roomPlaced = false;

		// Try all available doorways
		foreach (Doorway availableDoorway in allAvailableDoorways)
		{
			// Try all available doorways in current room
			foreach (Doorway currentDoorway in currentRoomDoorways)
			{
				// Position room
				PositionRoomAtDoorway(ref currentRoom, currentDoorway, availableDoorway);
				Physics.SyncTransforms();
				// Check room overlaps
				if (CheckRoomOverlap(currentRoom))
				{
					continue;
				}

				roomPlaced = true;

				// Add room to list
				placedRooms.Add(currentRoom);
				roomCount++;

				// Remove occupied doorways
				currentDoorway.gameObject.SetActive(false);
				availableDoorways.Remove(currentDoorway);

				availableDoorway.gameObject.SetActive(false);
				availableDoorways.Remove(availableDoorway);

				// Exit loop if room has been placed
				break;
			}

			// Exit loop if room has been placed
			if (roomPlaced)
			{
				break;
			}
		}

		// Room couldn't be placed. Restart generator and try again
		if (!roomPlaced)
		{
			
			//Destroy(currentRoom.gameObject);
			//ResetLevelGenerator();
			//roomCount = 0;
		}
	}



	void PositionRoomAtDoorway(ref Room room, Doorway roomDoorway, Doorway targetDoorway)
	{
		// Reset room position and rotation
		room.transform.position = Vector3.zero;
		room.transform.rotation = Quaternion.identity;

		// Rotate room to match previous doorway orientation
		Vector3 targetDoorwayEuler = targetDoorway.transform.eulerAngles;
		Vector3 roomDoorwayEuler = roomDoorway.transform.eulerAngles;
		float deltaAngle = Mathf.DeltaAngle(roomDoorwayEuler.y, targetDoorwayEuler.y);
		Quaternion currentRoomTargetRotation = Quaternion.AngleAxis(deltaAngle, Vector3.up);
		room.transform.rotation = currentRoomTargetRotation * Quaternion.Euler(0, 180f, 0);

		// Position room
		Vector3 roomPositionOffset = roomDoorway.transform.position - room.transform.position;
		room.transform.position = targetDoorway.transform.position - roomPositionOffset;
	}
	
	bool CheckRoomOverlap(Room room)
	{
		Bounds bounds = room.RoomBounds;
		//bounds.center = room.transform.position;
		bounds.Expand(-0.08f);

		Collider[] colliders = Physics.OverlapBox(room.transform.position, bounds.size / 2, room.transform.rotation, roomLayerMask);
		if (colliders.Length > 0)
		{
			// Ignore collisions with current room
			foreach (Collider c in colliders)
			{
				if (c.transform.parent.gameObject.Equals(room.gameObject))
				{
					continue;
				}
				else
				{
					
					
					Debug.LogError("Overlap detected");
					gc.SpawnNewGenerator();
					
					//return true;
				}
			}
		}

		return false;
	}


	
	/*
	bool CheckRoomOverlap(Room room)
	{
		Vector3 roomPos = room.gameObject.transform.position;
		Vector3 pt1 = new Vector3(room.RoomBounds.center.x - room.RoomBounds.extents.x, room.RoomBounds.center.y + room.RoomBounds.extents.y, room.RoomBounds.center.z - room.RoomBounds.extents.z);
		Vector3 pt2 = new Vector3(room.RoomBounds.center.x - room.RoomBounds.extents.x, room.RoomBounds.center.y - room.RoomBounds.extents.y, room.RoomBounds.center.z + room.RoomBounds.extents.z);
		Vector3 pt3 = new Vector3(room.RoomBounds.center.x - room.RoomBounds.extents.x, room.RoomBounds.center.y + room.RoomBounds.extents.y, room.RoomBounds.center.z + room.RoomBounds.extents.z);
		Vector3 pt4 = new Vector3(room.RoomBounds.center.x + room.RoomBounds.extents.x, room.RoomBounds.center.y - room.RoomBounds.extents.y, room.RoomBounds.center.z - room.RoomBounds.extents.z);
		Vector3 pt5 = new Vector3(room.RoomBounds.center.x + room.RoomBounds.extents.x, room.RoomBounds.center.y + room.RoomBounds.extents.y, room.RoomBounds.center.z - room.RoomBounds.extents.z);
		Vector3 pt6 = new Vector3(room.RoomBounds.center.x + room.RoomBounds.extents.x, room.RoomBounds.center.y - room.RoomBounds.extents.y, room.RoomBounds.center.z + room.RoomBounds.extents.z);

		foreach (Room r in placedRooms)
		{
			if (r.RoomBounds.Intersects(room.RoomBounds) ||
				r.RoomBounds.Contains(roomPos) ||
				r.RoomBounds.Contains(room.RoomBounds.min)
				|| r.RoomBounds.Contains(room.RoomBounds.max)
				|| r.RoomBounds.Contains(pt1)
				|| r.RoomBounds.Contains(pt2)
				|| r.RoomBounds.Contains(pt3)
				|| r.RoomBounds.Contains(pt4)
				|| r.RoomBounds.Contains(pt5)
				|| r.RoomBounds.Contains(pt6))
			{
				Debug.LogError("Overlap Detected");
				//StopCoroutine(cachedCoro);




				Debug.LogError("Reset level generator");

				StopCoroutine(cachedCoro);
				Destroy(tempCurrentRoom);

				// Delete all rooms
				if (startRoom)
				{
					Destroy(startRoom.gameObject);
				}

				if (endRoom)
				{
					Destroy(endRoom.gameObject);
				}

				foreach (Room room1 in placedRooms)
				{
					Destroy(room1.gameObject);
				}

				// Clear lists
				placedRooms.Clear();
				availableDoorways.Clear();

				cachedCoro = GenerateLevel();
				// Reset coroutine
				StartCoroutine(cachedCoro);
				//StartCoroutine(GenerateLevel());

				return true;
			}
		}

		return false;
	}
	*/



	void PlaceEndRoom()
	{
		// Instantiate room
		endRoom = Instantiate(endRoomPrefab) as EndRoom;
		endRoom.transform.parent = this.transform;

		// Create doorway lists to loop over
		List<Doorway> allAvailableDoorways = new List<Doorway>(availableDoorways);
		Doorway doorway = endRoom.doorways[0];

		bool roomPlaced = false;

		// Try all available doorways
		foreach (Doorway availableDoorway in allAvailableDoorways)
		{
			// Position room
			Room room = (Room)endRoom;
			PositionRoomAtDoorway(ref room, doorway, availableDoorway);

			// Check room overlaps
			if (CheckRoomOverlap(endRoom))
			{
				continue;
			}

			roomPlaced = true;

			// Remove occupied doorways
			doorway.gameObject.SetActive(false);
			availableDoorways.Remove(doorway);

			availableDoorway.gameObject.SetActive(false);
			availableDoorways.Remove(availableDoorway);

			// Exit loop if room has been placed
			break;
		}

		// Room couldn't be placed. Restart generator and try again
		if (!roomPlaced)
		{
			ResetLevelGenerator();
		}

		
	}

	public void ResetLevelGenerator()
	{
		Debug.LogError("Reset level generator");

		StopCoroutine("GenerateLevel");

		// Delete all rooms
		if (startRoom)
		{
			Destroy(startRoom.gameObject);
		}

		if (endRoom)
		{
			Destroy(endRoom.gameObject);
		}

		foreach (Room room in placedRooms)
		{
			Destroy(room.gameObject);
		}

		// Clear lists
		placedRooms.Clear();
		availableDoorways.Clear();

		// Reset coroutine
		StartCoroutine("GenerateLevel");
	}
}

