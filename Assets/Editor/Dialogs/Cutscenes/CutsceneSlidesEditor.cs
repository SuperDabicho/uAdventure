﻿using UnityEngine;
using System.Text.RegularExpressions;

using uAdventure.Core;
using Animation = uAdventure.Core.Animation;
using UnityEditor;

namespace uAdventure.Editor
{
    public class CutsceneSlidesEditor : BaseCreatorPopup, DialogReceiverInterface
    {
        private GUIStyle titleStyle;

        private Texture2D backgroundPreviewTex = null;

        private Texture2D addTexture = null;
        private Texture2D moveLeft, moveRight = null;
        private Texture2D clearImg = null;
        private Texture2D duplicateImg = null;

        private string[] transitionTypeName;
        private Texture2D[] transitionTypeTexture;

        private static GUISkin defaultSkin;
        private static GUISkin noBackgroundSkin;
        private static GUISkin selectedFrameSkin;

        private bool useTransitonFlag, slidesAnimationFlag;

        private string documentationTextContent = "", frameDocumentation = "";
        private string imagePath = "", soundPath = "";

        private long animationDuration = 40, transitionDuration = 0;
        private int transitionType;

        private Animation workingAnimation;

        private int selectedFrame;

        private Vector2 scrollPosition;

        private string cutscenePath;

        private void OnEnable()
        {
            titleStyle = new GUIStyle();
            titleStyle.fontStyle = FontStyle.Bold;
            titleStyle.margin = new RectOffset(0, 0, 5, 5);
        }

        public void Init(DialogReceiverInterface e, string cutsceneFilePath)
        {
            windowWidth = 800;
            windowHeight = 1000;

            cutscenePath = cutsceneFilePath;
            clearImg = Resources.Load<Texture2D>("EAdventureData/img/icons/deleteContent");
            addTexture = Resources.Load<Texture2D>("EAdventureData/img/icons/addNode");
            moveLeft = Resources.Load<Texture2D>("EAdventureData/img/icons/moveNodeLeft");
            moveRight = Resources.Load<Texture2D>("EAdventureData/img/icons/moveNodeRight");
            duplicateImg = Resources.Load<Texture2D>("EAdventureData/img/icons/duplicateNode");

            noBackgroundSkin = Resources.Load<GUISkin>("Editor/EditorNoBackgroundSkin");
            selectedFrameSkin = Resources.Load<GUISkin>("Editor/EditorLeftMenuItemSkinConcreteOptions");


            transitionTypeName = new string []{ "None" , "Fade in", "Horizontal", "Vertical"};
            transitionTypeTexture = new Texture2D[]
            {
                Resources.Load<Texture2D>("EAdventureData/img/icons/transitionNone"),
                Resources.Load<Texture2D>("EAdventureData/img/icons/transitionFadein"),
                Resources.Load<Texture2D>("EAdventureData/img/icons/transitionHorizontal"),
                Resources.Load<Texture2D>("EAdventureData/img/icons/transitionVertical")
            };
            Debug.Log(cutsceneFilePath);

            workingAnimation = Loader.loadAnimation(AssetsController.InputStreamCreatorEditor.getInputStreamCreator(),
                cutsceneFilePath, new EditorImageLoader());

            Debug.Log(workingAnimation.getAboslutePath() + " " + workingAnimation.getFrames().Count + " " + workingAnimation.isSlides() + " " + workingAnimation.getId());
            if (workingAnimation == null)
                workingAnimation = new Animation(cutsceneFilePath, 40, new EditorImageLoader());

            // Initalize
            selectedFrame = -1;
            useTransitonFlag = workingAnimation.isUseTransitions();
            slidesAnimationFlag = workingAnimation.isSlides();

            documentationTextContent = workingAnimation.getDocumentation();
            OnFrameSelectionChanged(selectedFrame);

            base.Init(e);
        }

        void OnGUI()
        {
            EditorGUILayout.PrefixLabel(TC.get("Animation.GeneralInfo"), GUIStyle.none, titleStyle);
            EditorGUI.BeginChangeCheck();
            documentationTextContent = EditorGUILayout.TextField(TC.get("Animation.Documentation"), documentationTextContent);
            if (EditorGUI.EndChangeCheck()) workingAnimation.setDocumentation(documentationTextContent);

            EditorGUI.BeginChangeCheck();
            useTransitonFlag = EditorGUILayout.Toggle(TC.get("Animation.UseTransitions"), useTransitonFlag);
            if (EditorGUI.EndChangeCheck()) OnUseTransitonFlagLastChanged(useTransitonFlag);

            EditorGUI.BeginChangeCheck();
            slidesAnimationFlag = EditorGUILayout.Toggle(TC.get("Animation.Slides"), slidesAnimationFlag);
            if(EditorGUI.EndChangeCheck()) OnSlidesAnimationFlagLastChanged(slidesAnimationFlag);

            /*
             * Transition panel
             */
            EditorGUILayout.PrefixLabel(TC.get("Animation.Timeline"), GUIStyle.none, titleStyle);
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, true, false, GUILayout.Height(125));
            EditorGUILayout.BeginHorizontal();
            GUI.skin = noBackgroundSkin;
            for (int i = 0; i < workingAnimation.getFrames().Count; i++)
            {
                if (selectedFrame == i)
                    GUI.skin = selectedFrameSkin;

                var frame = workingAnimation.getFrame(i);
                var frameContent = new GUIContent(frame.getTime().ToString(), frame.getImage(false, false, global::uAdventure.Core.Animation.ENGINE).texture);
                if (GUILayout.Button(frameContent, GUILayout.Height(100), GUILayout.Width(80)))
                {
                    OnFrameSelectionChanged(i);
                }
                if (useTransitonFlag && i != workingAnimation.getFrames().Count-1)
                {
                    var transition = workingAnimation.getTranstionForFrame(i);
                    var transitionContent = new GUIContent(transition.getTime().ToString(), transitionTypeTexture[transition.getType()]);
                    if (GUILayout.Button(transitionContent, GUILayout.Height(100), GUILayout.Width(80)))
                    {
                        OnFrameSelectionChanged(i);
                    }
                }
                GUI.skin = noBackgroundSkin;
            }
            GUI.skin = defaultSkin;
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndScrollView();


            /*
              * Transition button panel
              */
            EditorGUILayout.BeginHorizontal();
            GUI.skin = noBackgroundSkin;
            GUILayout.FlexibleSpace();

            GUI.enabled = selectedFrame > 0;
            if (GUILayout.Button(moveLeft))
            {
                workingAnimation.moveLeft(selectedFrame);
                selectedFrame--;
            }

            GUI.enabled = selectedFrame >= 0;
            if (GUILayout.Button(clearImg))
            {
                workingAnimation.removeFrame(selectedFrame);
            }

            GUI.enabled = true;
            if (GUILayout.Button(addTexture))
            {
                workingAnimation.addFrame(selectedFrame, null);
            }

            GUI.enabled = selectedFrame >= 0;
            if (GUILayout.Button(duplicateImg))
            {
                workingAnimation.addFrame(selectedFrame, workingAnimation.getFrame(selectedFrame));

            }
            GUI.enabled = selectedFrame < workingAnimation.getFrames().Count - 1;
            if (GUILayout.Button(moveRight))
            {
                workingAnimation.moveRight(selectedFrame);
                selectedFrame++;
            }

            GUI.skin = defaultSkin;
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();


            GUI.enabled = selectedFrame != -1;
            /*
             * Frame info panel
             */
            EditorGUILayout.PrefixLabel(TC.get("Animation.Details"), GUIStyle.none, titleStyle);

            EditorGUI.BeginChangeCheck();
            frameDocumentation = EditorGUILayout.TextField(TC.get("Animation.Documentation"), frameDocumentation);
            if (EditorGUI.EndChangeCheck()) OnFrameDocumentationChanged(frameDocumentation);

            EditorGUI.BeginChangeCheck();
            animationDuration = EditorGUILayout.LongField(TC.get("Animation.Duration"), animationDuration);
            if (EditorGUI.EndChangeCheck()) OnFrameDurationChanged(animationDuration);
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel(TC.get("Animation.Image"));
            if (GUILayout.Button(clearImg, GUILayout.Width(clearImg.width + 20)))
            {
            }
            GUILayout.Box(imagePath, GUILayout.ExpandWidth(true));
            if (GUILayout.Button(TC.get("Buttons.Select"), GUILayout.Width(GUI.skin.label.CalcSize(new GUIContent(TC.get("Buttons.Select"))).x + 20)))
            {
                ImageFileOpenDialog imageDialog =
                    (ImageFileOpenDialog)ScriptableObject.CreateInstance(typeof(ImageFileOpenDialog));
                imageDialog.Init(this, BaseFileOpenDialog.FileType.FRAME_IMAGE);
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel(TC.get("Animation.Sound"));
            if (GUILayout.Button(clearImg, GUILayout.Width(clearImg.width + 20)))
            {
            }
            GUILayout.Box(soundPath, GUILayout.ExpandWidth(true));
            if (GUILayout.Button(TC.get("Buttons.Select"), GUILayout.Width(GUI.skin.label.CalcSize(new GUIContent(TC.get("Buttons.Select"))).x + 20)))
            {
                MusicFileOpenDialog musicDialog =
                    (MusicFileOpenDialog)ScriptableObject.CreateInstance(typeof(MusicFileOpenDialog));
                musicDialog.Init(this, BaseFileOpenDialog.FileType.FRAME_MUSIC);
            }
            EditorGUILayout.EndHorizontal();

            GUI.enabled = useTransitonFlag && selectedFrame != -1 && selectedFrame < workingAnimation.getFrames().Count - 1;

            EditorGUILayout.PrefixLabel(TC.get("NextScene.Transition"), GUIStyle.none, titleStyle);
            EditorGUI.BeginChangeCheck();
            transitionDuration = EditorGUILayout.LongField(TC.get("Animation.Duration"), transitionDuration);
            if (EditorGUI.EndChangeCheck()) OnTransitionDurationChanged(transitionDuration);

            EditorGUI.BeginChangeCheck();
            transitionType = EditorGUILayout.Popup(TC.get("Conditions.Type"), transitionType, transitionTypeName); // TODO create a type in TC for transition 
            if (EditorGUI.EndChangeCheck()) OnTransitionTypeChanged(transitionType);

            GUILayout.FlexibleSpace();

            EditorGUILayout.BeginHorizontal();
            GUI.enabled = true;
            if (GUILayout.Button("OK"))
            {
                AnimationWriter.writeAnimation(cutscenePath, workingAnimation);
                reference.OnDialogOk("", this);
                this.Close();
            }
            if (GUILayout.Button(TC.get("GeneralText.Cancel")))
            {
                reference.OnDialogCanceled();
                this.Close();
            }
            EditorGUILayout.EndHorizontal();
        }

        private void OnFrameSelectionChanged(int i)
        {
            selectedFrame = (i == selectedFrame) ? -1 : i;

            if (selectedFrame != -1)
            {
                var frame = workingAnimation.getFrame(selectedFrame);
                imagePath = frame.getUri();
                soundPath = frame.getSoundUri();

                frameDocumentation = frame.getDocumentation();

                animationDuration = frame.getTime();
                transitionDuration = workingAnimation.getTranstionForFrame(selectedFrame).getTime();
                transitionType = workingAnimation.getTranstionForFrame(selectedFrame).getType();
            }
            else
            {
                imagePath = soundPath = frameDocumentation = string.Empty;
                animationDuration = transitionDuration = 0; 
            }

        }

        private void OnSlidesAnimationFlagLastChanged(bool val)
        {
            workingAnimation.setSlides(val);
        }

        private void OnUseTransitonFlagLastChanged(bool val)
        {
            workingAnimation.setUseTransitions(val);
        }

        private void OnFrameDurationChanged(long dur)
        {
            animationDuration = System.Math.Max(0, dur);
            workingAnimation.getFrame(selectedFrame).setTime(animationDuration);
        }

        private void OnTransitionDurationChanged(long dur)
        {
            transitionDuration = System.Math.Max(0, dur);
            workingAnimation.getTranstionForFrame(selectedFrame).setTime(transitionDuration); 
        }
        
        private void OnTransitionTypeChanged(int type)
        {
            workingAnimation.getTranstionForFrame(selectedFrame).setType(type);
        }

        private void OnFrameImageChanged(string val)
        {
            imagePath = val;
            workingAnimation.getFrame(selectedFrame).setUri(val);
        }

        private void OnFrameMusicChanged(string val)
        {
            soundPath = val;
            workingAnimation.getFrame(selectedFrame).setSoundUri(val);
        }
        private void OnFrameDocumentationChanged(string val)
        {
            workingAnimation.getFrame(selectedFrame).setDocumentation(val);
        }

        public void OnDialogOk(string message, object workingObject = null, object workingObjectSecond = null)
        {
            if (workingObject is BaseFileOpenDialog.FileType)
            {
                switch ((BaseFileOpenDialog.FileType)workingObject)
                {
                    case BaseFileOpenDialog.FileType.FRAME_IMAGE:
                        OnFrameImageChanged(message);
                        break;
                    case BaseFileOpenDialog.FileType.FRAME_MUSIC:
                        OnFrameMusicChanged(message);
                        break;
                }
            }
        }

        public void OnDialogCanceled(object workingObject = null)
        {

        }
    }
}