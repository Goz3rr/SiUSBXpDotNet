using SiUSBXpDotNet;

Console.WriteLine("Hello, World!");

var count = USBXpress.GetDevices().Count();
Console.WriteLine($"Found {count} devices");

foreach (var device in USBXpress.GetDevices())
{
    Console.WriteLine(device.Description);
    Console.WriteLine(device.Description);
}