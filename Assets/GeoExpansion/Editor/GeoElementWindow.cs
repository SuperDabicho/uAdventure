﻿using UnityEngine;
using System.Collections;
using System;
using UnityEditorInternal;
using UnityEditor;
using System.Linq;
using MapzenGo.Helpers;
using uAdventure.Geo;
using System.Collections.Generic;
using MapzenGo.Helpers.Search;
using MapzenGo.Models.Settings.Editor;
using uAdventure.Core;

namespace uAdventure.Editor
{

    public class GeoElementWindow : ReorderableListEditorWindowExtension, DialogReceiverInterface
    {
        private int selectedElement;

        const string PATH_SAVE_SCRIPTABLE_OBJECT = "Assets/MapzenGo/Resources/Settings/";

        /* ---------------------------------
         * Attributes
         * -------------------------------- */

        private SearchPlace place;
        private string address = "";
        private Vector2 location;
        private string lastSearch = "";
        private float timeSinceLastWrite;
        private bool editing;
        private GeoElement element;
        private Rect mm_Rect;
        private string[] menus;
        private Texture2D imagePreview;

        /* ----------------------------------
         * GUI ELEMENTS
         * -----------------------------------*/
        private DropDown addressDropdown;
        private GUIMap map;
        private ReorderableList geometriesReorderableList;

        public GeoElementWindow(Rect aStartPos, GUIContent aContent, GUIStyle aStyle, params GUILayoutOption[] aOptions) : base(aStartPos, aContent, aStyle, aOptions)
        {
            mm_Rect = aStartPos;

            var bc = new GUIContent();
            bc.image = (Texture2D)Resources.Load("EAdventureData/img/icons/poi", typeof(Texture2D));
            bc.text = "GeoElements";  //TC.get("Element.Name1");
            ButtonContent = bc;

            // Get existing open window or if none, make a new one:
            place = UnityEngine.Object.FindObjectOfType<SearchPlace>();
            if (place == null)
            {
                SearchPlace search = new GameObject("Searcher").AddComponent<SearchPlace>();
                place = search;
            }
            menus = new string[] { "Position", "Attributes", "Actions" };

            Init();
        }

        /* ----------------------------------
         * INIT: Used for late initialization after constructor
         * ----------------------------------*/
        void Init()
        {
            EditorApplication.update += this.Update;
            
            place.DataStructure = HelperExtention.GetOrCreateSObjectReturn<StructSearchData>(ref place.DataStructure, PATH_SAVE_SCRIPTABLE_OBJECT);
            place.namePlaceСache = "";
            place.DataStructure.dataChache.Clear();

            addressDropdown = new DropDown("Address");
            map = new GUIMap();
            map.Repaint += Repaint;
            map.Zoom = 19;
            
            
        }

        /* ----------------------------------
          * ON GUI: Used for drawing the window every unity event
          * ----------------------------------*/
        int selected = 0;
        public override void Draw(int aID)
        {

            if (selectedElement == -1)
            {
                GUILayout.Label("Nothing selected", GUILayout.Width(mm_Rect.width), GUILayout.Height(mm_Rect.height));
                return;
            }

            element = Controller.getInstance().getSelectedChapterDataControl().getObjects<GeoElement>()[selectedElement];

            // Set geometries list reference
            map.Geometries = new List<GMLGeometry>() { element.Geometry };

            selected = GUILayout.Toolbar(selected, menus);

            switch (selected)
            {
                case 0:
                    {
                        element.Geometry.Type = (GMLGeometry.GeometryType)EditorGUILayout.EnumPopup("Geometry type", element.Geometry.Type);
                        EditorGUILayout.LabelField("Points: " + element.Geometry.Points.Count);
                        element.Influence = EditorGUILayout.FloatField("Influence Radius", element.Influence);


                        if (GUILayout.Button("Center") && element.Geometry.Points.Count > 0)
                        {
                            location = element.Geometry.Points.Aggregate(new Vector2(), (p, n) => p + n.ToVector2()) / element.Geometry.Points.Count;
                            map.Center = location.ToVector2d();
                        }

                        if (GUILayout.Button(!editing ? "Edit" : "Finish"))
                        {
                            editing = !editing;
                        }
                        
                        EditorGUILayout.Separator();
                        var prevAddress = address;
                        address = addressDropdown.LayoutBegin();
                        if (address != prevAddress)
                        {
                            timeSinceLastWrite = 0;
                        }

                        // Location control
                        location = EditorGUILayout.Vector2Field("Location", location);
                        var lastRect = GUILayoutUtility.GetLastRect();
                        if (location != map.Center.ToVector2())
                            map.Center = new Vector2d(location.x, location.y);


                        // Map drawing
                        map.selectedGeometry = element.Geometry;
                        if (map.DrawMap(GUILayoutUtility.GetRect(mm_Rect.width, mm_Rect.height - lastRect.y - lastRect.height)))
                        {
                            Debug.Log(map.GeoMousePosition);
                            if (element != null && editing)
                            {
                                element.Geometry.AddPoint(map.GeoMousePosition);
                            }
                        }
                        
                        location = map.Center.ToVector2();
                        

                        if (addressDropdown.LayoutEnd())
                        {
                            // If new Location is selected from the dropdown
                            lastSearch = address = addressDropdown.Value;
                            foreach (var l in place.DataStructure.dataChache)
                                if (l.label == address)
                                    location = l.coordinates;

                            place.DataStructure.dataChache.Clear();
                            Repaint();
                        }
                    } break;
                case 1:
                    {
                        GUILayout.Label("Full description");
                        element.FullDescription = GUILayout.TextArea(element.FullDescription, GUILayout.Height(250));
                        
                        element.Name = EditorGUILayout.TextField("Name", element.Name);
                        element.BriefDescription = EditorGUILayout.TextField("Brief description", element.BriefDescription);
                        element.DetailedDescription = EditorGUILayout.TextField("Detailed description", element.DetailedDescription);
                        
                        GUILayout.Label("Element image");
                        GUILayout.BeginHorizontal();
                       /* if (GUILayout.Button(clearImg, GUILayout.Width(0.1f * windowWidth)))
                        {
                            foregroundMaskPath = "";
                        }*/
                        GUILayout.Box(element.Image, GUILayout.Width(0.78f * m_Rect.width));
                        if (GUILayout.Button(TC.get("Buttons.Select"), GUILayout.Width(0.2f * m_Rect.width)))
                        {
                            ShowAssetChooser(AssetType.Image);
                        }
                        GUILayout.EndHorizontal();
                    }
                    break;
                case 2:
                    {

                    }break;
            }

            
        }

        protected override void OnAdd(ReorderableList r)
        {
            Controller.getInstance().getSelectedChapterDataControl().getObjects<GeoElement>().Add(new GeoElement("newGeoElement"));
        }

        protected override void OnAddOption(ReorderableList r, string option) { }

        protected override void OnButton()
        {
            reorderableList.index = -1;
        }

        protected override void OnElementNameChanged(ReorderableList r, int index, string newName)
        {
            Controller.getInstance().getSelectedChapterDataControl().getObjects<GeoElement>()[index].Id = newName;
        }

        protected override void OnRemove(ReorderableList r)
        {
            Controller.getInstance().getSelectedChapterDataControl().getObjects<GeoElement>().RemoveAt(r.index);
        }

        protected override void OnReorder(ReorderableList r)
        {
        }

        protected override void OnSelect(ReorderableList r)
        {
            selectedElement = r.index;
        }

        protected override void OnUpdateList(ReorderableList r)
        {
            r.list = Controller.getInstance().getSelectedChapterDataControl().getObjects<GeoElement>().ConvertAll(s => s.Id);
        }

        /* ------------------------------------------
         * Update: used for taking care of the http requests
         * ------------------------------------------ */
        void Update()
        {
            //Debug.Log(Time.fixedDeltaTime);
            timeSinceLastWrite += Time.fixedDeltaTime;
            if (timeSinceLastWrite > 3f)
            {
                PerformSearch();
            }

            if (place.DataStructure.dataChache.Count > 0)
            {
                var addresses = new List<string>();
                foreach (var r in place.DataStructure.dataChache)
                    addresses.Add(r.label);
                addressDropdown.Elements = addresses;
                Repaint();
            }
        }

        /* ---------------------------------------
         * PerformSearch: Used to control the start of searches
         * --------------------------------------- */
        private void PerformSearch()
        {
            if (address != null && address.Trim() != "" && lastSearch != address)
            {
                place.namePlace = address;
                place.SearchInMapzen();
                lastSearch = address;
            }
        }

        enum AssetType
        {
            Image
        }

        // --------------------------
        // Dialog methods: show and receive
        // --------------------------
        void ShowAssetChooser(AssetType type)
        {
            switch (type)
            {
                case AssetType.Image:
                    ImageFileOpenDialog backgroundDialog =
                    (ImageFileOpenDialog)ScriptableObject.CreateInstance(typeof(ImageFileOpenDialog));
                    backgroundDialog.Init(this, BaseFileOpenDialog.FileType.ITEM_IMAGE);
                    break;
            }

        }

        public void OnDialogOk(string message, object workingObject = null, object workingObjectSecond = null)
        {
            switch ((BaseFileOpenDialog.FileType)workingObject)
            {
                case BaseFileOpenDialog.FileType.ITEM_IMAGE:
                    element.Image = message;
                    Controller.getInstance().getSelectedChapterDataControl().getScenesList().getScenes()[
                       GameRources.GetInstance().selectedSceneIndex].setPreviewBackground(message);
                    if (element.Image != null && !element.Image.Equals(""))
                        imagePreview = AssetsController.getImage(element.Image).texture;
                    break;
            }
        }

        public void OnDialogCanceled(object workingObject = null)
        {
            Debug.Log("Canceled");
        }
    }


}

