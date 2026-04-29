using System;
using UnityEditor;
using UnityEngine;

namespace CIW.Code.Feedbacks.Editor
{
    [CustomPropertyDrawer(typeof(FeedbackOverride))]
    public class FeedbackOverrideDrawer : PropertyDrawer
    {
        const float Space = 6f;
        const float SectionSpace = 12f;
        const float ButtonWidth = 70f;
        const float MiniButtonWidth = 60f;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float height = EditorGUIUtility.singleLineHeight;

            if (!property.isExpanded) return height;

            var customConfigProp = property.FindPropertyRelative("customConfig");
            var feedbackProp = property.FindPropertyRelative("feedback");
            var isUseOverrideProp = property.FindPropertyRelative("isUseOverride");
            var playOnChainProp = property.FindPropertyRelative("playOnChain");
            var playOnNormalProp = property.FindPropertyRelative("playOnNormal");

            height += Space;
            height += EditorGUI.GetPropertyHeight(feedbackProp, true); // feedback
            height += Space;

            height += EditorGUI.GetPropertyHeight(isUseOverrideProp, true); // isUSeOverride
            height += SectionSpace;

            if (!isUseOverrideProp.boolValue)
                return height + SectionSpace;

            height += EditorGUI.GetPropertyHeight(playOnChainProp, true); // playOnChain
            height += Space;
            height += EditorGUI.GetPropertyHeight(playOnNormalProp, true); // playOnNormal
            height += SectionSpace;

            height += EditorGUIUtility.singleLineHeight; // custom config label
            height += Space;

            bool hasFeedback = feedbackProp.objectReferenceValue != null;
            bool hasConfig = customConfigProp.managedReferenceValue != null;

            if (!hasFeedback)
            {
                height += EditorGUIUtility.singleLineHeight * 2f;
                return height;
            }

            if (feedbackProp.objectReferenceValue is FeedbackData feedback && feedback.ConfigType == null)
            {
                height += EditorGUIUtility.singleLineHeight * 2f;
                return height;
            }

            if (!hasConfig)
            {
                height += EditorGUIUtility.singleLineHeight; // create row
                height += Space;
                return height;
            }

            height += EditorGUI.GetPropertyHeight(customConfigProp, true);
            height += Space;
            height += EditorGUIUtility.singleLineHeight; // bottom buttons row
            height += Space;

            return height;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            var feedbackProp = property.FindPropertyRelative("feedback");
            var isUseOverrideProp = property.FindPropertyRelative("isUseOverride");
            var playOnChainProp = property.FindPropertyRelative("playOnChain");
            var playOnNormalProp = property.FindPropertyRelative("playOnNormal");
            var customConfigProp = property.FindPropertyRelative("customConfig");

            Rect rect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);

            property.isExpanded = EditorGUI.Foldout(rect, property.isExpanded, label, true);
            if (!property.isExpanded)
            {
                EditorGUI.EndProperty();
                return;
            }

            EditorGUI.indentLevel++;
            rect.y += EditorGUIUtility.singleLineHeight + SectionSpace;

            DrawProperty(ref rect, feedbackProp, true, Space);
            DrawProperty(ref rect, isUseOverrideProp, true, SectionSpace);

            if (!isUseOverrideProp.boolValue)
            {
                EditorGUI.indentLevel--;
                EditorGUI.EndProperty();
                return;
            }

            DrawProperty(ref rect, playOnChainProp, true, Space);
            DrawProperty(ref rect, playOnNormalProp, true, SectionSpace);

            DrawCustomConfigSectiom(rect, property, feedbackProp, customConfigProp);

            EditorGUI.indentLevel--;
            EditorGUI.EndProperty();
        }

        private void DrawCustomConfigSectiom(Rect rect, SerializedProperty rootProperty, SerializedProperty feedbackProp, SerializedProperty customConfigProp)
        {
            EditorGUI.LabelField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), "Custom Config", EditorStyles.boldLabel);

            rect.y += EditorGUIUtility.singleLineHeight + Space;

            FeedbackData feedback = feedbackProp.objectReferenceValue as FeedbackData;

            using (new EditorGUI.IndentLevelScope())
            {
                if (feedback == null)
                {
                    Rect helpRect = new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight * 2f);
                    EditorGUI.HelpBox(helpRect, "먼저 Feedback Data를 할당해야 합니다.", MessageType.Info);
                    return;
                }

                if (feedback.ConfigType == null)
                {
                    Rect helpRect = new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight * 2f);
                    EditorGUI.HelpBox(helpRect, "이 Feedback Data는 Custom Config를 지원하지 않습니다.", MessageType.Info);
                    return;
                }

                object currentConfig = customConfigProp.managedReferenceValue;
                bool hasConfig = currentConfig != null;

                if (!hasConfig)
                {
                    DrawCreateRow(rect, rootProperty, feedback, customConfigProp);
                    return;
                }

                ValidateConfigType(feedback, customConfigProp);

                float propHeight = EditorGUI.GetPropertyHeight(customConfigProp, true);
                Rect propRect = new Rect(rect.x, rect.y, rect.width, propHeight);
                EditorGUI.PropertyField(propRect, customConfigProp, true);

                rect.y += propHeight + Space;

                DrawBottomButtons(rect, rootProperty, feedback, customConfigProp);
            }
        }

        private void DrawCreateRow(Rect rect, SerializedProperty rootProperty, FeedbackData feedback, SerializedProperty customConfigProp)
        {
            Rect infoRect = new Rect(rect.x, rect.y, rect.width - ButtonWidth - 4f, EditorGUIUtility.singleLineHeight);
            Rect createRect = new Rect(rect.x + rect.width - ButtonWidth, rect.y, ButtonWidth, EditorGUIUtility.singleLineHeight);

            EditorGUI.LabelField(infoRect, $"None ({feedback.ConfigType.Name})");

            if (GUI.Button(createRect, "Create"))
            {
                FeedbackConfig config = feedback.CreateDefaultConfig();

                if (config  == null)
                {
                    Debug.LogWarning($"{feedback.name} : CreateDefaultConfig returned null");
                    return;
                }

                customConfigProp.managedReferenceValue = config;
                rootProperty.serializedObject.ApplyModifiedProperties();
            }
        }

        private void DrawBottomButtons(Rect rect, SerializedProperty rootProperty, FeedbackData feedback, SerializedProperty customConfigProp)
        {
            Rect leftRect = new Rect(rect.x, rect.y,  rect.width - (MiniButtonWidth * 2f + 8f), EditorGUIUtility.singleLineHeight);
            Rect resetRect = new Rect(rect.x + rect.width - (MiniButtonWidth * 2f + 8f), rect.y, MiniButtonWidth, EditorGUIUtility.singleLineHeight);
            Rect clearRect = new Rect(rect.x + rect.width - MiniButtonWidth, rect.y, MiniButtonWidth, EditorGUIUtility.singleLineHeight);

            string currentTypeName = customConfigProp.managedReferenceValue?.GetType().Name ?? "None";
            EditorGUI.LabelField(leftRect, $"Type : {currentTypeName}");

            if (GUI.Button(resetRect, "Reset"))
            {
                FeedbackConfig config = feedback.CreateDefaultConfig();

                if (config == null)
                {
                    Debug.LogWarning($"{feedback.name} : CreateDefaultConfig returned null");
                    return;
                }

                customConfigProp.managedReferenceValue = config;
                rootProperty.serializedObject.ApplyModifiedProperties();
            }

            if (GUI.Button(clearRect, "Clear"))
            {
                customConfigProp.managedReferenceValue = null;
                rootProperty.serializedObject.ApplyModifiedProperties();
            }
        }

        private void ValidateConfigType(FeedbackData feedback, SerializedProperty customConfigProp)
        {
            object manageRef = customConfigProp.managedReferenceValue;
            if (manageRef == null) return;
            if (feedback.ConfigType == null) return;
            if (manageRef.GetType() == feedback.ConfigType) return;

            Debug.LogWarning($"{feedback.name} : customConfig type mismatch. " + $"Expected {feedback.ConfigType.Name}, but got {manageRef.GetType().Name}");
        }

        private float DrawProperty(ref Rect rect, SerializedProperty property, bool includeChildren = true, float extraSpace = 0f)
        {
            float height = EditorGUI.GetPropertyHeight(property, includeChildren);
            Rect drawRect = new Rect(rect.x, rect.y, rect.width, height);
            EditorGUI.PropertyField(drawRect, property, includeChildren);
            rect.y += height + extraSpace;
            return height;
        }
    }
}

