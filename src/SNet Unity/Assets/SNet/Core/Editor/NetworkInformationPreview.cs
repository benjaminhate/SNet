using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace SNet.Core.Editor
{
    [CustomPreview(typeof(GameObject))]
    public class NetworkInformationPreview : ObjectPreview
    {
        private class NetworkIdentityInfo
        {
            public GUIContent Name;
            public GUIContent Value;
        }

        private class Styles
        {
            public readonly GUIStyle LabelStyle = new GUIStyle(EditorStyles.label);
            public readonly GUIStyle ComponentStyle = new GUIStyle(EditorStyles.boldLabel);
            
            public Styles()
            {
                var fontColor = new Color(0.7f, 0.7f, 0.7f);
                LabelStyle.padding.right += 20;
                LabelStyle.normal.textColor    = fontColor;
                LabelStyle.active.textColor    = fontColor;
                LabelStyle.focused.textColor   = fontColor;
                LabelStyle.hover.textColor     = fontColor;
                LabelStyle.onNormal.textColor  = fontColor;
                LabelStyle.onActive.textColor  = fontColor;
                LabelStyle.onFocused.textColor = fontColor;
                LabelStyle.onHover.textColor   = fontColor;
                
                ComponentStyle.normal.textColor    = fontColor;
                ComponentStyle.active.textColor    = fontColor;
                ComponentStyle.focused.textColor   = fontColor;
                ComponentStyle.hover.textColor     = fontColor;
                ComponentStyle.onNormal.textColor  = fontColor;
                ComponentStyle.onActive.textColor  = fontColor;
                ComponentStyle.onFocused.textColor = fontColor;
                ComponentStyle.onHover.textColor   = fontColor;
            }
        }

        private List<NetworkIdentityInfo> _identityInfos;
        private GUIContent _title;
        private Styles _styles = new Styles();
        
        public override void Initialize(Object[] targets)
        {
            base.Initialize(targets);
            GetNetworkInformation(target as GameObject);
        }
        
        public override GUIContent GetPreviewTitle()
        {
            return _title ?? (_title = EditorGUIUtility.TrTextContent("Network Information"));
        }
        
        public override bool HasPreviewGUI()
        {
            return _identityInfos != null && _identityInfos.Count > 0;
        }

        public override void OnPreviewGUI(Rect r, GUIStyle background)
        {
            base.OnPreviewGUI(r, background);

            if (_identityInfos == null || _identityInfos.Count == 0)
                return;
            
            if(_styles == null)
                _styles = new Styles();
            
            var maxNameLabelSize = new Vector2(100, 16);
            var maxValueLabelSize = GetMaxNameLabelSize();

            var initialX = r.x + 10;
            var initialY = r.y + 10;
            
            var nameLabelRect = new Rect(initialX, initialY, maxNameLabelSize.x, maxNameLabelSize.y);
            var valueLabelRect = new Rect(maxNameLabelSize.x, initialY, maxValueLabelSize.x, maxValueLabelSize.y);
            
            foreach (var info in _identityInfos)
            {
                GUI.Label(nameLabelRect, info.Name, _styles.LabelStyle);
                GUI.Label(valueLabelRect, info.Value, _styles.LabelStyle);
                nameLabelRect.y += nameLabelRect.height;
                nameLabelRect.x = initialX;
                valueLabelRect.y += valueLabelRect.height;
            }
        }
        
        private Vector2 GetMaxNameLabelSize()
        {
            var maxLabelSize = Vector2.zero;
            foreach (var labelSize in _identityInfos.Select(info => _styles.LabelStyle.CalcSize(info.Value)))
            {
                if (maxLabelSize.x < labelSize.x)
                {
                    maxLabelSize.x = labelSize.x;
                }
                if (maxLabelSize.y < labelSize.y)
                {
                    maxLabelSize.y = labelSize.y;
                }
            }
            return maxLabelSize;
        }

        private void GetNetworkInformation(GameObject gameObject)
        {
            var netId = gameObject.GetComponent<SNetIdentity>();
            if (netId == null) return;

            _identityInfos = new List<NetworkIdentityInfo>();
            
            _identityInfos.Add(GetStringInfo("AssetID", netId.AssetId.ToString()));
            _identityInfos.Add(GetStringInfo("SceneID", netId.SceneId.ToString()));
            
            if (!Application.isPlaying)
            {
                return;
            }
            
            _identityInfos.Add(GetStringInfo("NetworkID", netId.Id));
        }

        private NetworkIdentityInfo GetStringInfo(string name, string value)
        {
            return new NetworkIdentityInfo
            {
                Name = new GUIContent(name),
                Value = new GUIContent(value)
            };
        }
    }
}