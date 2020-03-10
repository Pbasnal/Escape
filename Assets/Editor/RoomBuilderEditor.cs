using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(RoomBuilder))]
public class RoomBuilderEditor : Editor
{
    private void OnSceneGUI()
    {
        // get the chosen game object
        if (target != null)
        {
            var roomBuilder = target as RoomBuilder;

            if (roomBuilder.roomSize != null)
            {
                Vector2 center = roomBuilder.transform.position;

                Vector2 topRight = new Vector2(roomBuilder.roomSize.Width / 2, roomBuilder.roomSize.Width / 2);
                Vector2 topLeft = new Vector2(-roomBuilder.roomSize.Width / 2, roomBuilder.roomSize.Width / 2);
                Vector2 bottomRight = new Vector2(roomBuilder.roomSize.Width / 2, -roomBuilder.roomSize.Width / 2);
                Vector2 bottomLeft = new Vector2(-roomBuilder.roomSize.Width / 2, -roomBuilder.roomSize.Width / 2);

                Handles.DrawLine(topLeft, topRight);
                Handles.DrawLine(topRight, bottomRight);
                Handles.DrawLine(bottomRight, bottomLeft);
                Handles.DrawLine(bottomLeft, topLeft);
            }
        }
    }
}
