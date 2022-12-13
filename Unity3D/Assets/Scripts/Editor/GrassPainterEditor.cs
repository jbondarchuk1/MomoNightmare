using UnityEditor;
using UnityEngine;
using UnityEditorInternal;

[CustomEditor(typeof(GeometryGrassPainter))]
[InitializeOnLoad]
public class GeometryGrassPainterEditor : Editor
{
    GeometryGrassPainter GeometryGrassPainter;
    readonly string[] toolbarStrings = { "Add", "Remove", "Edit", "Reproject" };

    readonly string[] toolbarStringsEdit = { "Edit Colors", "Edit Length/Width", "Both" };

    private void OnEnable()
    {
        GeometryGrassPainter = (GeometryGrassPainter)target;
    }
    void OnSceneGUI()
    {
        //base
        Handles.color = Color.green;
        Handles.DrawWireDisc(GeometryGrassPainter.hitPosGizmo, GeometryGrassPainter.hitNormal, GeometryGrassPainter.brushSize);
        Handles.color = new Color(0, 0.5f, 0, 0.4f);
        Handles.DrawSolidDisc(GeometryGrassPainter.hitPosGizmo, GeometryGrassPainter.hitNormal, GeometryGrassPainter.brushSize);

        if (GeometryGrassPainter.toolbarInt == 1)
        {
            Handles.color = Color.red;
            Handles.DrawWireDisc(GeometryGrassPainter.hitPosGizmo, GeometryGrassPainter.hitNormal, GeometryGrassPainter.brushSize);
            Handles.color = new Color(0.5f, 0f, 0f, 0.4f);
            Handles.DrawSolidDisc(GeometryGrassPainter.hitPosGizmo, GeometryGrassPainter.hitNormal, GeometryGrassPainter.brushSize);
        }
        if (GeometryGrassPainter.toolbarInt == 2)
        {
            Handles.color = Color.yellow;
            Handles.DrawWireDisc(GeometryGrassPainter.hitPosGizmo, GeometryGrassPainter.hitNormal, GeometryGrassPainter.brushSize);
            Handles.color = new Color(0.5f, 0.5f, 0f, 0.4f);
            Handles.DrawSolidDisc(GeometryGrassPainter.hitPosGizmo, GeometryGrassPainter.hitNormal, GeometryGrassPainter.brushSize);
            // falloff
            Handles.color = Color.yellow;
            Handles.DrawWireDisc(GeometryGrassPainter.hitPosGizmo, GeometryGrassPainter.hitNormal, GeometryGrassPainter.brushSize * GeometryGrassPainter.brushFalloffSize);
            Handles.color = new Color(0.5f, 0.5f, 0f, 0.4f);
            Handles.DrawSolidDisc(GeometryGrassPainter.hitPosGizmo, GeometryGrassPainter.hitNormal, GeometryGrassPainter.brushSize * GeometryGrassPainter.brushFalloffSize);
        }
        if (GeometryGrassPainter.toolbarInt == 3)
        {
            Handles.color = Color.cyan;
            Handles.DrawWireDisc(GeometryGrassPainter.hitPosGizmo, GeometryGrassPainter.hitNormal, GeometryGrassPainter.brushSize);
            Handles.color = new Color(0, 0.5f, 0.5f, 0.4f);
            Handles.DrawSolidDisc(GeometryGrassPainter.hitPosGizmo, GeometryGrassPainter.hitNormal, GeometryGrassPainter.brushSize);
        }
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.LabelField("Grass Limit", EditorStyles.boldLabel);
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(GeometryGrassPainter.i.ToString() + "/", EditorStyles.label);
        GeometryGrassPainter.grassLimit = EditorGUILayout.IntField(GeometryGrassPainter.grassLimit);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Hit Settings", EditorStyles.boldLabel);
        LayerMask tempMask = EditorGUILayout.MaskField("Hit Mask", InternalEditorUtility.LayerMaskToConcatenatedLayersMask(GeometryGrassPainter.hitMask), InternalEditorUtility.layers);
        GeometryGrassPainter.hitMask = InternalEditorUtility.ConcatenatedLayersMaskToLayerMask(tempMask);
        LayerMask tempMask2 = EditorGUILayout.MaskField("Painting Mask", InternalEditorUtility.LayerMaskToConcatenatedLayersMask(GeometryGrassPainter.paintMask), InternalEditorUtility.layers);
        GeometryGrassPainter.paintMask = InternalEditorUtility.ConcatenatedLayersMaskToLayerMask(tempMask2);
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Paint Status (Right-Mouse Button to paint)", EditorStyles.boldLabel);
        GeometryGrassPainter.toolbarInt = GUILayout.Toolbar(GeometryGrassPainter.toolbarInt, toolbarStrings);
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Brush Settings", EditorStyles.boldLabel);
        GeometryGrassPainter.brushSize = EditorGUILayout.Slider("Brush Size", GeometryGrassPainter.brushSize, 0.1f, 10f);

        if (GeometryGrassPainter.toolbarInt == 0)
        {
            GeometryGrassPainter.normalLimit = EditorGUILayout.Slider("Normal Limit", GeometryGrassPainter.normalLimit, 0f, 1f);
            GeometryGrassPainter.density = EditorGUILayout.Slider("Density", GeometryGrassPainter.density, 0.1f, 10f);
        }

        if (GeometryGrassPainter.toolbarInt == 2)
        {
            GeometryGrassPainter.toolbarIntEdit = GUILayout.Toolbar(GeometryGrassPainter.toolbarIntEdit, toolbarStringsEdit);
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Flood Options", EditorStyles.boldLabel);
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Flood Colors"))
            {
                GeometryGrassPainter.FloodColor();
            }
            if (GUILayout.Button("Flood Length/Width"))
            {
                GeometryGrassPainter.FloodLengthAndWidth();
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.LabelField("Soft Falloff Settings", EditorStyles.boldLabel);
            GeometryGrassPainter.brushFalloffSize = EditorGUILayout.Slider("Brush Falloff Size", GeometryGrassPainter.brushFalloffSize, 0.01f, 1f);
            GeometryGrassPainter.Flow = EditorGUILayout.Slider("Brush Flow", GeometryGrassPainter.Flow, 0.1f, 10f);
        }

        if (GeometryGrassPainter.toolbarInt == 0 || GeometryGrassPainter.toolbarInt == 2)
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Width and Length ", EditorStyles.boldLabel);
            GeometryGrassPainter.sizeWidth = EditorGUILayout.Slider("Grass Width", GeometryGrassPainter.sizeWidth, 0f, 2f);
            GeometryGrassPainter.sizeLength = EditorGUILayout.Slider("Grass Length", GeometryGrassPainter.sizeLength, 0f, 2f);
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Color", EditorStyles.boldLabel);
            GeometryGrassPainter.AdjustedColor = EditorGUILayout.ColorField("Brush Color", GeometryGrassPainter.AdjustedColor);
            EditorGUILayout.LabelField("Random Color Variation", EditorStyles.boldLabel);
            GeometryGrassPainter.rangeR = EditorGUILayout.Slider("Red", GeometryGrassPainter.rangeR, 0f, 1f);
            GeometryGrassPainter.rangeG = EditorGUILayout.Slider("Green", GeometryGrassPainter.rangeG, 0f, 1f);
            GeometryGrassPainter.rangeB = EditorGUILayout.Slider("Blue", GeometryGrassPainter.rangeB, 0f, 1f);
        }

        if (GeometryGrassPainter.toolbarInt == 3)
        {
            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Reprojection Y Offset", EditorStyles.boldLabel);

            GeometryGrassPainter.reprojectOffset = EditorGUILayout.FloatField(GeometryGrassPainter.reprojectOffset);
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        if (GUILayout.Button("Clear Mesh"))
        {
            if (EditorUtility.DisplayDialog("Clear Painted Mesh?",
               "Are you sure you want to clear the mesh?", "Clear", "Don't Clear"))
            {
                GeometryGrassPainter.ClearMesh();
            }
        }




    }

}