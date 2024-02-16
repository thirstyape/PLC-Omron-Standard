# PLC Omron Standard

[![MIT](https://img.shields.io/github/license/thirstyape/PLC-Omron-Standard)](https://github.com/thirstyape/PLC-Omron-Standard/blob/main/LICENSE)
[![NuGet](https://img.shields.io/nuget/v/Plc.Omron.Standard.svg)](https://www.nuget.org/packages/Plc.Omron.Standard/)

This project was created to have an easy to use, .NET Standard library to communicate with Omron PLCs. The following features are available:

* Read from PLC memory areas
* Write to PLC memory areas
* Read and write with single values or arrays
* Read and write with typed values (options are `byte`, `bool`, `short`, `int`, `float`, `string`)
* Receive events during communications

## Getting Started

These instuctions can be used to acquire and implement the library.

### Installation

To use this library either clone a copy of the repository or check out the [NuGet package](https://www.nuget.org/packages/Plc.Omron.Standard/)

### Usage

**TCP Example**

The following example provides a basic use case for TCP based communications.

```csharp
using PLC_Omron_Standard;

public class MyClass() 
{
    public bool Test() 
    {
        // Create PLC connection
        var plc = new PlcOmron("192.168.1.100");

        plc.Connect();

        // Read data
        var data = plc.ReadString(123); // Assumes value is "Hello"

        // Write data
        return plc.Write(123, $"{data}, World!");
    }
}
```

**UDP Example**

The following example provides a basic use case for UDP based communications.

```csharp
using PLC_Omron_Standard;

public class MyClass() 
{
    public bool Test() 
    {
        // Get node addresses
        var ip = "192.168.1.100";
        var remote = byte.Parse(ip.Split('.').Last());

        var local = byte.Parse("101"); // This can be any non-zero value, but using the final part of your local IP address is recommended
        
        // Create PLC connection
        var plc = new PlcOmron(ip, 9600, false, remote, local);

        plc.Connect();

        // Read data
        var data = plc.ReadString(123); // Assumes value is "Hello"

        // Write data
        return plc.Write(123, $"{data}, World!");
    }
}
```

**Events Example**

The following example provides a use case where events are subscribed to and handled.

```csharp
using PLC_Omron_Standard;

public class MyClass() 
{
    private readonly ILogger Logger;

    public bool Test(ILogger<MyClass> logger) 
    {
        // Store logger
        Logger = logger;

        // Create PLC connection
        var plc = new PlcOmron("192.168.1.100");

        plc.NotifyCommandError += LogError;
        plc.Connect();

        // Read data
        var data = plc.ReadString(123); // Assumes value is "Hello"

        // Write data
        return plc.Write(123, $"{data}, World!");
    }

    private void LogError(string message) 
    {
        Logger.LogError("{Message}", message);
    }
}
```

## Authors

* **NF Software Inc.**

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details

## Acknowledgments

Parts of this library have been inspired by:
* [mcOmron](https://github.com/mcNets/mcOmron)
* [dotnet-omron](https://github.com/ricado-group/dotnet-omron)

Thank you to:
* [Freepik](https://www.flaticon.com/authors/freepik) for the project icon