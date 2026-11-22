# Ahorcado · Edición Minecraft

Proyecto final de un curso de C# de [davidmotog](https://github.com/davidmotog). Adivina criaturas, estructuras, armaduras y herramientas de Minecraft antes de cometer seis fallos. Juego de escritorio para Windows, en español, hecho con Windows Forms y .NET 10.

**[Descargar Ahorcado.exe para Windows x64](https://github.com/davidmotog/Minecraft-Ahorcado-Edition-/releases/latest/download/Ahorcado.exe)** · [Descargar en ZIP](https://github.com/davidmotog/Minecraft-Ahorcado-Edition-/releases/latest/download/Ahorcado-win-x64.zip)

Proyecto de fans independiente de Mojang y Microsoft. [Directrices de uso de Minecraft](https://www.minecraft.net/es-es/usage-guidelines)

![Partida en curso de Ahorcado con el selector de categorías y el botón Personalizar](docs/partida.png)

## Cómo jugar

1. Abre **Ahorcado.exe**. Si descargaste el ZIP, descomprímelo primero. El ejecutable incluye el entorno de ejecución de .NET.
2. Elige una categoría o deja **Todas**. Pulsa **Comenzar** y prueba una letra o escribe el nombre completo. Puedes pulsar **Intro** para enviar la respuesta.
3. Hay seis fallos por partida. Las respuestas repetidas y las entradas inválidas se ignoran. Se admiten mayúsculas, minúsculas y respuestas sin tildes. La Ñ cuenta como letra distinta de la N.
4. Al ganar, se abre una ficha con la respuesta, su categoría y un enlace a sus imágenes. Pulsa **Mostrar solución** para revelar una respuesta, o **Nueva partida** para jugar de nuevo.

## Skins e imágenes

- En **Personalizar**, escribe el nombre de un jugador de Minecraft Java y pulsa **Buscar skin**. Revisa la vista previa y pulsa **Aplicar**. También puedes abrir un PNG de 64 × 64 o 64 × 32 píxeles o volver al muñeco original.
- La búsqueda envía el nombre a Mojang y descarga su skin pública cuando está disponible. La skin que apliques se guarda en este equipo y se puede usar sin conexión en la siguiente partida.
- En la ficha de victoria puedes pulsar **Añadir foto** para asociar un PNG o JPEG a esa respuesta. Se guarda en este equipo y aparece en las próximas victorias. Con Minecraft Java 26.3 instalado en la ruta predeterminada, la ficha puede mostrar directamente la textura local de una armadura o herramienta. En los demás casos muestra una ilustración de la categoría y **Ver imágenes en la wiki** abre la ficha correspondiente en tu navegador.

## Catálogo

La versión 2 toma como referencia Minecraft Java 26.3. Contiene 250 respuestas en español. Incluye 90 criaturas con identificador propio, 52 estructuras generadas y sus variantes registradas, 41 piezas de armadura y 67 herramientas, armas y objetos de utilidad. Las variantes visuales que comparten identificador, como los colores de las ovejas, cuentan como una sola criatura. Las piezas de una estructura, los proyectiles, los vehículos y las entidades técnicas quedan fuera del juego.

Los identificadores y las traducciones se contrastaron con los [registros de Java 26.3](https://github.com/misode/mcmeta/tree/2240df2376509bfaf12becbb36e156e29f8ecb4d) y los [archivos de idioma](https://github.com/misode/mcmeta/tree/4ea7e5424848e1bed4e8c059986950bdd0acfb22/assets/minecraft/lang). La lista editable está en [catalogo.json](src/Ahorcado/Recursos/catalogo.json).

## Código y compilación

Abre [Ahorcado.sln](Ahorcado.sln) en una versión de Visual Studio compatible con .NET 10, o ejecuta estos comandos en Windows con el [SDK de .NET 10](https://dotnet.microsoft.com/download/dotnet/10.0).

```powershell
dotnet build Ahorcado.sln -c Release
dotnet run --project src/Ahorcado
dotnet run --project tests/Ahorcado.Pruebas -c Release
```

El proyecto está en [src/Ahorcado](src/Ahorcado). `Partida.cs` contiene las reglas, `Catalogo.cs` lee el catálogo y `VentanaJuego.cs` contiene la interfaz principal. Las pruebas revisan las reglas y comprueban que cada nombre del catálogo se pueda acertar.

[GitHub Actions](https://github.com/davidmotog/Minecraft-Ahorcado-Edition-/actions/workflows/windows.yml) compila el juego, ejecuta las pruebas y guarda la edición portátil. Cada publicación incluye `SHA256SUMS-EXE.txt` para comprobar el ejecutable y `SHA256SUMS.txt` para comprobar el ZIP.

## Licencia

El código y las ilustraciones originales de esta edición se distribuyen con la [licencia MIT](LICENSE). Los títulos usan [Pixelify Sans](https://github.com/eifetx/Pixelify-Sans), bajo la [SIL Open Font License 1.1](src/Ahorcado/Recursos/PixelifySans-OFL.txt). Minecraft, sus marcas y sus recursos oficiales pertenecen a sus respectivos titulares.
