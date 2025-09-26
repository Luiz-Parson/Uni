using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConnectorAccess.Service.Services
{
    public class TcpSocketService : BackgroundService
    {
        private readonly ILogger<TcpSocketService> logger;
        private readonly IConfiguration config;
        private readonly IMemoryCache cache;
        private readonly IServiceProvider serviceProvider;
        private readonly TimeSpan processInterval;

        public TcpSocketService(
            ILogger<TcpSocketService> logger,
            IConfiguration config,
            IMemoryCache cache,
            IServiceProvider serviceProvider)
        {
            this.logger = logger;
            this.config = config;
            this.cache = cache;
            this.serviceProvider = serviceProvider;

            processInterval = TimeSpan.FromMinutes(int.Parse(config["Intervals:ProcessIntervalMinutes"]));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var readerAddress = config["ReaderSettings:ReaderAddress"];
            var readerPort = int.Parse(config["ReaderSettings:ReaderPort"]);
            var epcStartFilter = config["Filters:EpcStartFilter"];

            try
            {
                using var scope = serviceProvider.CreateScope();
                var productService = scope.ServiceProvider.GetRequiredService<ProductService>();
                var generalControlService = scope.ServiceProvider.GetRequiredService<GeneralControlService>();

                using var client = new TcpClient();
                logger.LogInformation("Conectando ao leitor em {Address}:{Port}", readerAddress, readerPort);

                await client.ConnectAsync(readerAddress, readerPort);

                logger.LogInformation("Conectado ao leitor!");

                using var stream = client.GetStream();
                var buffer = new byte[1024];
                var accumulator = new StringBuilder();

                while (!stoppingToken.IsCancellationRequested)
                {
                    var bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length, stoppingToken);

                    if (bytesRead > 0)
                    {
                        accumulator.Append(Encoding.UTF8.GetString(buffer, 0, bytesRead));

                        string accumulatedText = accumulator.ToString();
                        string[] lines = accumulatedText.Split(new[] { "\r\n" }, StringSplitOptions.None);

                        // processa todas menos a última (pode estar incompleta)
                        for (int i = 0; i < lines.Length - 1; i++)
                        {
                            var line = lines[i].Trim();
                            if (string.IsNullOrWhiteSpace(line))
                                continue;

                            var fields = line.Split(',');

                            if (fields.Length >= 2)
                            {
                                var epc = fields[1];

                                if (!string.IsNullOrWhiteSpace(epc) &&
                                    epc.StartsWith(epcStartFilter) &&
                                    epc.Length == 24)
                                {
                                    logger.LogInformation("Mensagem recebida: {Message}", line);

                                    var now = DateTime.Now;

                                    if (cache.TryGetValue(epc, out DateTime lastTime))
                                    {
                                        if (now - lastTime < processInterval)
                                        {
                                            logger.LogInformation("Ignorando EPC {Epc}, já processado recentemente.", epc);
                                            continue;
                                        }
                                    }

                                    cache.Set(epc, now, TimeSpan.FromMinutes(int.Parse(config["Intervals:CacheDurationMinutes"])));

                                    var product = productService.GetProductByEpc(epc);
                                    if (product == null)
                                        product = productService.AddProduct("Desconhecido", "Desconhecido", epc, "Live monitoring");

                                    generalControlService.AddGeneralControl(product.Id, now);
                                }
                            }
                            else
                            {
                                logger.LogWarning("Mensagem inválida: {Message}", line);
                            }
                        }

                        // mantém a última parte no acumulador (pode estar incompleta)
                        accumulator.Clear();
                        accumulator.Append(lines[^1]);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Erro ao conectar-se ao leitor.");
            }
        }
    }
}
