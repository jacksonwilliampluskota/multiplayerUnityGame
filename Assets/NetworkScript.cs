using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class NetworkScript : MonoBehaviour {

	NetworkServerSimple m_Server;
	NetworkClient m_Client;
	const short k_MyMessage = 100;
    public GameObject pref;
    

    // When using a server instance like this it must be pumped manually
    void Update() {
		if (m_Server != null)
			m_Server.Update();
	}

	private void Start()
	{
        // StartServer();
		StartClient();
	}

	void StartServer() {
		m_Server = new NetworkServerSimple();
		m_Server.RegisterHandler(k_MyMessage, OnMyMessage);
		m_Server.RegisterHandler(MsgType.Connect, OnConnect);
		m_Server.RegisterHandler(MsgType.Disconnect, OnDisconnect);
		if (m_Server.Listen(5555))
			Debug.Log("Started listening on 5555");
	}

	void StartClient() {
		m_Client = new NetworkClient();
		m_Client.RegisterHandler(MsgType.Connect, OnClientConnected);
		m_Client.RegisterHandler(k_MyMessage, OnMyMessage);
		m_Client.Connect("127.0.0.1", 5555);
	}

	void SendMsgClient(string msg)
	{
		NetworkWriter writer = new NetworkWriter(); 
		writer.StartMessage(k_MyMessage); 
		writer.Write(42); 
		writer.Write(msg); 
		writer.FinishMessage(); 
		Debug.Log("SendMsg=" + msg);

		m_Client.SendWriter(writer, 0);
	}

	void OnMyMessage(NetworkMessage netmsg)
	{
		Debug.Log("Got message, size=" + netmsg.reader.Length); 
		var someValue = netmsg.reader.ReadInt32(); 
		var someString = netmsg.reader.ReadString(); 
		Debug.Log("Message value=" + someValue + " Message string=‘" + someString + "’");
	}
	
	void OnConnect(NetworkMessage netMsg)
	{
		Debug.Log("Chat client connect:" + netMsg.conn.connectionId);
		var response = "Welcome"+netMsg.conn.connectionId;
		
		NetworkWriter writer = new NetworkWriter(); 
		writer.StartMessage(k_MyMessage); 
		writer.Write(42); 
		writer.Write(response); 
		writer.FinishMessage(); 
		Debug.Log("SendMsg=" + response);

		netMsg.conn.SendWriter(writer, 0);
	}

	void OnDisconnect(NetworkMessage netMsg)
	{
		Debug.Log("Chat client disconnect:"+netMsg.conn.connectionId);
	}

	void OnClientConnected(NetworkMessage netmsg) {
		Debug.Log("Client connected to server");
		SendMsgClient("Hello server!");
        CreatePrefab();
	}

    void CreatePrefab() {
        Debug.Log("prefab adicionado");
        //Instantiate(prefab, new Vector3(50.0F, 0, 0), Quaternion.identity);
        Instantiate(pref, new Vector3(0, 0, 0), Quaternion.identity);
    }
}
