using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerVibrationManager : MonoBehaviour
{
    #region Attributes
    [System.Serializable]
    public struct VibratingController
    {
        #region Attributes
        private bool isVibrating;
        private float intensity;
        private float duration;
        private PlayerController player;
        #endregion

        #region Properties
        public PlayerController Player     
        {
            get {
                return player;
            }
        }

        public bool IsVibrating 
        {
            get {
                return isVibrating;
            }
            set {
                isVibrating = value;
            }
        }

        public float Intensity 
        {
            get {
                return intensity;
            }
        }

        public float Duration 
        {
            get {
                return duration;
            }
        }
        #endregion

        #region Initializing
        public VibratingController(PlayerController pController, float intensity, float duration)
        {
            isVibrating = false;
            player = pController;
            this.intensity = intensity;
            this.duration = duration;
        }
        #endregion
    }

    List<VibratingController> vibratingControllers;
    #endregion

    #region Initializing
    void Start()
    {
        vibratingControllers = new List<VibratingController>();
    }
    #endregion

    public void StartControllerVibration(PlayerController pController, float intensity, float duration)
    {
        bool foundMatch = false;
        for (int i = 0; i < vibratingControllers.Count; i++) {
            if(!foundMatch) {
                foundMatch = vibratingControllers[i].Player == pController;
                if (foundMatch) break;
            }
                
        }

        if (!foundMatch) {
            VibratingController vController = new VibratingController(pController, intensity, duration);
            vibratingControllers.Add(vController);
            StartCoroutine(VibrateController(vController));
        }
    }

    IEnumerator VibrateController(VibratingController vController)
    {
        if (!vController.IsVibrating) {
            vController.IsVibrating = true;
            PlayerActions playerActions = vController.Player.inputcontroller.playerActions;
            playerActions.Device.Vibrate(vController.Intensity);
            yield return new WaitForSeconds(vController.Duration);
            playerActions.Device.StopVibration();
            vController.IsVibrating = false;

            vibratingControllers.Remove(vController);
            vibratingControllers.TrimExcess();
        }
    }
}
