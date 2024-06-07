using SiUSBXpDotNet;

Console.WriteLine("Hello, World!");

Console.WriteLine($"DLL Version: {USBXpress.GetDLLVersion()}");
Console.WriteLine($"Timeout rx:{USBXpress.ReadTimeout} tx:{USBXpress.WriteTimeout}");

USBXpress.ReadTimeout = TimeSpan.FromSeconds(1);
USBXpress.WriteTimeout = Timeout.InfiniteTimeSpan;
Console.WriteLine($"Timeout rx:{USBXpress.ReadTimeout} tx:{USBXpress.WriteTimeout}");

var count = USBXpress.GetDevices().Count();
Console.WriteLine($"Found {count} devices");

foreach (var device in USBXpress.GetDevices())
{
    Console.WriteLine($"Serial: {device.SerialNumber}");
    Console.WriteLine($"Desc: {device.Description}");
    Console.WriteLine($"LinkName: {device.LinkName}");
    Console.WriteLine($"VID: {device.Vid}");
    Console.WriteLine($"PID: {device.Pid}");

    device.Open();

    Console.WriteLine($"Partnum: {device.PartNum}");
    Console.WriteLine($"ProductStr: {device.ProductString}");

    //device.Close();

    //device.Open();
    //Console.WriteLine($"ProductStr: {device.ProductString}");
    //device.Dispose();
}