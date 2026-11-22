# Ahorcado · Edición histórica Alfa 2.0

Esta edición recupera la interfaz, la lista de criaturas y las reglas del proyecto original del curso de C# de davidmotog. La publicación adapta la compilación a .NET 10, identifica la versión como Alfa 2.0 y usa ilustraciones originales para distribuir el juego. El código se distribuye con licencia MIT.

## Compilación

Con el SDK de .NET 10 instalado en Windows, ejecuta este comando desde la carpeta del proyecto.

```powershell
dotnet publish Ahorcado.csproj -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true -p:EnableCompressionInSingleFile=true -p:DebugType=None
```

El ejecutable se llama `Ahorcado-alfa2.0.exe`. Para jugar, abre el programa, pulsa **Comenzar** e introduce una letra cada vez.
