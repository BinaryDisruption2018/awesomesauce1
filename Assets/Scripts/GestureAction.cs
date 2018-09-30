// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using HoloToolkit.Unity.InputModule;
using UnityEngine;
using System.Collections;
using System.IO;

namespace Academy
{

    /// <summary>
    /// GestureAction performs custom actions based on 
    /// which gesture is being performed.
    /// </summary>
    public class GestureAction : MonoBehaviour, INavigationHandler, IManipulationHandler, ISpeechHandler
    {
        public ArrayList textures = new ArrayList();
        private int currentViewIndex = 0;
        public GameObject theEarth;
        
        /*
        public void Start()
        {
            
        }
        */

        [Tooltip("Rotation max speed controls amount of rotation.")]
        [SerializeField]
        private float RotationSensitivity = 10.0f;

        private bool isNavigationEnabled = true;
        public bool IsNavigationEnabled
        {
            get { return isNavigationEnabled; }
            set { isNavigationEnabled = value; }
        }

        private Vector3 manipulationOriginalPosition = Vector3.zero;

        private void changeTexture(Texture2D texture)
        {
            Renderer renderer = GetComponent<Renderer>();

            Debug.Log(renderer.material.mainTexture);

            renderer.material.mainTexture = texture;
         }

        public static Texture2D LoadPNG(string filePath)
        {

            Texture2D tex = null;
            byte[] fileData;

            if (File.Exists(filePath))
            {
                fileData = File.ReadAllBytes(filePath);
                Debug.Log(fileData);
                tex = new Texture2D(2, 2);
                tex.LoadImage(fileData); //..this will auto-resize the texture dimensions.
            }
            return tex;
        }

        public void Awake()
        {
            string[] imagePaths = { "Assets/one.jpg", "Assets/two.jpg", "Assets/three.jpg", "Assets/four.jpg", "Assets/five.jpg" };

            foreach (string path in imagePaths)
            {
                textures.Add(LoadPNG(path));
                Debug.Log(textures[textures.Count - 1]);
            }

            Debug.Log("This ran.");
            //changeTexture((Texture2D)textures[2]);
            //changeTexture((Texture2D)textures[4]);
        }

        void INavigationHandler.OnNavigationStarted(NavigationEventData eventData)
        {
            InputManager.Instance.PushModalInputHandler(gameObject);
        }

        void INavigationHandler.OnNavigationUpdated(NavigationEventData eventData)
        {
            if (isNavigationEnabled)
            {
                /* TODO: DEVELOPER CODING EXERCISE 2.c */

                // 2.c: Calculate a float rotationFactor based on eventData's NormalizedOffset.x multiplied by RotationSensitivity.
                // This will help control the amount of rotation.
                float rotationFactor = eventData.NormalizedOffset.x * RotationSensitivity;

                // 2.c: transform.Rotate around the Y axis using rotationFactor.
                transform.Rotate(new Vector3(0, -1 * rotationFactor, 0));
            }
        }

        void INavigationHandler.OnNavigationCompleted(NavigationEventData eventData)
        {
            InputManager.Instance.PopModalInputHandler();
        }

        void INavigationHandler.OnNavigationCanceled(NavigationEventData eventData)
        {
            InputManager.Instance.PopModalInputHandler();
        }

        void IManipulationHandler.OnManipulationStarted(ManipulationEventData eventData)
        {
            if (!isNavigationEnabled)
            {
                InputManager.Instance.PushModalInputHandler(gameObject);

                manipulationOriginalPosition = transform.position;
            }
        }

        void IManipulationHandler.OnManipulationUpdated(ManipulationEventData eventData)
        {
            if (!isNavigationEnabled)
            {
                /* TODO: DEVELOPER CODING EXERCISE 4.a */

                // 4.a: Make this transform's position be the manipulationOriginalPosition + eventData.CumulativeDelta

            }
        }

        void IManipulationHandler.OnManipulationCompleted(ManipulationEventData eventData)
        {
            InputManager.Instance.PopModalInputHandler();
        }

        void IManipulationHandler.OnManipulationCanceled(ManipulationEventData eventData)
        {
            InputManager.Instance.PopModalInputHandler();
        }

        void ISpeechHandler.OnSpeechKeywordRecognized(SpeechEventData eventData)
        {
            if (eventData.RecognizedText.ToLower().Equals("move earth"))
            {
                isNavigationEnabled = false;
            }
            else if (eventData.RecognizedText.ToLower().Equals("rotate earth"))
            {
                isNavigationEnabled = true;
            }
            else if (eventData.RecognizedText.ToLower().Equals("change view"))
            {
                currentViewIndex += 1;
                if(currentViewIndex > textures.Count - 1)
                {
                    currentViewIndex = 0;
                }

                changeTexture((Texture2D)textures[currentViewIndex]);
            }
            else
            {
                return;
            }

            eventData.Use();
        }
    }
}