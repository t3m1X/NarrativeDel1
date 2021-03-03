using System.Collections.Generic;
using UnityEngine;

namespace RayWenderlich.KQClone.Core {
[RequireComponent(typeof(InteractableObject))]
public class ApplePicker : MonoBehaviour
{
    private InteractableObject m_interactableObject;
    [SerializeField] private List<SpriteRenderer> MyApples;
    private int m_nApples = 0;

    private void Awake() {
        m_interactableObject = GetComponent<InteractableObject>();
    }

    public void RemoveApple() {
        if (m_nApples < MyApples.Count) {
            MyApples[m_nApples++].enabled = false;
            m_interactableObject.ChangeState("default");

        }
        else {
            m_interactableObject.ChangeState("empty");
        }
    }

    public void AddApple() {
        if (m_nApples > 0) {
            MyApples[--m_nApples].enabled = true;
            if (m_nApples == 0)
                m_interactableObject.ChangeState("full");
            else
                m_interactableObject.ChangeState("default");
        }
    }
}

} //namespace RayWenderlich.KQClone.Utilities
