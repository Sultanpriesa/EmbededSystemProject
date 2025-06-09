using System;
using System.IO.Ports;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using EMSAPI.Models;
using EMSAPI.Data;

namespace EMSAPI.Services
{
    public class SerialService : BackgroundService
    {
        private readonly object _serialLock = new object();
        private readonly IServiceProvider _serviceProvider;

        public SerialService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using (var serialPort = new SerialPort("COM13", 9600))
            {
                serialPort.Open();
                while (!stoppingToken.IsCancellationRequested)
                {
                    try
                    {
                        var messagePart = serialPort.ReadLine().Trim();
                        Console.WriteLine($"Received from embed: {messagePart}"); // Debug log
                        var sensorSignalEmbed = 0;
                        var buttonSignalEmbed = 0;

                        if (messagePart == "You Got Mail!")
                        {

                            sensorSignalEmbed = 1;
                            buttonSignalEmbed = 1;
                        }
                        else if (messagePart == "ON")
                        {
                            sensorSignalEmbed = 0;
                            buttonSignalEmbed = 1;
                        }
                        else if (messagePart == "OFF")
                        {
                            sensorSignalEmbed = 0;
                            buttonSignalEmbed = 0;
                        }

                        var movement = new Movement
                        {
                            Message = messagePart,
                            ButtonSignal = buttonSignalEmbed,
                            SensorSignal = sensorSignalEmbed,
                            DataFrom = "Embed"
                        };
                        // Save movement to DB using a scoped context
                        using (var scope = _serviceProvider.CreateScope())
                        {
                            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                            db.Movements.Add(movement);
                            await db.SaveChangesAsync(stoppingToken);
                        }
                    }
                    catch (TimeoutException) { }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                    }
                    await Task.Delay(100, stoppingToken);
                }
                serialPort.Close();
            }
        }


        public void SendData(string message)
        {
            using (var serialPort = new SerialPort("COM11", 9600))
            {
                try
                {
                    serialPort.Open();
                    serialPort.WriteLine(message);
                    Console.WriteLine($"Sent: {message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error sending data: {ex.Message}");
                }
                finally
                {
                    serialPort.Close();
                }
            }
        }

        public async Task SendMovementAsync(Movement movement)
        {
            string message = $"Message: {movement.Message}, ButtonSignal: {movement.ButtonSignal}, SensorSignal: {movement.SensorSignal}";
            await Task.Run(() =>
            {
                lock (_serialLock)
                {
                    SendData(message);
                }
            });
        }
    }
}