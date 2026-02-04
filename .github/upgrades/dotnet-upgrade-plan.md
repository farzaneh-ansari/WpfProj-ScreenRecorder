# .NET 8.0 Upgrade Plan

## Execution Steps

Execute steps below sequentially one by one in the order they are listed.

1. Validate that an .NET 8.0 SDK required for this upgrade is installed on the machine and if not, help to get it installed.
2. Ensure that the SDK version specified in global.json files is compatible with the .NET 8.0 upgrade.
3. Upgrade `WpfProj\WpfProj.csproj`

## Settings

This section contains settings and data used by execution steps.

### Excluded projects

Table below contains projects that do belong to the dependency graph for selected projects and should not be included in the upgrade.

| Project name | Description |
|:------------|:-----------:|

### Aggregate NuGet packages modifications across all projects

NuGet packages used across all selected projects or their dependencies that need version update in projects that reference them.

| Package Name                        | Current Version | New Version | Description                                                   |
|:------------------------------------|:---------------:|:-----------:|:--------------------------------------------------------------|
| Microsoft.Bcl.AsyncInterfaces       | 9.0.10          |             | Replace with `Microsoft.Bcl.AsyncInterfaces` `8.0.0`          |
| System.Buffers                      | 4.5.1           |             | Functionality included with new framework reference           |
| System.Drawing.Common               | 9.0.10          |             | Replace with `System.Drawing.Common` `8.0.23`                 |
| System.IO.Pipelines                 | 9.0.10          |             | Replace with `System.IO.Pipelines` `8.0.0`                    |
| System.Memory                       | 4.5.5           |             | Functionality included with new framework reference           |
| System.Numerics.Vectors             | 4.5.0           |             | Functionality included with new framework reference           |
| System.Runtime.CompilerServices.Unsafe | 6.0.0        |             | Replace with `System.Runtime.CompilerServices.Unsafe` `6.1.2` |
| System.Text.Encodings.Web           | 9.0.10          |             | Replace with `System.Text.Encodings.Web` `8.0.0`              |
| System.Text.Json                    | 9.0.10          |             | Replace with `System.Text.Json` `8.0.6`                       |
| System.Threading.Tasks.Extensions   | 4.5.4           |             | Functionality included with new framework reference           |
| System.ValueTuple                   | 4.5.0           |             | Functionality included with new framework reference           |

| Package Name                        | Current Version | New Version | Description                                   |
|:------------------------------------|:---------------:|:-----------:|:----------------------------------------------|
| Microsoft.Bcl.AsyncInterfaces       |                 | 8.0.0       | Replacement for `Microsoft.Bcl.AsyncInterfaces` |
| System.Drawing.Common               |                 | 8.0.23      | Replacement for `System.Drawing.Common`         |
| System.IO.Pipelines                 |                 | 8.0.0       | Replacement for `System.IO.Pipelines`           |
| System.Runtime.CompilerServices.Unsafe |              | 6.1.2       | Replacement for `System.Runtime.CompilerServices.Unsafe` |
| System.Text.Encodings.Web           |                 | 8.0.0       | Replacement for `System.Text.Encodings.Web`     |
| System.Text.Json                    |                 | 8.0.6       | Replacement for `System.Text.Json`              |

### Project upgrade details

#### `WpfProj\\WpfProj.csproj` modifications

Project properties changes:
  - Project file needs to be converted to SDK-style.
  - Target framework should be changed from `.NETFramework,Version=v4.8` to `net8.0-windows`

NuGet packages changes:
  - `Microsoft.Bcl.AsyncInterfaces` should be replaced (currently `9.0.10`) with `Microsoft.Bcl.AsyncInterfaces` `8.0.0`
  - `System.Drawing.Common` should be replaced (currently `9.0.10`) with `System.Drawing.Common` `8.0.23`
  - `System.IO.Pipelines` should be replaced (currently `9.0.10`) with `System.IO.Pipelines` `8.0.0`
  - `System.Runtime.CompilerServices.Unsafe` should be replaced (currently `6.0.0`) with `System.Runtime.CompilerServices.Unsafe` `6.1.2`
  - `System.Text.Encodings.Web` should be replaced (currently `9.0.10`) with `System.Text.Encodings.Web` `8.0.0`
  - `System.Text.Json` should be replaced (currently `9.0.10`) with `System.Text.Json` `8.0.6`
  - `System.Buffers` should be removed (`4.5.1`) (functionality included with new framework reference)
  - `System.Memory` should be removed (`4.5.5`) (functionality included with new framework reference)
  - `System.Numerics.Vectors` should be removed (`4.5.0`) (functionality included with new framework reference)
  - `System.Threading.Tasks.Extensions` should be removed (`4.5.4`) (functionality included with new framework reference)
  - `System.ValueTuple` should be removed (`4.5.0`) (functionality included with new framework reference)
