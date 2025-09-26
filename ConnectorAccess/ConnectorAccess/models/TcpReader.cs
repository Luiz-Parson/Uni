using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConnectorAccess
{
    public class TcpReader : IDisposable
    {
        private readonly string ip;
        private readonly int port;
        private bool _isListening = false;
        private CancellationTokenSource cancellationTokenSource;
        private TcpClient client;
        private NetworkStream stream;

        public event Action<string> OnEpcReceived;

        public TcpReader(string ip, int port)
        {
            this.ip = ip;
            this.port = port;
        }

        public async Task ConnectAsync()
        {
            try
            {
                client = new TcpClient();
                await client.ConnectAsync(ip, port);
                stream = client.GetStream();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void StartListening()
        {
            if (_isListening) return;

            // Limpar buffer antes de iniciar a escuta
            if (stream != null && stream.DataAvailable)
            {
                byte[] discardBuffer = new byte[1024];
                while (stream.DataAvailable)
                {
                    stream.Read(discardBuffer, 0, discardBuffer.Length);
                }
            }

            _isListening = true;
            cancellationTokenSource = new CancellationTokenSource();

            Task.Run(() => ReadLoop(cancellationTokenSource.Token));
        }

        public void StopListening()
        {
            if (!_isListening) return;

            _isListening = false;
            cancellationTokenSource?.Cancel();
        }

        private async Task ReadLoop(CancellationToken cancellationToken)
        {
            byte[] buffer = new byte[1024];

            try
            {
                while (!cancellationToken.IsCancellationRequested && client?.Connected == true)
                {
                    // Esse ReadAsync vai “bloquear” assincronamente até chegar dado ou a conexão fechar
                    int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length, cancellationToken);

                    if (bytesRead == 0)
                    {
                        break;
                    }

                    string epc = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                    OnEpcReceived?.Invoke(epc);
                }
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("O cancellationToken foi cancelado");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro na leitura TCP: {ex.Message}");
            }
        }

        public void Disconnect()
        {
            try
            {
                client?.Close();
                stream?.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao desconectar: {ex.Message}");
            }
        }

        // Para testar!
        public void StartMockingEpcReadings(string[] mockEpcs, int intervalMilliseconds = 100)
        {
            if (_isListening) return;

            _isListening = true;
            cancellationTokenSource = new CancellationTokenSource();

            Task.Run(async () =>
            {
                try
                {
                    int index = 0;

                    while (!cancellationTokenSource.Token.IsCancellationRequested && _isListening)
                    {
                        if (index >= mockEpcs.Length)
                        {
                            index = 0; // Reinicia os EPCs simulados
                        }

                        OnEpcReceived?.Invoke(mockEpcs[index]);
                        index++;

                        await Task.Delay(intervalMilliseconds, cancellationTokenSource.Token);
                    }
                }
                catch (TaskCanceledException)
                {
                    Console.WriteLine("O cancellationToken foi cancelado");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro no mock de leituras: {ex.Message}");
                }
            });
        }

        public Boolean IsListening()
        {
            return _isListening;
        }

        public void Dispose()
        {
            StopListening();
            Disconnect();
            stream?.Dispose();
            client?.Dispose();
            cancellationTokenSource?.Dispose();
        }
    }
}
