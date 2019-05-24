using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;

public class NetworkManagerBehaviour : SocketIOComponent
{
    public static NetworkManagerBehaviour instance;

    SocketIOComponent SIO;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        onFirstConection();
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);

        SIO = gameObject.GetComponent<SocketIOComponent>();

        SIO.On("successfull", successMesage);

        SIO.On("UPDATED", successMesageTwo);

        SIO.Emit("updatePosition", new JSONObject(JsonUtility.ToJson(GameManagerBehaviour.instance.getPlayerPos())) );

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

        });
    }

    public void successMesage(SocketIOEvent e)
    {
        print("Conectado com sucesso");
    }

    public void successMesageTwo(SocketIOEvent e)
    {
        print("E agora o que eu faço?");
    }

    public void sendPosition(Vector2 position)
    {
        var pos = JsonUtility.ToJson(position);
        SIO.Emit("updatePosition", new JSONObject(pos));
    }
    //b) Sending additional data

    //   Dictionary<string, string> data = new Dictionary<string, string>();
    //data["email"] = "some@email.com";
    //   data["pass"] = Encrypt("1234");
    //socket.Emit("user:login", new JSONObject(data));
}