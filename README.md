# NetSnmpMib

A comprehensive C# library for parsing and managing SNMP MIB (Management Information Base) files. NetSnmpMib provides efficient OID resolution, description lookup, and hierarchical MIB data management with support for embedded MIBs and JSON serialization.

## Features

- **MIB File Parsing**: Parse standard SNMP MIB files with comprehensive syntax support
- **OID Resolution**: Convert between OIDs and human-readable names
- **Description Lookup**: Retrieve detailed descriptions for SNMP objects
- **Hierarchical Management**: Handle complex MIB hierarchies and dependencies
- **Import Resolution**: Automatic resolution of MIB imports and dependencies
- **Embedded MIBs**: Built-in support for common standard MIBs
- **JSON Serialization**: Export MIB data to JSON format
- **Error Handling**: Robust error reporting and validation
- **State Machine Parser**: Advanced parsing engine with comprehensive state management

## Installation

### Package Manager
```bash
Install-Package NetSnmpMib
```

### .NET CLI
```bash
dotnet add package NetSnmpMib
```

### PackageReference
```xml
<PackageReference Include="NetSnmpMib" Version="1.0.0" />
```

## Requirements

- **.NET 8.0** or higher
- **Newtonsoft.Json 13.0.3**

## Quick Start

### Basic MIB Parsing

```csharp
using NetSnmpMib;

// Initialize MibManager with path to MIB files directory
var mibManager = new MibManager(@"C:\path\to\mib\files");

// Initialize and parse all MIB files in the directory
mibManager.Initialize(verbose: true);

// Get all available OIDs
var allOids = mibManager.GetAllOids();
Console.WriteLine($"Loaded {allOids.Count} OIDs");
```

### OID Resolution

```csharp
// Find name for a specific OID
string oidName = mibManager.FindNameForOid("1.3.6.1.2.1.1.1.0");
if (oidName != null)
{
    Console.WriteLine($"OID 1.3.6.1.2.1.1.1.0 = {oidName}");
}

// Find description for a specific OID
string description = mibManager.FindDescriptionForOid("1.3.6.1.2.1.1.1.0");
if (!string.IsNullOrEmpty(description))
{
    Console.WriteLine($"Description: {description}");
}
```

### Advanced Parsing with MibParser

```csharp
// Create parser instance
var parser = new MibParser();

// Parse a specific MIB file
try
{
    parser.ParseMibFile(@"C:\path\to\RFC1213-MIB.txt");
    Console.WriteLine("MIB file parsed successfully");
    
    // Access parsed tables
    foreach (var table in parser.Tables)
    {
        Console.WriteLine($"Parsed MIB: {table.Key}");
        Console.WriteLine($"Groups: {table.Value.Groups.Count}");
        Console.WriteLine($"Items: {table.Value.Items.Count}");
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Error parsing MIB: {ex.Message}");
}
```

### Error Handling

```csharp
var mibManager = new MibManager(@"C:\path\to\mib\files");

try
{
    mibManager.Initialize(verbose: true);
    
    // Attempt to resolve OID
    string name = mibManager.FindNameForOid("1.3.6.1.2.1.1.1.0");
    if (name == null)
    {
        Console.WriteLine("OID not found in loaded MIBs");
    }
}
catch (DirectoryNotFoundException)
{
    Console.WriteLine("MIB directory not found");
}
catch (Exception ex)
{
    Console.WriteLine($"Error initializing MIB manager: {ex.Message}");
}
```

### Working with Embedded MIBs

```csharp
// Get list of available embedded MIBs
var defaultMibs = MibParser.GetDefaultMibs();
Console.WriteLine("Available embedded MIBs:");
foreach (var mib in defaultMibs)
{
    Console.WriteLine($"  - {mib}");
}

// Parse a specific embedded MIB
var mibTable = MibParser.ParseDefaultMIBs("SNMPv2-MIB");
Console.WriteLine($"Loaded {mibTable.Groups.Count} groups and {mibTable.Items.Count} items");
```

### JSON Export

```csharp
var parser = new MibParser();
parser.ParseMibFile(@"C:\path\to\RFC1213-MIB.txt");

// Export specific item lookup data to JSON
parser.ExportLookup("RFC1213-MIB", "ifOperStatus", @"C:\export\ifOperStatus.json");
```

## API Reference

### MibManager Class

The main facade class for MIB operations.

#### Constructor
```csharp
public MibManager(string mibFolderPath)
```

#### Methods

| Method | Description | Returns |
|--------|-------------|---------|
| `Initialize(bool verbose = false)` | Initialize and parse all MIB files in the specified directory | `void` |
| `GetAllOids()` | Get all available OIDs from loaded MIBs | `List<string>` |
| `FindNameForOid(string oid)` | Find the name for a specific OID | `string` (null if not found) |
| `FindDescriptionForOid(string oid)` | Find the description for a specific OID | `string` (empty if not found) |

### MibParser Class

Core parsing engine with state machine implementation.

#### Constructor
```csharp
public MibParser()
```

#### Properties

| Property | Type | Description |
|----------|------|-------------|
| `Tables` | `Dictionary<string, MibTable>` | Collection of parsed MIB tables |
| `Errors` | `List<MibParserError>` | List of parsing errors |
| `LogErrors` | `bool` | Enable/disable error logging |

#### Methods

| Method | Description |
|--------|-------------|
| `ParseMibFile(string filename)` | Parse a MIB file from disk |
| `ParseMibString(string mibText, string mibName)` | Parse MIB content from string |
| `GetDefaultMibs()` | Get list of embedded MIB names |
| `ParseDefaultMIBs(string mib)` | Parse an embedded MIB |
| `ExportLookup(string table, string itemName, string file)` | Export item data to JSON |

### MibTable Class

Container for parsed MIB data with lookup functionality.

#### Properties

| Property | Type | Description |
|----------|------|-------------|
| `Name` | `string` | MIB table name |
| `Groups` | `Dictionary<string, SnmpItem>` | SNMP groups/objects |
| `Items` | `Dictionary<string, SnmpItem>` | SNMP items/variables |

#### Methods

| Method | Description | Returns |
|--------|-------------|---------|
| `OidToName(string oid)` | Convert OID to name | `string` |
| `GetDescriptionByOid(string oid)` | Get description by OID | `string` |
| `GetOidsCount()` | Get count of items | `int` |

## Project Structure

```
NetSnmpMib/
├── NetSnmpMib.sln              # Solution file
├── NetSnmpMib/
│   ├── MibManager.cs           # High-level facade for MIB operations
│   ├── MibParser.cs            # Core parsing engine with state machine
│   ├── MibTable.cs             # Container for parsed MIB data
│   ├── NetSnmpMib.csproj       # Project file
│   └── bin/                    # Build output
└── README.md                   # This file
```

## Supported MIB Features

- **Object Types**: OBJECT-TYPE, OBJECT-IDENTITY, MODULE-IDENTITY
- **Data Types**: INTEGER, OCTET STRING, OBJECT IDENTIFIER, and custom types
- **Syntax**: SEQUENCE OF, enumerated types, textual conventions
- **Imports**: Automatic resolution of MIB dependencies
- **Hierarchies**: Full support for OID hierarchies and parent-child relationships
- **Descriptions**: Parsing and storage of object descriptions
- **Error Handling**: Comprehensive error reporting and validation

## Built-in MIBs

The library includes several standard MIBs:

- SNMPv2-SMI
- RFC1155-SMI
- RFC1213-MIB
- SNMPv2-MIB
- IF-MIB
- SNMPv2-CONF
- SNMPv2-TC

## Contributing

We welcome contributions to NetSnmpMib! Here's how you can help:

### Development Setup

1. **Clone the repository**
   ```bash
   git clone https://github.com/yourusername/NetSnmpMib.git
   cd NetSnmpMib
   ```

2. **Open in Visual Studio or VS Code**
   ```bash
   # Visual Studio
   start NetSnmpMib.sln
   
   # VS Code
   code .
   ```

3. **Build the project**
   ```bash
   dotnet build
   ```

4. **Run tests**
   ```bash
   dotnet test
   ```

### Contribution Guidelines

- Follow C# coding conventions and best practices
- Add unit tests for new features
- Update documentation for API changes
- Ensure all tests pass before submitting PRs
- Use meaningful commit messages

### Reporting Issues

Please report bugs and feature requests through [GitHub Issues](https://github.com/yourusername/NetSnmpMib/issues).

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

```
MIT License

Copyright (c) 2024 NetSnmpMib Contributors

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
```

## Acknowledgments

- Built for .NET 8.0 with modern C# features
- Uses Newtonsoft.Json for JSON serialization
- Designed for high-performance SNMP applications
- Supports both file-based and embedded MIB resources

---

**NetSnmpMib** - Efficient SNMP MIB parsing and management for .NET applications.
