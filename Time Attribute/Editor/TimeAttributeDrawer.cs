using UnityEngine;
using UnityEditor;

/*

    TimeAttribute created by Lucas Sarkadi.

    Creative Commons Zero v1.0 Universal licence, 
    meaning it's free to use in any project with no need to ask permission or credits the author.

    Check out the github page for more informations:
    https://github.com/Slyp05/Unity_TimeAttribute

*/
[CustomPropertyDrawer(typeof(TimeAttribute))]
public class TimeAttributeDrawer : PropertyDrawer
{
    // consts
    const string frameStr = "Frames";
    const string notFloatWarning = "The [Time] attribute is useable only on float fields.";

    const float modeChoiceWidthMin = 43;
    const float modeChoiceWidthMax = 72;
    const float modeChoiceWidthRatio = 0.33f;

    // unity
    public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
    {
        if (property.propertyType != SerializedPropertyType.Float)
        {
            Debug.LogWarning(notFloatWarning);
            EditorGUI.PropertyField(rect, property, label, true);
        }
        else
        {
            TimeAttribute attr = (attribute as TimeAttribute);

            // mode choice
            Rect popUpRect = new Rect(rect);

            float modeWidth = Mathf.Clamp((rect.width - EditorGUIUtility.labelWidth) * modeChoiceWidthRatio, modeChoiceWidthMin, modeChoiceWidthMax);

            popUpRect.width = modeWidth;
            popUpRect.x = rect.xMax - popUpRect.width;

            int cacheIndent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            int modeChoice = (!property.isExpanded ^ attr.defaultToFrame ? 0 : 1);
            EditorGUI.BeginChangeCheck();
            modeChoice = EditorGUI.Popup(popUpRect, modeChoice, new string[2] { attr.unit.ToString(), frameStr });
            if (EditorGUI.EndChangeCheck())
                property.isExpanded = (modeChoice == 1) ^ attr.defaultToFrame;

            EditorGUI.indentLevel = cacheIndent;

            bool timeMode = (modeChoice == 0);

            // field tooltip
            Rect hiddenRect = new Rect(rect);

            hiddenRect.xMin += EditorGUIUtility.labelWidth;
            hiddenRect.xMax -= modeWidth + 2;

            EditorGUI.LabelField(hiddenRect, new GUIContent(" ", timeMode ?
                $"{TimeToFrames(property.floatValue)} {frameStr.ToLower()}" :
                $"{RealToDisplayTime(property.floatValue, attr.unit)} {attr.unit.ToString().ToLower()}"));

            // field
            rect.xMax -= modeWidth + 2;

            if (timeMode)
            {
                EditorGUI.BeginChangeCheck();
                float time = EditorGUI.FloatField(rect, GetGUIContentFromProperty(property),
                    RealToDisplayTime(property.floatValue, attr.unit));
                if (EditorGUI.EndChangeCheck())
                    property.floatValue = DisplayToRealTime(time, attr.unit);
            }
            else // frameMode
            {
                EditorGUI.BeginChangeCheck();
                int frame = EditorGUI.IntField(rect, GetGUIContentFromProperty(property),
                    TimeToFrames(property.floatValue));
                if (EditorGUI.EndChangeCheck())
                    property.floatValue = FramesToTime(frame);
            }
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property, label, true);
    }

    // private methods
    float frameTime => Time.fixedUnscaledDeltaTime;

    int TimeToFrames(float time) => Mathf.CeilToInt(time / frameTime);

    float FramesToTime(int frames) => (frames * frameTime);

    float RealToDisplayTime(float time, TimeAttribute.Unit unit)
    {
        if (unit == TimeAttribute.Unit.Minutes)
            return time / 60f;
        else if (unit == TimeAttribute.Unit.Hours)
            return time / 3600f;

        return time;
    }

    float DisplayToRealTime(float time, TimeAttribute.Unit unit)
    {
        if (unit == TimeAttribute.Unit.Minutes)
            return time * 60f;
        else if (unit == TimeAttribute.Unit.Hours)
            return time * 3600f;

        return time;
    }

    GUIContent GetGUIContentFromProperty(SerializedProperty prop, string displayName = null)
    {
        GUIContent ret = EditorGUI.BeginProperty(new Rect(), new GUIContent(displayName ?? prop.displayName), prop);
        EditorGUI.EndProperty();

        return ret;
    }
}

