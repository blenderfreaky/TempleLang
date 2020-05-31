# C--

This is the Compiler for my small C-- (formerly TempleLang) Programming Language.
It compiles to x64 with Windows 10.

## Usage

To get the CLI, [download a release](https://github.com/blenderfreaky/TempleLang/releases) or build from source.  

To use the CLI, run `./TempleLang.CLI --help` for help.

```
TempleLang.CLI 1.0.0
MIT Liscense

  -f, --file        Required. File to compile

  -t, --target      Path to place the .exe in

  -i, --printIL     Whether to output the intermediate language to the target directory

  -a, --printASM    Whether to output the assembler to the target directory

  --help            Display this help screen.

  --version         Display version information.
```

## Prerequisites

[NASM](https://www.nasm.us/) needs to be installed and the executable needs to be in the PATH variable.

`LINK.EXE` needs to be installed and in in the PATH variable.
`LINK.EXE` comes with MSVC Build Tools, which you can get using the [Visual Studio installer](https://visualstudio.microsoft.com/downloads/), however it will not be in your PATH variable and you will need to add it manually.
It is typically located at `C:\Program Files (x86)\Microsoft Visual Studio\2019\Enterprise\VC\Tools\MSVC\<VERSION>\bin\Hostx64\x64\link.exe`. Check if this is the right install path and add it to your PATH variable.

While not necessarily required, the [Windows SDK](https://developer.microsoft.com/en-us/windows/downloads/windows-10-sdk) is needed to interact with the Windows kernel.
It is currently required to install the Windows SDK to `C:\Program Files (x86)\Windows Kits\10\Lib\10.0.18362.0\um\x64`, even if you're using a different Windows version.
[You can change this to a different path in the source code](https://github.com/blenderfreaky/TempleLang/blob/master/TempleLang.Compiler/TempleLangHelper.cs#L109).
The reason for this is that windows doesn't seem to properly add the SDK to the path and this is a "temporary" fix.

## Example

See [QuickSort](https://github.com/blenderfreaky/TempleLang/tree/master/QuickSort) for a small example of C-- code.

To compile it run `./TempleLang.CLI --file QuickSort.tl`.
The QuickSort example requires the Windows SDK to be installed.
See Prerequisites for more info.
