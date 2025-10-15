using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using UnityEngine;

public class NetworkManagerPong : MonoBehaviour
{
    public enum Mode { Host, Client }
    public Mode mode = Mode.Host;

    [Header("Rede")]
    public string remoteIp = "192.168.0.10"; // IP do Host no cliente
    public int port = 7777;
    public float sendInterval = 0.03f;

    [Header("Referências")]
    public Transform paddleHost;
    public Transform paddleClient;
    public Transform ball;
    public GameManager gameManager;

    [Header("Configuração bola")]
    public float velocidadeBola = 6f;

    private UdpClient udp;
    private IPEndPoint remoteEP;
    private ConcurrentQueue<(string, IPEndPoint)> recvQueue = new ConcurrentQueue<(string, IPEndPoint)>();
    private IPEndPoint lastClient;

    private float lastSend = 0f;
    private Vector2 ballPos;
    private Vector2 ballVel;

    private bool rodando = false;

    [Serializable]
    public struct InputMsg { public float paddleY; }

    [Serializable]
    public struct StateMsg
    {
        public float hostPaddleY;
        public float clientPaddleY;
        public float ballX;
        public float ballY;
        public int scoreHost;
        public int scoreClient;
    }

    void Start()
    {
        rodando = true;

        if (gameManager == null)
        {
            gameManager = FindObjectOfType<GameManager>();
            if (gameManager == null)
                Debug.LogWarning("NetworkManagerPong: GameManager não encontrado. Pontuação será ignorada.");
        }

        if (mode == Mode.Host)
        {
            udp = new UdpClient(port);
            remoteEP = new IPEndPoint(IPAddress.Any, 0);
            Debug.Log("Host iniciado na porta " + port);
            Task.Run(() => ReceiveLoop());

            ballPos = ball != null ? (Vector2)ball.position : Vector2.zero;
            ballVel = new Vector2(velocidadeBola, UnityEngine.Random.Range(-velocidadeBola, velocidadeBola));
        }
        else
        {
            udp = new UdpClient();
            udp.Client.Bind(new IPEndPoint(IPAddress.Any, 0));
            remoteEP = new IPEndPoint(IPAddress.Parse(remoteIp), port);
            Debug.Log($"Cliente iniciado. Tentando se conectar ao Host {remoteIp}:{port}");
            Task.Run(() => ReceiveLoop());
        }
    }

    async Task ReceiveLoop()
    {
        while (rodando)
        {
            try
            {
                var result = await udp.ReceiveAsync();
                string msg = Encoding.UTF8.GetString(result.Buffer);
                recvQueue.Enqueue((msg, result.RemoteEndPoint));

                if (mode == Mode.Host)
                    Debug.Log($"Host recebeu input do cliente {result.RemoteEndPoint}");
                else
                    Debug.Log($"Cliente recebeu estado do Host");
            }
            catch (ObjectDisposedException)
            {
                break; // UDP fechado
            }
            catch (Exception e)
            {
                Debug.LogWarning("Erro ReceiveLoop: " + e.Message);
                await Task.Delay(500); // evita spam
            }
        }
    }

    void Update()
    {
        // Processa mensagens
        while (recvQueue.TryDequeue(out var entry))
        {
            string msg = entry.Item1;
            IPEndPoint sender = entry.Item2;

            if (mode == Mode.Host)
            {
                var input = JsonUtility.FromJson<InputMsg>(msg);
                if (paddleClient != null)
                {
                    Vector3 p = paddleClient.position;
                    p.y = input.paddleY;
                    paddleClient.position = p;
                }
                lastClient = sender;
            }
            else
            {
                var state = JsonUtility.FromJson<StateMsg>(msg);

                if (paddleHost != null)
                {
                    Vector3 hostP = paddleHost.position;
                    hostP.y = state.hostPaddleY;
                    paddleHost.position = Vector3.Lerp(paddleHost.position, hostP, 0.7f);
                }

                if (paddleClient != null)
                {
                    Vector3 myP = paddleClient.position;
                    myP.y = state.clientPaddleY;
                    paddleClient.position = Vector3.Lerp(paddleClient.position, myP, 0.7f);
                }

                if (ball != null)
                    ball.position = Vector3.Lerp(ball.position, new Vector3(state.ballX, state.ballY, 0), 0.8f);

                if (gameManager != null)
                    gameManager.SetPontuacao(state.scoreHost, state.scoreClient);
            }
        }

        // Lógica Host ou Cliente
        if (mode == Mode.Host)
            UpdateHost();
        else
            UpdateClient();
    }

    private void UpdateHost()
    {
        float v = Input.GetAxis("Vertical");
        if (paddleHost != null)
        {
            Vector3 pHost = paddleHost.position;
            pHost.y += v * Time.deltaTime * 7f;
            paddleHost.position = pHost;
        }

        ballPos += ballVel * Time.deltaTime;
        if (ballPos.y > 4.5f || ballPos.y < -4.5f) ballVel.y = -ballVel.y;

        if (paddleHost != null && paddleClient != null)
        {
            if (Mathf.Abs(ballPos.x - paddleHost.position.x) < 0.5f &&
                Mathf.Abs(ballPos.y - paddleHost.position.y) < 1f && ballVel.x < 0)
                ballVel.x = -ballVel.x;

            if (Mathf.Abs(ballPos.x - paddleClient.position.x) < 0.5f &&
                Mathf.Abs(ballPos.y - paddleClient.position.y) < 1f && ballVel.x > 0)
                ballVel.x = -ballVel.x;
        }

        if (ballPos.x < -9f)
        {
            if (gameManager != null) gameManager.AumentarPontuacaoDoJogador2();
            ResetBall(1);
        }
        else if (ballPos.x > 9f)
        {
            if (gameManager != null) gameManager.AumentarPontuacaoDoJogador1();
            ResetBall(-1);
        }

        if (ball != null)
            ball.position = new Vector3(ballPos.x, ballPos.y, 0);

        if (lastClient != null && Time.time - lastSend > sendInterval)
        {
            var state = new StateMsg
            {
                hostPaddleY = paddleHost != null ? paddleHost.position.y : 0f,
                clientPaddleY = paddleClient != null ? paddleClient.position.y : 0f,
                ballX = ballPos.x,
                ballY = ballPos.y,
                scoreHost = gameManager != null ? gameManager.pontuacaoDoJogador1 : 0,
                scoreClient = gameManager != null ? gameManager.pontuacaoDoJogador2 : 0
            };

            string json = JsonUtility.ToJson(state);
            byte[] data = Encoding.UTF8.GetBytes(json);
            udp.SendAsync(data, data.Length, lastClient);
            lastSend = Time.time;
        }
    }

    private void UpdateClient()
    {
        float v = Input.GetAxis("Vertical");
        if (paddleClient != null)
        {
            Vector3 pClient = paddleClient.position;
            pClient.y += v * Time.deltaTime * 7f;
            paddleClient.position = pClient;
        }

        if (Time.time - lastSend > sendInterval)
        {
            var input = new InputMsg { paddleY = paddleClient != null ? paddleClient.position.y : 0f };
            string json = JsonUtility.ToJson(input);
            byte[] data = Encoding.UTF8.GetBytes(json);
            try
            {
                udp.SendAsync(data, data.Length, remoteEP);
            }
            catch (Exception e)
            {
                Debug.LogWarning("Falha ao enviar para Host: " + e.Message);
            }
            lastSend = Time.time;
        }
    }

    void ResetBall(int dir)
    {
        ballPos = Vector2.zero;
        ballVel = new Vector2(velocidadeBola * dir, UnityEngine.Random.Range(-velocidadeBola, velocidadeBola));
    }

    void OnDestroy()
    {
        rodando = false;
        udp?.Close();
    }

    void OnApplicationQuit()
    {
        rodando = false;
        udp?.Close();
    }
}




