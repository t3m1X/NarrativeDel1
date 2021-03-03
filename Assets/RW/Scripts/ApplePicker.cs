using System.Collections.Generic;
using UnityEngine;

namespace RayWenderlich.KQClone.Core {
[RequireComponent(typeof(InteractableObject))]
public class ApplePicker : MonoBehaviour
{
    private InteractableObject m_interactableObject;
    [SerializeField] private List<SpriteRenderer> MyApples;

    private void Awake() {
        m_interactableObject = GetComponent<InteractableObject>();
    }

    public void RemoveApple() {
        if (MyApples.Count > 0)
        {
            MyApples[0].enabled = false;
            MyApples.RemoveAt(0);
        }
        else {
            m_interactableObject.ChangeState("empty");
        }
    }
}

} //namespace RayWenderlich.KQClone.Utilities
