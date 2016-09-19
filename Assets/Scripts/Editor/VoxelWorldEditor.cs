using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(VoxelEditor))]
public class VoxelWorldEditor : Editor {

    SerializedProperty voxelAtlasFileProperty;

    SerializedProperty worldDimensionsProperty;

    SerializedProperty brickResolutionProperty;

    SerializedProperty maxDimensionsProperty;

    VoxelBrush setBrush = new SetVoxelBrush();

    VoxelBrush setAdjacentBrush = new SetVoxelAdjacentBrush();

    VoxelBrush currentBrush;

    VoxelMaterial currentMaterial;

    int currentMaterialId = 1;

    void OnEnable()
    {
        voxelAtlasFileProperty = serializedObject.FindProperty("voxelAtlasFile");

        worldDimensionsProperty = serializedObject.FindProperty("worldDimensions");

        brickResolutionProperty = serializedObject.FindProperty("brickResolution");

        maxDimensionsProperty = serializedObject.FindProperty("maxDimensions");

        currentBrush = setAdjacentBrush;

        VoxelEditor editor = (VoxelEditor)target;


    }

	// Use this for initialization
	void OnSceneGUI ()
    {
        Event e = Event.current;
        // We use hotControl to lock focus onto the editor (to prevent deselection)
        int controlID = GUIUtility.GetControlID(FocusType.Passive);

        VoxelEditor editor = (VoxelEditor)target;

        switch (Event.current.GetTypeForControl(controlID))
        {
            case EventType.MouseDown:

                if(Event.current.button == 1)
                {
                    Selection.activeGameObject = null;
                }

                GUIUtility.hotControl = controlID;

                Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);

                editor.PaintWithRay(ray, currentBrush, currentMaterial);

                Event.current.Use();

                break;

            case EventType.MouseUp:

                GUIUtility.hotControl = 0;

                Event.current.Use();

                break;

        }

    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        serializedObject.ApplyModifiedProperties();

        currentMaterialId = EditorGUILayout.IntSlider(currentMaterialId, 1, 50);
        EditorGUILayout.PropertyField(voxelAtlasFileProperty, true);
        EditorGUILayout.PropertyField(worldDimensionsProperty, true);
        EditorGUILayout.PropertyField(brickResolutionProperty, true);
        EditorGUILayout.PropertyField(maxDimensionsProperty, true);

        VoxelEditor editor = (VoxelEditor)target;
        currentMaterial = editor.materialAtlas.GetVoxelMaterial((byte)currentMaterialId);
        if (GUILayout.Button("Brush: Place"))
        {
            

            currentBrush = setAdjacentBrush;
        }

        if (GUILayout.Button("Brush: Remove"))
        {
            currentMaterial = editor.materialAtlas.GetVoxelMaterial(0);

            currentBrush = setBrush;
        }

        serializedObject.ApplyModifiedProperties();
        serializedObject.Update();
        EditorUtility.SetDirty(target);
    }

    public void Update()
    {
        // This is necessary to make the framerate normal for the editor window.
        Repaint();
    }
}
