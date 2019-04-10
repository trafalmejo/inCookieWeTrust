using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Text;
using RedScarf.EasyCSV.Demo;

public class CloudFaceDetector : MonoBehaviour
{
	[Tooltip("Image source component used for making camera shots.")]
	public WebcamSource imageSource;

	[Tooltip("Image component used for rendering camera shots")]
    public RawImage cameraShot;

	[Tooltip("Whether to recognize the emotions of the detected faces, or not.")]
	public bool recognizeEmotions = false;

//	[Tooltip("Whether to draw rectangles around the detected faces on the picture.")]
//	public bool displayFaceRectangles = true;

//	[Tooltip("Whether to draw arrow pointing to the head direction.")]
//	public bool displayHeadDirection = false;

	[Tooltip("Text component used for displaying hints and status messages.")]
    public Text hintText;

    [Tooltip("Text component used to display face-detection results.")]
    public Text resultText;

    // whether webcamSource has been set or there is web camera at all
    private bool hasCamera = false;

    // initial hint message
    private string hintMessage;

    // AspectRatioFitter component;
    private AspectRatioFitter ratioFitter;

	// CSV
	public GameObject CSVreader;
	public bool connection = false;
	GameObject mess;
	private float blackTime = 41;


	public static CloudFaceDetector instance = null;

	void Awake(){
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy (gameObject);
	
	}
    void Start()
    {
		mess = GameObject.Find ("fortuneText");
        if (cameraShot)
        {
            ratioFitter = cameraShot.GetComponent<AspectRatioFitter>();
        }

		hasCamera = imageSource != null && imageSource.HasCamera();

        hintMessage = hasCamera ? "Click on the camera image to make a shot" : "No camera found";
        
        SetHintText(hintMessage);
    }
	void Update(){
		if (Input.GetKeyDown (KeyCode.Space)) {
			OnCameraClick ();
		}
		if(Application.internetReachability != NetworkReachability.NotReachable){
			connection = true;
		}else{
			connection = false;

		}
		//Debug.Log ("internet connection:" + Network.conne);

		}

    // camera panel onclick event handler
    public void OnCameraClick()
    {
		backgroundWhite ();
		Invoke("backgroundBlack", blackTime);
		if(connection){
			Debug.Log ("CONNECTION");
        if (!hasCamera) 
			return;
        
        if (DoCameraShot())
        {
			ClearResultText();
            StartCoroutine(DoFaceDetection());
        }
		}else{
			Debug.Log ("NO CONNECTION");
			string message = CSVreader.GetComponent<CsvTest> ().getMessage (35);
			mess.GetComponent<Text> ().text = message;

		}
    }

    // camera-shot panel onclick event handler
    public void OnShotClick()
    {
        if (DoImageImport())
        {
			ClearResultText();
            StartCoroutine(DoFaceDetection());
        }
    }

    // camera shot step
    private bool DoCameraShot()
    {
        if (cameraShot != null && imageSource != null)
        {
            SetShotImageTexture(imageSource.GetImage());
            return true;
        }

        return false;
    }

    // imports image and displays it on the camera-shot object
    private bool DoImageImport()
    {
        Texture2D tex = FaceDetectionUtils.ImportImage();
        if (!tex) return false;

        SetShotImageTexture(tex);

        return true;
    }

    // performs face detection
    private IEnumerator DoFaceDetection()
    {
        // get the image to detect
        Face[] faces = null;
        Texture2D texCamShot = null;

        if (cameraShot)
        {
			texCamShot = (Texture2D)cameraShot.texture;
            SetHintText("Wait...");
        }

        // get the face manager instance
		CloudFaceManager faceManager = CloudFaceManager.Instance;

        if (!faceManager)
        {
            SetHintText("Check if the FaceManager component exists in the scene.");
        }
        else if(texCamShot)
        {
			byte[] imageBytes = texCamShot.EncodeToJPG();
			yield return null;

			//faces = faceManager.DetectFaces(texCamShot);
			AsyncTask<Face[]> taskFace = new AsyncTask<Face[]>(() => {
				return faceManager.DetectFaces(imageBytes);
			});

			taskFace.Start();
			yield return null;

			while (taskFace.State == TaskState.Running)
			{
				yield return null;
			}

			if(string.IsNullOrEmpty(taskFace.ErrorMessage))
			{
				faces = taskFace.Result;

				if(faces != null && faces.Length > 0)
				{
					// stick to detected face rectangles
					FaceRectangle[] faceRects = new FaceRectangle[faces.Length];

					for(int i = 0; i < faces.Length; i++)
					{
						faceRects[i] = faces[i].faceRectangle;
					}

					yield return null;

					// get the emotions of the faces
					if(recognizeEmotions)
					{
						//Emotion[] emotions = faceManager.RecognizeEmotions(texCamShot, faceRects);

						//emotion api
						AsyncTask<Emotion[]> taskEmot = new AsyncTask<Emotion[]>(() => {
							return faceManager.RecognizeEmotions(imageBytes, faceRects);
						});

						taskEmot.Start();
						yield return null;

						while (taskEmot.State == TaskState.Running)
						{
							yield return null;
						}

						if(string.IsNullOrEmpty(taskEmot.ErrorMessage))
						{
							Emotion[] emotions = taskEmot.Result;
							int matched = faceManager.MatchEmotionsToFaces(ref faces, ref emotions);

							if(matched != faces.Length)
							{
								Debug.Log(string.Format("Matched {0}/{1} emotions to {2} faces.", matched, emotions.Length, faces.Length));
							}
						}
						else
						{
							SetHintText(taskEmot.ErrorMessage);
						}
					}

					CloudFaceManager.DrawFaceRects(texCamShot, faces, FaceDetectionUtils.FaceColors);
					//SetHintText("Click on the camera image to make a shot");
					SetHintText(hintMessage);
					SetResultText(faces);
				}
				else
				{
					SetHintText("No face(s) detected.");
                    string message = CSVreader.GetComponent<CsvTest>().getMessage(35);
                    mess.GetComponent<Text>().text = message;
                }
			}
			else
			{
				SetHintText(taskFace.ErrorMessage);
			}
        }

        yield return null;
    }

    // display image on the camera-shot object
    private void SetShotImageTexture(Texture2D tex)
    {        
        if (ratioFitter)
        {
            ratioFitter.aspectRatio = (float)tex.width / (float)tex.height;
        }

        if (cameraShot)
        {
            cameraShot.texture = tex;
        }
    }
	// display results
	private void SetResultText01(Face[] faces)
	{
		StringBuilder sbResult = new StringBuilder();

		if (faces != null && faces.Length > 0)
		{
			for (int i = 0; i < faces.Length; i++)
			{
				Face face = faces[i];
				string faceColorName = FaceDetectionUtils.FaceColorNames[i % FaceDetectionUtils.FaceColors.Length];

				string[] r = FaceDetectionUtils.FaceToString (face, faceColorName);
				string res = r[0];


				sbResult.Append(string.Format("<color={0}>{1}</color>", faceColorName, res));
			}
		}

		string result = sbResult.ToString();

		if (resultText)
		{
			resultText.text = result;
			//resultText.text = faces[0].ToString();
		}
		else
		{
			Debug.Log(result);
		}
	}
    // display results
    private void SetResultText(Face[] faces)
    {
        StringBuilder sbResult = new StringBuilder();
		int maxWidth = 0;
		Face closer = null;
		int closerId = 0;
		string c = "";
        if (faces != null && faces.Length > 0)
        {
            for (int i = 0; i < faces.Length; i++)
            {
                Face face = faces[i];
				if (face.faceRectangle.width > maxWidth) {
					closer = faces [i];
					maxWidth = face.faceRectangle.width;
					closerId = i;
				}

                //string faceColorName = FaceDetectionUtils.FaceColorNames[i % FaceDetectionUtils.FaceColors.Length];

                //string res = FaceDetectionUtils.FaceToString(face, faceColorName);

                //sbResult.Append(string.Format("<color={0}>{1}</color>", faceColorName, res));
            }
			if(faces[closerId] != null){
				Face face = faces [closerId];
				string faceColorName = FaceDetectionUtils.FaceColorNames[closerId];
				string res = FaceDetectionUtils.FaceToString(face, faceColorName)[0];
				c = FaceDetectionUtils.FaceToString (face, faceColorName) [1];

				sbResult.Append(string.Format("<color={0}>{1}</color>", faceColorName, res));
			}
        }

        string result = sbResult.ToString();



		Debug.Log (c);
		string[] separators = new string[1];
		separators[0] = "-";
		string[] categoryString = c.Split(separators, System.StringSplitOptions.None);
		int category = 35;
		//Debug.Log (categoryString[0]);
		if(connection){
		category = CSVreader.GetComponent<CsvTest>().categoryToColumn (categoryString [0]);
		}
		bool isage = false;
		//Debug.Log (categoryString.Length);
		//Debug.Log (categoryString[1]);


		string message = CSVreader.GetComponent<CsvTest> ().getMessage (category);
		if (categoryString.Length > 1) {
			//Debug.Log ("is age");
			isage = true;
			message = message.Replace ("(age)", categoryString [1]);
		}
		//message = message.ToLower();
		GameObject dummy = GameObject.Find ("fortuneText");

		if (dummy != null) {
			dummy.GetComponent<Text> ().text = message;

			//dummy.GetComponent<Text> ().text = "You’re looking good and feeling young on Wednesday even though you're in late 50s. That new shirt will earn you some free drinks and envious glares from everyone in the office.";
		}
		if (resultText)
        {
            resultText.text = result;
			//resultText.text = faces[0].ToString();
			if (closer != null) {
				//resultText.text = "Closer face is: "+ closer.faceAttributes.gender;
			}
        }
        else
        {
            Debug.Log(result);
        }
    }
	void backgroundBlack(){
		Debug.Log ("black");
		GameObject back = GameObject.Find ("Whitescreen");
		GameObject camera = GameObject.Find ("Main Camera");
		camera.GetComponent<Camera> ().backgroundColor = Color.black;
		back.GetComponent<Image> ().color = Color.black;

	}
	void backgroundWhite(){
		Debug.Log ("white");

		GameObject back = GameObject.Find ("Whitescreen");
		GameObject camera = GameObject.Find ("Main Camera");
		back.GetComponent<Image> ().color = Color.white;
		camera.GetComponent<Camera> ().backgroundColor = Color.white;

	}
    // clear result
    private void ClearResultText()
    {
        if (resultText)
        {
            resultText.text = "";
        }
    }

    // displays hint or status text
    private void SetHintText(string sHintText)
    {
        if (hintText)
        {
            hintText.text = sHintText;
        }
        else
        {
            Debug.Log(sHintText);
        }
    }

}
