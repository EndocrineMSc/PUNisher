using System;
using UnityEngine;

public class JamEvents
{
    #region Environment

    public static event Action<InteractableObject> OnInteractableObjectClicked;

    public static void TriggerInteractableObjectClicked(InteractableObject interactableObject) {
        OnInteractableObjectClicked.Invoke(interactableObject);
    }

    #endregion

    #region Dialogue

    public static event Action OnEnteredDialogue;

    public static void TriggerEnterDialogue() {
        OnEnteredDialogue.Invoke();
    }

    public static event Action<bool> OnDialogueFinished;

    public static void TriggerDialogueFinished(bool isFinalDialogue) {
        OnDialogueFinished.Invoke(isFinalDialogue);
    }

    #endregion


}
