# PLC Omron Standard

[![MIT](https://img.shields.io/github/license/thirstyape/PLC-Omron-Standard)](https://github.com/thirstyape/PLC-Omron-Standard/blob/main/LICENSE)
[![NuGet](https://img.shields.io/nuget/v/Plc.Omron.Standard.svg)](https://www.nuget.org/packages/Plc.Omron.Standard/)

This project was created to have an easy to use, .NET Standard library to communicate with Omron PLCs.

## Getting Started

These instuctions can be used to acquire and implement the library.

### Installation

To use this library either clone a copy of the repository or check out the [NuGet package](https://www.nuget.org/packages/Plc.Omron.Standard/)

### Usage

**Basic Example**

The following example provides a complete use case. This example makes use of the most basic configuration.

```
using PLC_Omron_Standard;

public class MyClass() 
{
    public bool Test() 
    {
        // Create PLC connection
        var plc = new PlcOmron("192.168.1.100");

        plc.Connect();

        // Read data
        var data = plc.Read(123, 10);

        // Write data
        return data.Length > 0 && plc.Write(123, "Hello!");
    }
}
```

## Authors

* **Nathanael Frey**

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details

## Acknowledgments

Parts of this library have been inspired by:
* [mcOmron](https://github.com/mcNets/mcOmron)
* [dotnet-omron](https://github.com/ricado-group/dotnet-omron)
