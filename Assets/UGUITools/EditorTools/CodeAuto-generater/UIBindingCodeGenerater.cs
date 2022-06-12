
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace YmhUGUITools.Editor
{
    public static class UIBindingCodeGenerater
    {
        [MenuItem("GameObject/ParseAttributeToCode(Self)", priority = 0)]
        static void ParseAttributeToCode()
        {

            var selects = Selection.GetTransforms(SelectionMode.Unfiltered);
            StringBuilder builder = new StringBuilder();
            foreach (var item in selects)
            {
                string path = item.name;
                RecallParent(item, ref path);
                if (item.GetComponent<Text>() != null)
                {
                    string prefix = "Text text_";
                    string suffix = ";";
                    string text = prefix + path + suffix;
                    builder.AppendLine(text);
                }
                else if (item.GetComponent<Button>() != null)
                {
                    string prefix = "Button btn_";
                    string suffix = ";";
                    string text = prefix + path + suffix;
                    builder.AppendLine(text);
                }
                else if (item.GetComponent<Image>() != null)
                {
                    string prefix = "Image img_";
                    string suffix = ";";
                    string text = prefix + path + suffix;
                    builder.AppendLine(text);
                }
            }

            GUIUtility.systemCopyBuffer = builder.ToString();
        }


        [MenuItem("GameObject/ParseReferenceToCode(Self)", priority = 0)]
        static void ParseReferenceToCode()
        {
            
            var selects = Selection.GetTransforms(SelectionMode.Unfiltered);
            StringBuilder builder = new StringBuilder();
            foreach (var item in selects)
            {
                string path = item.name;
                RecallParent(item, ref path);
                if (item.GetComponent<Text>() != null)
                {
                    string prefix = "text_" + item.name + " = " + "GameObject.Find(\"";
                    string suffix = "\").GetComponent<Text>();";
                    string text = prefix + path + suffix;
                    builder.AppendLine(text);
                }
                else if (item.GetComponent<Button>() != null)
                {
                    string prefix = item.name + " = " + "GameObject.Find(\"";
                    string suffix = "\").GetComponent<Button>();";
                    string text = prefix + path + suffix;
                    builder.AppendLine(text);
                }
                else if(item.GetComponent<Image>() != null)
                {
                    string prefix = item.name + " = " + "GameObject.Find(\"";
                    string suffix = "\").GetComponent<Image>();";
                    string text = prefix + path + suffix;
                    builder.AppendLine(text);
                }
            }

            GUIUtility.systemCopyBuffer = builder.ToString();
        }


        static void RecallParent(Transform t, ref string str)
        {
            if (!t.parent.CompareTag("UIBase"))
            {
                str = t.parent.name + "/" + str;
                RecallParent(t.parent, ref str);
            }
        }

    }
}

