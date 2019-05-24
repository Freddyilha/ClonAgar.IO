using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;

public class NetworkManagerBehaviour : SocketIOComponent
{
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        onFirstConection();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

    private void onFirstConection()
    {
        On("open", (eventCallBack) =>
        {
            print("conectei");
            Debug.Log(eventCallBack);
            Debug.Log("algo");
        });
    }

}
