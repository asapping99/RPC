using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour {

	public GameObject _player1;
	public GameObject _player2;
	public SpriteRenderer spr1;
	public SpriteRenderer spr2;
	public int playerCount = 0;
	private const string typeName = "UniqueGameName";
	private const string gameName = "GOM TEST ROOM";
	private HostData[] hostList;

	// Use this for initialization
	void Start () {
		spr1 = _player1.GetComponent<SpriteRenderer>();
		spr2 = _player2.GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private void StartServer() {
		Network.InitializeServer(4, 9090, !Network.HavePublicAddress());
		MasterServer.RegisterHost(typeName, gameName);
	}
	
	private void RefreshHostList() {
		MasterServer.RequestHostList(typeName);
	}
	
	void OnMasterServerEvent(MasterServerEvent msEvent) {
		Debug.Log(msEvent);
		if (msEvent == MasterServerEvent.HostListReceived) {
			hostList = MasterServer.PollHostList ();
		}
	}

	void OnServerInitialized() {
		Debug.Log("Server Initializied");
	}

	private void JoinServer(HostData hostData) {
		Network.Connect(hostData);
	}

	void OnPlayerConnected(NetworkPlayer player) {
		if (Network.isServer) {
			playerCount++;
		}

		Debug.Log("Player " + playerCount + " connected from " + player.ipAddress + ":" + player.port);
		if (playerCount == 1) {
			Debug.Log("Player 1");
			spr1.enabled = true;
		} else if (playerCount == 2) {
			Debug.Log("Player 2");
			spr1.enabled = true;
			spr2.enabled = true;
		} else {
			Debug.Log("Player Full!!");
			spr1.enabled = true;
			spr2.enabled = true;
		}
	}

	void OnPlayerDisconnected(NetworkPlayer player) {
		if (Network.isServer) {
			playerCount--;
			if (playerCount <= 0) {
				playerCount = 0;
			}
		}
		if (playerCount <= 0) {
			spr1.enabled = false;
			spr2.enabled = false;
		} else if (playerCount == 1) {
			spr2.enabled = false;
		} else {
			spr1.enabled = true;
			spr2.enabled = true;
		}
		Debug.Log("Clean up after player " + player);
		Network.RemoveRPCs(player);
		Network.DestroyPlayerObjects(player);
	}

	void OnConnectedToServer() {
		Debug.Log("Server Joined");
		if (playerCount == 1) {
			Debug.Log("Player 1");
			spr1.enabled = true;
		} else if (playerCount == 2) {
			Debug.Log("Player 2");
			spr1.enabled = true;
			spr2.enabled = true;
		} else {
			Debug.Log("Player Full!!");
			spr1.enabled = true;
			spr2.enabled = true;
		}

	}

	void OnDisconnectedFromServer(NetworkDisconnection info) {
		Debug.Log("Disconnected from server: " + info);
		if (playerCount <= 0) {
			spr1.enabled = false;
			spr2.enabled = false;
		} else if (playerCount == 1) {
			spr2.enabled = false;
		} else {
			spr1.enabled = true;
			spr2.enabled = true;
		}
	}

	void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info) {
		Debug.Log ("ttt :: " + playerCount);
		if (stream.isWriting) {
			stream.Serialize(ref playerCount);
		}else {
			stream.Serialize(ref playerCount);
		}
	}

	void OnGUI() {
		if (!Network.isClient && !Network.isServer) {
			if (GUI.Button(new Rect(100, 100, 250, 100), "Start Server")) {
				StartServer();
			}
			
			if (GUI.Button(new Rect(100, 250, 250, 100), "Refresh Hosts")) {
				RefreshHostList();
			}
			
			if (hostList != null) {
				for (int i = 0; i < hostList.Length; i++) {
					if (GUI.Button(new Rect(400, 100 + (110 * i), 300, 100), hostList[i].gameName)) {
						JoinServer(hostList[i]);
					}
				}
			}
		}
	}


}
