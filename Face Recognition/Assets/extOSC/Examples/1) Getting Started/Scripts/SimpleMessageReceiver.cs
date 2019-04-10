/* Copyright (c) 2018 ExT (V.Sigalkin) */

using UnityEngine;

namespace extOSC.Examples
{
    public class SimpleMessageReceiver : MonoBehaviour
    {
        #region Public Vars

        public string Address = "/message";

        [Header("OSC Settings")]
        public OSCReceiver Receiver;

        #endregion

        #region Unity Methods

        protected virtual void Start()
        {
            Receiver.Bind(Address, ReceivedMessage);
        }

        #endregion

        #region Private Methods

        private void ReceivedMessage(OSCMessage message)
        {
            //Debug.LogFormat("Received: {0}", message);
			int value;
			if (message.ToInt(out value))
			{
				//Debug.Log (value);
				if (value.ToString () == "1") {
					//Debug.Log ("Activating...");
					CloudFaceDetector.instance.OnCameraClick ();
				} else {
					Debug.Log ("Received a differente msessage from 1");
				}
			}//			if (]) {
//			
//			}
        }

        #endregion
    }
}