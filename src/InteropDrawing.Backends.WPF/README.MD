### Schematic3D Backend for WPF

for viewport visualization, we have two transform layers:
  - The physical viewport, which represent the render size on screen
  - The virtual viewport, the portion of the scene to show within the physical layer.

The hierarchy order would be something like this:

```XML
<PhysicalViewport Width=800 Height=600>
    <VirtualViewport3D Width=500 Height=500 X=200 Y=700>
        <Scene3D />
    </VirtualViewport3D>
    <VirtualViewport2D Width=20 Height=20 X=200 Y=700>
        <Scene2D />
    </VirtualViewport2D>
</PhysicalViewport>
```
This graph takes a 500x500 area of the scene and fits it in the physical viewport at 800x600

In practice, the Physical/Virtual layer can be achieved with a ViewBox

```XML
<ViewBox Width=800 Height=600>
    <Grid Width=500 Height=500>
        <Scene3D />
        <Scene2D />        
    <Grid />
<ViewBox>
```


Now the design problem is that VirtualViewport needs to be
only a DependencyObject to prevent it having visual attributes (like width, height, etc)