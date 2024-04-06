# Unicolour in Unity

This example demonstrates how [Unicolour](https://github.com/waacton/Unicolour) can be used in Unity for 3D visualisation of colour spaces.

> ☢️ This is the first time I've made anything in Unity and is unlikely to follow best practices!

## Key points
1. It is mostly a default Unity scene
2. Colours and positions of spheres are calculated using [Unicolour](https://github.com/waacton/Unicolour) 
3. [Unicolour](https://github.com/waacton/Unicolour) is referenced as an external DLL by placing `Wacton.Unicolour.dll` into `Assets/Plugins`
4. [Unicolour](https://github.com/waacton/Unicolour) is used as normal in `Assets/Scripts/ColourController.cs`
5. Confirmed to work when built for Windows and WebGL

## Controls
- <kbd>Left Mouse</kbd> · next colour space
- <kbd>Right Mouse</kbd> · previous colour space
- <kbd>Tab</kbd> · toggle movement on/off
- <kbd>Mouse</kbd> · look
- <kbd>W</kbd><kbd>A</kbd><kbd>S</kbd><kbd>D</kbd> · move forward, backward, left, right
- <kbd>Space</kbd><kbd>Ctrl</kbd> · move up, down
- <kbd>Shift</kbd> · hold to move faster
- <kbd>Q</kbd><kbd>E</kbd> · roll left, right
- <kbd>Esc</kbd> · quit
