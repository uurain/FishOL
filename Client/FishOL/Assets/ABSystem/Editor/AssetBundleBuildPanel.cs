using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Tangzx.ABSystem
{
    public class AssetBundleBuildPanel : EditorWindow
    {
        [MenuItem("ABSystem/Builder Panel")]
        static void Open()
        {
            GetWindow<AssetBundleBuildPanel>("ABSystem", true);
        }

        [MenuItem("ABSystem/Build_Config")]
        public static void BuildRes()
        {
            Debug.Log("BuildRes");
            LuaBuild.HandleLuaFile();
            DiffGameRes.BuildAllFiles(0);
        }

        [MenuItem("ABSystem/Builde AssetBundles")]
        static void BuildAssetBundles()
        {
            LuaBuild.HandleLuaFile();
            AssetBundleBuildConfig config = LoadAssetAtPath<AssetBundleBuildConfig>(savePath);
            if (config == null)
                return;

#if UNITY_5 || UNITY_2018
            ABBuilder builder = new AssetBundleBuilder5x(new AssetBundlePathResolver());
#else
			ABBuilder builder = new AssetBundleBuilder4x(new AssetBundlePathResolver());
#endif

            builder.SetDataWriter(config.depInfoFileFormat == AssetBundleBuildConfig.Format.Text ? new AssetBundleDataWriter() : new AssetBundleDataBinaryWriter());
            builder.Begin();

            for (int i = 0; i < config.filters.Count; i++)
            {
                AssetBundleFilter f = config.filters[i];
                if (f.valid)
                    builder.AddRootTargets(new DirectoryInfo(f.path), new string[] { f.filter });
            }

            builder.Export();
            builder.End();

            BuildUIUpdate();
            DiffGameRes.BuildAllFiles(0);
        }

        [MenuItem("ABSystem/Build UpdateUI")]
        static void BuildUIUpdate()
        {

            AssetBundleBuildConfig config = LoadAssetAtPath<AssetBundleBuildConfig>(savePath);
            if (config == null)
                return;

            string updateAbCachedPath = Application.dataPath + "/AssetBundles/updateCached.txt";
            Dictionary<string, string> cachedMd5 = new Dictionary<string, string>();
            if (File.Exists(updateAbCachedPath))
            {
                string[] cachedStrs = File.ReadAllLines(updateAbCachedPath);
                for (int i = 0; i < cachedStrs.Length; ++i)
                {
                    string[] temps = cachedStrs[i].Split(':');
                    cachedMd5[temps[0]] = temps[1];
                }
            }

            string SavePath = Application.streamingAssetsPath + "/AssetBundles/";
            string[] StrPaths = Directory.GetFiles(Application.dataPath + "/UIUpdate");
            List<Object> ListAssetsObj = new List<Object>();
            bool needBuild = false;
            int updateChildCount = 0;
            List<string> cacheInfoList = new List<string>();
            for (int i = 0; i < StrPaths.Length; i++)
            {
                string StrExtension = Path.GetExtension(StrPaths[i]);
                if (StrExtension == ".meta")
                    continue;
                updateChildCount++;
                string XdStrPath = StrPaths[i].Substring(StrPaths[i].LastIndexOf("/Assets") + 1).Replace('\\', '/');

                string md5Str = Util.md5file(XdStrPath);
                if (!cachedMd5.ContainsKey(XdStrPath) || cachedMd5[XdStrPath] != md5Str)
                {
                    needBuild = true;
                }
                Object obj = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(XdStrPath);
                string info = string.Format("{0}:{1}", XdStrPath, Util.md5file(XdStrPath));
                cacheInfoList.Add(info);
                Debug.Log(info);
                ListAssetsObj.Add(obj);
            }
            if (updateChildCount != cachedMd5.Count)
            {
                needBuild = true;
            }
            Debug.Log("need build UIUpdate:" + needBuild);
            needBuild = true;
            if (needBuild)
            {
                StreamWriter sw = new StreamWriter(updateAbCachedPath);
                for (int i = 0; i < cacheInfoList.Count; ++i)
                {
                    sw.WriteLine(cacheInfoList[i]);
                }
                sw.Close();

                Debug.Log("build update ui");
                BuildPipeline.BuildAssetBundle(null, ListAssetsObj.ToArray(), SavePath + "UIUpdate.ab", BuildAssetBundleOptions.ChunkBasedCompression, EditorUserBuildSettings.activeBuildTarget);
            }
        }

        static T LoadAssetAtPath<T>(string path) where T : Object
        {
#if UNITY_5
            return AssetDatabase.LoadAssetAtPath<T>(savePath);
#else
			return (T)AssetDatabase.LoadAssetAtPath(savePath, typeof(T));
#endif
        }

        const string savePath = "Assets/ABSystem/config.asset";

        private AssetBundleBuildConfig _config;
        private ReorderableList _list;
        private Vector2 _scrollPosition = Vector2.zero;

        AssetBundleBuildPanel()
        {

        }

        void OnListElementGUI(Rect rect, int index, bool isactive, bool isfocused)
        {
            const float GAP = 5;

            AssetBundleFilter filter = _config.filters[index];
            rect.y++;

            Rect r = rect;
            r.width = 16;
            r.height = 18;
            filter.valid = GUI.Toggle(r, filter.valid, GUIContent.none);

            r.xMin = r.xMax + GAP;
            r.xMax = rect.xMax - 300;
            GUI.enabled = false;
            filter.path = GUI.TextField(r, filter.path);
            GUI.enabled = true;

            r.xMin = r.xMax + GAP;
            r.width = 50;
            if (GUI.Button(r, "Select"))
            {
                var path = SelectFolder();
                if (path != null)
                    filter.path = path;
            }

            r.xMin = r.xMax + GAP;
            r.xMax = rect.xMax;
            filter.filter = GUI.TextField(r, filter.filter);
        }

        string SelectFolder()
        {
            string dataPath = Application.dataPath;
            string selectedPath = EditorUtility.OpenFolderPanel("Path", dataPath, "");
            if (!string.IsNullOrEmpty(selectedPath))
            {
                if (selectedPath.StartsWith(dataPath))
                {
                    return "Assets/" + selectedPath.Substring(dataPath.Length + 1);
                }
                else
                {
                    ShowNotification(new GUIContent("不能在Assets目录之外!"));
                }
            }
            return null;
        }

        void OnListHeaderGUI(Rect rect)
        {
            EditorGUI.LabelField(rect, "Asset Filter");
        }

        void InitConfig()
        {
            _config = LoadAssetAtPath<AssetBundleBuildConfig>(savePath);
            if (_config == null)
            {
                _config = new AssetBundleBuildConfig();
            }
        }

        void InitFilterListDrawer()
        {
            _list = new ReorderableList(_config.filters, typeof(AssetBundleFilter));
            _list.drawElementCallback = OnListElementGUI;
            _list.drawHeaderCallback = OnListHeaderGUI;
            _list.draggable = true;
            _list.elementHeight = 22;
            _list.onAddCallback = (list) => Add();
        }

        void Add()
        {
            string path = SelectFolder();
            if (!string.IsNullOrEmpty(path))
            {
                var filter = new AssetBundleFilter();
                filter.path = path;
                _config.filters.Add(filter);
            }
        }

        void OnGUI()
        {
            if (_config == null)
            {
                InitConfig();
            }

            if (_list == null)
            {
                InitFilterListDrawer();
            }

            bool execBuild = false;
            //tool bar
            GUILayout.BeginHorizontal(EditorStyles.toolbar);
            {
                if (GUILayout.Button("Add", EditorStyles.toolbarButton))
                {
                    Add();
                }
                if (GUILayout.Button("Save", EditorStyles.toolbarButton))
                {
                    Save();
                }
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Build", EditorStyles.toolbarButton))
                {
                    execBuild = true;
                }
            }
            GUILayout.EndHorizontal();

            //context
            GUILayout.BeginVertical();
            {
                //format
                GUILayout.BeginHorizontal();
                {
                    EditorGUILayout.PrefixLabel("DepInfoFileFormat");
                    _config.depInfoFileFormat = (AssetBundleBuildConfig.Format)EditorGUILayout.EnumPopup(_config.depInfoFileFormat);
                }
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                {
                    EditorGUILayout.PrefixLabel("Ver");
                    //_config.Ver = EditorGUILayout.TextField(_config.Ver);
                    //_config.Ver = Regex.Replace(_config.Ver, "[^0-9]", "");
                }
                GUILayout.EndHorizontal();

                GUILayout.Space(10);

                //Filter item list
                _scrollPosition = GUILayout.BeginScrollView(_scrollPosition);
                {
                    _list.DoLayoutList();
                }
                GUILayout.EndScrollView();
            }
            GUILayout.EndVertical();

            //set dirty
            if (GUI.changed)
                EditorUtility.SetDirty(_config);

            if (execBuild)
                Build();
        }

        private void Build()
        {
            Save();
            BuildAssetBundles();
        }

        void Save()
        {
            AssetBundlePathResolver.instance = new AssetBundlePathResolver();

            if (LoadAssetAtPath<AssetBundleBuildConfig>(savePath) == null)
            {
                AssetDatabase.CreateAsset(_config, savePath);
            }
            else
            {
                EditorUtility.SetDirty(_config);
            }
        }
    }
}