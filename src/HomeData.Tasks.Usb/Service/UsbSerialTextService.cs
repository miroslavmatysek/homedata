using System.IO.Ports;
using HomeData.Model.TaskSetup;
using HomeData.Tasks.Usb.Model;
using Microsoft.Extensions.Logging;

namespace HomeData.Tasks.Usb.Service;

public class UsbSerialTextService : IUsbSerialTextService
{
    private readonly ILogger<UsbSerialTextService> _logger;
    private readonly List<string> _data;
    private readonly object _lock;
    private readonly object _resultLock;
    private readonly Thread _readThread;


    private bool _continue;
    private UsbSerialTextTaskSetup _setup;
    private SerialPort _serialPort;

    public UsbSerialTextService(ILogger<UsbSerialTextService> logger)
    {
        _logger = logger;
        _data = new List<string>();
        _lock = new object();
        _resultLock = new object();
        _readThread = new Thread(Read);
        _continue = false;
    }


    public void Init(ITaskSetup setup)
    {
        _logger.LogInformation("Serial service init");
        Stop();

        _setup = (UsbSerialTextTaskSetup)setup;


        if (_logger.IsEnabled(LogLevel.Debug))
        {
            _logger.LogDebug("Ports available: {Ports}", string.Join(",", SerialPort.GetPortNames()));
        }

        _serialPort = new SerialPort(_setup.PortName, _setup.BaudRate, (Parity)_setup.PortParity,
            _setup.PortDataBits, (StopBits)_setup.PortStopBits);
        _serialPort.Handshake = (Handshake)_setup.PortHandshake;
    }

    public bool IsRunning
    {
        get => _serialPort != null && _continue;
    }

    public void Start()
    {
        if (_continue)
            return;

        lock (_lock)
        {
            if (IsRunning)
                return;

            _serialPort.Open();
            _continue = true;
            _readThread.Start();
        }
    }

    public void Stop()
    {
        if (!IsRunning)
            return;

        lock (_lock)
        {
            if (IsRunning)
            {
                _continue = false;
                _readThread.Join();
                _serialPort.Close();
            }

            _serialPort?.Dispose();
        }
    }

    private void AddResult(string result)
    {
        lock (_resultLock)
        {
            _data.Add(result);
        }
    }

    public string[] GetData()
    {
        lock (_resultLock)
        {
            var result = _data.ToArray();
            _data.Clear();
            return result;
        }
    }

    private void Read()
    {
        while (_continue)
        {
            try
            {
                var data = _serialPort.ReadLine();
                if (data.StartsWith("data:"))
                {
                    var cleanData = data.Replace("data:", "").Replace("|", "\"");

                    AddResult(cleanData);
                }
                else
                    Thread.Sleep(20);
            }
            catch (TimeoutException)
            {
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Usb serial text data loading error: {Message}", ex.Message);
            }
        }
    }

    public void Dispose()
    {
        Stop();
    }
}