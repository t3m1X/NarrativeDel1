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

using System.Collections;
using UnityEngine;

namespace RayWenderlich.KQClone.Core
{
    [RequireComponent(typeof(Animator), typeof(SetSortingOrder))]
    public class CharacterMovement : MonoBehaviour
    {

        [SerializeField] private float m_speed = 2f;
        private SetSortingOrder m_sortingScript;
        private Animator m_animator;
        private Vector2 m_currentDirection = Vector2.zero;
        private IEnumerator m_corroutine;

        private void Awake() {
            m_animator = GetComponent<Animator>();
            m_sortingScript = GetComponent<SetSortingOrder>();
            m_animator.speed = 0;
        }

        private void Update() {
            if (Input.GetKeyDown(KeyCode.UpArrow)) ToggleMovement(Vector2.up);
            if (Input.GetKeyDown(KeyCode.LeftArrow)) ToggleMovement(Vector2.left);
            if (Input.GetKeyDown(KeyCode.DownArrow)) ToggleMovement(Vector2.down);
            if (Input.GetKeyDown(KeyCode.RightArrow)) ToggleMovement(Vector2.right);

        }

        private void StopMovement() {
            m_animator.speed = 0;
            StopAllCoroutines();
        }

        private void ToggleMovement(Vector2 direction) {
            StopMovement();

            if (m_currentDirection != direction) {
                m_animator.speed = 1;
                m_animator.SetInteger("X", (int)direction.x);
                m_animator.SetInteger("Y", (int)direction.y);
                StartCoroutine(MovementRoutine(direction));

                m_currentDirection = direction;
            }
            else {
                m_currentDirection = Vector2.zero;
            }
        }
    
        private IEnumerator MovementRoutine(Vector2 direction) {
            while(true) {
                transform.Translate(direction * m_speed * Time.deltaTime);
                m_sortingScript.SetOrder();
                yield return null;
            }

        }


        private void OnCollisionEnter2D(Collision2D other) {
            StopMovement();
        }
    }
}