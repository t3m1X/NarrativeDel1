﻿/*
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

using System;
using System.Collections.Generic;
using UnityEngine;

namespace RayWenderlich.KQClone.Core
{
    public class InteractableObject : MonoBehaviour
    {
        [System.Serializable]
        public struct InterectableState {
            public string identifier;
            [TextArea] public string lookDialogue;
            public Interaction[] worldInteractions;
        }

        [System.Serializable]
        public struct Interaction {
            public string[] verbs;
            [TextArea] public string dialogue;
            [TextArea] public string awayDialogue;
            public UnityEngine.Events.UnityEvent actions;
        }

        [SerializeField] private float m_awayMinDistance = 1f;
        [SerializeField] private string m_currentStateKey = "default";
        [SerializeField] private InterectableState[] m_states = null;
        [SerializeField] private bool m_isAvailable = true;
        private Dictionary<string, InterectableState> m_stateDic = new Dictionary<string, InterectableState>();

        public string LookDialogue => m_stateDic[m_currentStateKey].lookDialogue;
        public bool IsAvailable { get => m_isAvailable; set => m_isAvailable = value; }

        public void ChangeState(string newStateId) {
            m_currentStateKey = newStateId;
        }

        public string ExecuteAction(string verb) {
            return ExecuteActionOnState(m_stateDic[m_currentStateKey].worldInteractions, verb);
        }

        private void Awake() {
            foreach (InterectableState state in m_states) {
                m_stateDic.Add(state.identifier.Trim(), state);
            }
        }

        private string ExecuteActionOnState(Interaction[] stateInteractions, string verb) {
            foreach (Interaction interaction in stateInteractions) {
                if (Array.IndexOf(interaction.verbs, verb) != 1) {
                    if (interaction.awayDialogue != string.Empty && Vector2.Distance(GameObject.FindGameObjectWithTag("Player").transform.position, transform.position) >= m_awayMinDistance) {
                        return interaction.awayDialogue;
                    }
                    else {
                        interaction.actions?.Invoke();
                        return interaction.dialogue;
                    }
                }
            }

            return "You can't do that";
        }
    }
}
