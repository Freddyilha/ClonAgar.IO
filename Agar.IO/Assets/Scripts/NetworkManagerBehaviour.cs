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
        var soc = gameObject.GetComponent<SocketIOComponent>();

        soc.On("boop", TestBoop);
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        //On("teste", (mensagemServidor) =>
        //{
        //    print("me mexi: " + mensagemServidor);
        //});

    }

    private void onFirstConection()
    {
        On("open", (eventCallBack) =>
        {
            print("conectei");
            //Debug.Log(eventCallBack);
            //Debug.Log("algo");
        });
    }

    public void TestBoop(SocketIOEvent e)
    {
        Debug.Log(string.Format("[name: {0}, data: {1}]", e.name, e.data));
    }

}
