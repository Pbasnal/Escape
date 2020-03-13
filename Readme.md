This project contains basic code to generate level in Spelunky style. To us this project- 

### RoomBuilder
1. To create a room, create a GameObject and add the RoomBuilder component to it.
2. RoomBuilder adds Grid and Tilemap component to the gameobject so that an individual room can be designed using a Tile Palette.
3. Since all the rooms should be square in shape and be of the same size, RoomBuilder has a RoomSize property which is a ScriptableObject. 
4. All rooms should have the same RoomSize. And in the editor window, a square box will appear to show the bounds of the Room.

### LevelGenerator
1. Once the room has been created, add another GameObject and add the LevelGenerator component. It takes a LevelSize property which should be different than RoomSize but it is created from the same ScriptableObject.


### LevelRenderer
1. Wall property is GameObject with a sprite which is used to generate the Walls around the Level.
2. Markers are another GameObjects which are used to highlight the path and starting room. This piece is just to debug the code and can be over-written and changed in order to fit the requriements.