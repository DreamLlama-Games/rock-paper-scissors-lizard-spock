using GameEnums;
using RelationMatrix;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(SymmetricRelationMatrix))]
    public class SymmetricRelationMatrixEditor : UnityEditor.Editor
    {
        private RelationElement _newElement;

        private const float CellSize     = 22f;
        private const float Padding      = 6f;
        private const float HeaderHeight = 80f;

        public override void OnInspectorGUI()
        {
            var matrix = (SymmetricRelationMatrix)target;

            EditorGUILayout.LabelField("Add Element", EditorStyles.boldLabel);

            EditorGUILayout.BeginHorizontal();
            _newElement = (RelationElement)EditorGUILayout.EnumPopup(_newElement);
            if (GUILayout.Button("Add", GUILayout.Width(70)))
            {
                Undo.RecordObject(matrix, "Add Relation Element");
                matrix.AddElement(_newElement);
                EditorUtility.SetDirty(matrix);
            }
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(10);
            DrawMatrix(matrix);
        }

        private void DrawMatrix(SymmetricRelationMatrix matrix)
        {
            var elements = matrix.Elements;
            if (elements.Count == 0)
                return;

            EditorGUILayout.LabelField("Relation Matrix", EditorStyles.boldLabel);
            GUILayout.Space(6);

            // --- Row label width ---
            var rowLabelWidth = 0f;
            foreach (var e in elements)
                rowLabelWidth = Mathf.Max(
                    rowLabelWidth,
                    EditorStyles.label.CalcSize(new GUIContent(e.ToString())).x
                );

            rowLabelWidth += Padding;
            var count = elements.Count;

            // --- HEADER AREA ---
            var headerRect = EditorGUILayout.GetControlRect(false, HeaderHeight);

            for (var i = 0; i < count; i++)
            {
                var cell = new Rect(
                    headerRect.x + rowLabelWidth + i * CellSize,
                    headerRect.y,
                    CellSize,
                    HeaderHeight
                );

                DrawVerticalHeader(cell, elements[i].ToString());
            }

            // --- MATRIX ---
            for (var r = 0; r < count; r++)
            {
                var rowRect = EditorGUILayout.GetControlRect(false, CellSize);

                EditorGUI.LabelField(
                    new Rect(rowRect.x, rowRect.y, rowLabelWidth, CellSize),
                    elements[r].ToString()
                );

                var x = rowRect.x + rowLabelWidth;

                for (var c = 0; c < count; c++)
                {
                    var cell = new Rect(x, rowRect.y, CellSize, CellSize);

                    if (c < r)
                    {
                        // lower triangle empty
                    }
                    else if (c == r)
                    {
                        EditorGUI.BeginDisabledGroup(true);
                        EditorGUI.Toggle(cell, true);
                        EditorGUI.EndDisabledGroup();

                        if (matrix.Get(elements[r], elements[c]) != 0f)
                        {
                            Undo.RecordObject(matrix, "Fix Diagonal");
                            matrix.Set(elements[r], elements[c], -1);
                            EditorUtility.SetDirty(matrix);
                        }
                    }
                    else
                    {
                        var isChecked = matrix.Get(elements[r], elements[c]) > 0f;
                        var newChecked = EditorGUI.Toggle(cell, isChecked);

                        if (newChecked != isChecked)
                        {
                            Undo.RecordObject(matrix, "Edit Relation");
                            matrix.Set(
                                elements[r],
                                elements[c],
                                newChecked ? 1 : -1
                            );
                            EditorUtility.SetDirty(matrix);
                        }
                    }

                    x += CellSize;
                }
            }
        }

        private void DrawVerticalHeader(Rect rect, string text)
        {
            Matrix4x4 oldMatrix = GUI.matrix;

            // Pivot at center-bottom of the header cell
            var pivot = new Vector2(
                rect.x + rect.width * 0.5f,
                rect.y + rect.height
            );

            GUIUtility.RotateAroundPivot(-90f, pivot);

            // After rotation, width/height swap
            var rotated = new Rect(
                pivot.x,
                pivot.y - rect.width * 0.5f,
                rect.height,
                rect.width
            );

            EditorGUI.LabelField(rotated, text, EditorStyles.miniLabel);

            GUI.matrix = oldMatrix;
        }
    }
}
