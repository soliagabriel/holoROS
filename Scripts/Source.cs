//Here the connection and interaction between Unity/HL and ROS is made.

//Two different WebSockets libraries are used to establish a connection.
//WebSocketSharp provides a websocket client that allow us to connect Unity with ROS via RosBridge. 
//Since this library it is not available for Windows Store Apps (HL format), Windows.Networking.Socket is responsible
//for connecting HL and ROS.

//Pieces of code wrapped in #if UNITY_EDITOR are used only when Unity Play mode is running.
//Pieces of code wrapped in #if !UNITY_EDITOR are used only when the app is running on HL.

//Methodology for establishing an async Rosbridge connection during Unity play mode and to build a message responsible for accessing and 
//sending a ROS service message were given here: 
//github.com/2016UAVClass/Simulation-Unity3D @author Michael Jenkin, Robert Codd-Downey and Andrew Speers

using UnityEngine;
using Scripts;
using System;
using System.Collections.Generic;
using System.Reflection;

#if UNITY_EDITOR
using WebSocketSharp;
using System.Threading;
#endif

#if !UNITY_EDITOR
using Windows.Networking.Sockets;
using Windows.Storage.Streams;
using Windows.Networking;
using Windows.Foundation;
using Windows.UI.Core;
#endif

public class Source : Singleton<Source> {

    public Button communicator;

    //VM IP adress used for connection
    private string host = "173.250.182.160";

    //Default ROSBridge port
    private int port = 9090;
    public bool con = false;

    public GameObject turtle;
    public GameObject plane;

    //Value that will be send to teleport_absolute
    public float tx, ty;

#if UNITY_EDITOR
    //WebSocket client from WebSocketSharp
    private WebSocket Socket;
    private Thread Thread;
#endif

    //WebSocket client from Windows.Networking.Sockets
#if !UNITY_EDITOR
    private MessageWebSocket messageWebSocket;
    Uri server;
    DataWriter dataWriter;
#endif

    void Update()
    {

#if UNITY_EDITOR
        //Connecting in Unity play mode
        if (Input.GetKeyDown(KeyCode.C))
        {
            Connect();
        }

        //Disconnecting in Unity play mode
        if (Input.GetKeyDown(KeyCode.E))
        {
            Disconnect();
        }

        //Reset in Unity Play mode
        if (Input.GetKeyDown(KeyCode.I))
        {
            turtle.transform.localPosition = new Vector3(0, 0, -0.7f);
        }

        //Clear in Unity Play mode
        if (Input.GetKeyDown(KeyCode.R))
        {
            SendService("/clear", "");
        }

#endif
        if (con)
        {
            communicator.ChangeButtonState(Button.State.Selected);

            turtle.SetActive(true);

            plane.GetComponent<Renderer>().material.color = new Color(0, 0, 0.4f);

            turtle.transform.parent = plane.transform;

#if UNITY_EDITOR
            //Moving in Unity play mode
            if (Input.GetKey(KeyCode.W))
            {
                turtle.transform.localPosition += Vector3.up * 0.005f;
            }

            if (Input.GetKey(KeyCode.S))
            {
                turtle.transform.localPosition += -Vector3.up * 0.005f;
            }

            if (Input.GetKey(KeyCode.D))
            {
                turtle.transform.localPosition += Vector3.right * 0.005f;
            }

            if (Input.GetKey(KeyCode.A))
            {
                turtle.transform.localPosition += -Vector3.right * 0.005f;
            }
#endif
            
            //Taking the cube position from the simulated environment and scaling to a compatible value that will be
            //send to ROS to update the turtle position.
            tx = (5.54f + (turtle.transform.localPosition.x * 11.78f));
            ty = (5.54f + (turtle.transform.localPosition.y * 11.78f));

            //Accessing ROS service turtle1/teleport_absolute to update turtle position
            SendService("/turtle1/teleport_absolute", "{\"x\": " + tx + ", \"y\": " + ty + ", \"theta\": 0}");
        }
        else
        {
            communicator.ChangeButtonState(Button.State.Inactive);

            plane.GetComponent<Renderer>().material.color = new Color(1, 1, 1);

            turtle.SetActive(false);
        }

    }

//Tap Gesture on HL
#if !UNITY_EDITOR
    void OnSelect()
    {
        if ((InteractibleManager.Instance.FocusedGameObject == GameObject.Find("Connector")) && (!con))
        {
            Connect();
        }

        if ((InteractibleManager.Instance.FocusedGameObject == GameObject.Find("Connector")) && (con))
        {
            Disconnect();
        }
    }
#endif

    public void Connect()
    {
        //Async connection.
#if UNITY_EDITOR
        Thread = new Thread(Run);
        Thread.Start();
#endif

#if !UNITY_EDITOR
        
        messageWebSocket = new MessageWebSocket();

        server = new Uri("ws://" + host + ":" + port.ToString());

        IAsyncAction outstandingAction = messageWebSocket.ConnectAsync(server);
        AsyncActionCompletedHandler aach = new AsyncActionCompletedHandler(NetworkConnectedHandler);
        outstandingAction.Completed = aach;
#endif
    }

//Successfull network connection handler on HL
#if !UNITY_EDITOR
    public void NetworkConnectedHandler(IAsyncAction asyncInfo, AsyncStatus status)
    {
        // Status completed is successful.
        if (status == AsyncStatus.Completed)
        {
            //Guarenteed connection
            con = true;
            
            //Creating the writer that will be repsonsible to send a message through Rosbridge
            dataWriter = new DataWriter(messageWebSocket.OutputStream);

        }
        else
        {
            con = false;
        }
    }
#endif

//Starting connection between Unity play mode and ROS.
    private void Run()
    {
#if UNITY_EDITOR
        Socket = new WebSocket("ws://" + host + ":" + port);
        Socket.Connect();
        con = true;

        while (true)
        {
            Thread.Sleep(10000);
        }
#endif
    }

    public void Disconnect()
    {
//Killing connection
#if UNITY_EDITOR
        Thread.Abort();
        Socket.Close();
        con = false;
#endif

#if !UNITY_EDITOR
        messageWebSocket.Dispose();
        messageWebSocket = null;
        con = false;
#endif
    }

#if UNITY_EDITOR
    void OnApplicationQuit()
    {
        Thread.Abort();
        Socket.Close();
        con = false;
    }
#endif

//Sending the service message
    public void SendService(string service, string args)
    {
#if UNITY_EDITOR
        if (Socket != null)
        {
            string s = CallService(service, args);
            Socket.Send(s);
        }
#endif

#if !UNITY_EDITOR
        if (messageWebSocket != null)
        {
            string s = CallService(service, args);
            dataWriter.WriteString(s);
            dataWriter.StoreAsync();
        }
#endif
    }

//Building the service message that will be send to ROS
    public static string CallService(string service, string args)
    {
        if ((args == null) || args.Equals(""))
            return "{\"op\": \"call_service\", \"service\": \"" + service + "\"}";
        else
            return "{\"op\": \"call_service\", \"service\": \"" + service + "\", \"args\" : " + args + "}";
    }

}