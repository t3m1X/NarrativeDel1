/*
 * Copyright (c) 2020 Razeware LLC
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 * Notwithstanding the foregoing, you may not use, copy, modify, merge, publish, 
 * distribute, sublicense, create a derivative work, and/or sell copies of the 
 * Software in any work that is designed, intended, or marketed for pedagogical or 
 * instructional purposes related to programming, coding, application development, 
 * or information technology.  Permission for such use, copying, modification,
 * merger, publication, distribution, sublicensing, creation of derivative works, 
 * or sale is expressly withheld.
 *    
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */

using System.Collections.Generic;
using UnityEngine;
using RayWenderlich.KQClone.Utilities;

namespace RayWenderlich.KQClone.Core
{
    [System.Serializable]
    public struct InteractableObjectLink {
        public string[] names;
        public InteractableObject interactableObject;
    }

    public class GameManager : MonoBehaviour
    {
        [SerializeField] private InteractableObjectLink[] m_objectArray = null;
        private UIManager m_uiManager;
        private Dictionary<string, InteractableObject> m_sceneDictionary;

        public void ExecuteCommand(string command) {
            var parsedCommand = CommandParser.Parse(command);

            if (string.IsNullOrEmpty(parsedCommand.verb)) {
                m_uiManager.ShowPopup("Enter a valid command.");
                return;
            }

            if (string.IsNullOrEmpty(parsedCommand.primaryEntity)) {
                m_uiManager.ShowPopup("You need to be more specific.");
                return;
            }

            if (m_sceneDictionary.ContainsKey(parsedCommand.primaryEntity)) {
                var sceneObject = m_sceneDictionary[parsedCommand.primaryEntity];
                if (sceneObject.IsAvailable)
                {
                    if (parsedCommand.verb == "look") m_uiManager.ShowPopup(sceneObject.LookDialogue);
                    else m_uiManager.ShowPopup(sceneObject.ExecuteAction(parsedCommand.verb));
                }
                else {
                    m_uiManager.ShowPopup("You can't do that - at least not now.");
                }
            }
            else {
                m_uiManager.ShowPopup($"I don't understand '{parsedCommand.primaryEntity}'.");
            }
        }

        private void Awake() {
            m_uiManager = GameManager.FindObjectOfType<UIManager>();
            m_sceneDictionary = new Dictionary<string, InteractableObject>();
            foreach (var item in m_objectArray) {
                foreach (string name in item.names) {
                    m_sceneDictionary.Add(name.ToLowerInvariant().Trim(), item.interactableObject);
                }
            }
        }
    }
}