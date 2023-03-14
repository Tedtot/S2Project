using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Interactor : MonoBehaviour
{
    /*public Transform interactionPoint;
    public LayerMask interactionLayer;
    public float interactionPointRadius;*/
    public bool isInteracting { get; private set; }
    /*private void Update() {
        var colliders = Physics.OverlapSphere(interactionPoint.position, interactionPointRadius, interactionLayer);

        if (Keyboard.current.eKey.wasPressedThisFrame) {
            for (int i = 0; i < colliders.Length; i++) {
                var interactable = colliders[i].GetComponent<IInteractable>();

                if (interactable != null) {
                    startInteraction(interactable);
                }
            }
        }
    }*/

    void startInteraction(IInteractable interactable) {
        interactable.interact(this, out bool interactSuccessful);
        isInteracting = true;
    }
    void endInteraction() {
        isInteracting = false;
    }
    void OnCollisionStay2D(Collision2D collision) {
        if (collision.gameObject.layer == 7 && Keyboard.current.eKey.isPressed) {
            GameObject gObject = collision.gameObject;
            var interactable = gObject.GetComponent<IInteractable>();

            if (interactable != null) {
                startInteraction(interactable);

                //Changes gridlayout size based on size of chest
                int invSize = gObject.GetComponent<ChestInventory>().InvSize;
                GameObject di = GameObject.Find("DynamicInventory");
                GridLayoutGroup glg = di.GetComponent<GridLayoutGroup>();
                /*if (invSize % 2 == 0) {
                    glg.constraintCount = 4;
                    di.transform.position = new Vector2(541, di.transform.position.y);
                } 
                else {
                    glg.constraintCount = 3;
                    di.transform.position = new Vector2(500, di.transform.position.y);
                }*/
                //
            }
        }
    }
}
