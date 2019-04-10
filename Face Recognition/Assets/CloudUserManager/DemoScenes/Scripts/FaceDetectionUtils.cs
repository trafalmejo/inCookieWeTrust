using UnityEngine;
using System.IO;
using System.Text;
using System.Collections.Generic;

public static class FaceDetectionUtils 
{
    private static readonly Color[] faceColors = new Color[] { Color.green, Color.yellow, Color.cyan, Color.magenta, Color.red };
    private static readonly string[] faceColorNames = new string[] { "Green", "Yellow", "Cyan", "Magenta", "Red", };
	//private GameObject text = GameObject.Find("CVSReader").GetComponent<CvsTest>();                                  

    public static Texture2D ImportImage()
    {
        Texture2D tex = null;

#if UNITY_EDITOR
		string filePath = UnityEditor.EditorUtility.OpenFilePanel("Open image file", "", "jpg");  // string.Empty; // 
#else
		string filePath = string.Empty;
#endif

        if (!string.IsNullOrEmpty(filePath))
        {
            byte[] fileBytes = File.ReadAllBytes(filePath);

            tex = new Texture2D(2, 2);
            tex.LoadImage(fileBytes);
        }

        return tex;
    }

    public static string[] FaceToString(Face face, string faceColorName)
    {
        StringBuilder sbResult = new StringBuilder();

        sbResult.Append(string.Format("{0} face:", faceColorName)).AppendLine();
        sbResult.Append(string.Format("  • Gender: {0}", face.faceAttributes.gender)).AppendLine();
		sbResult.Append(string.Format("  • Age: {0}", face.faceAttributes.age)).AppendLine();
		sbResult.Append(string.Format("  • Bald: {0:F0}%", face.faceAttributes.hair.bald * 100f)).AppendLine();
		sbResult.Append(string.Format("  • Invisible: {0}", face.faceAttributes.hair.invisible)).AppendLine();
		if (face.faceAttributes.hair.hairColor.Length > 0) {
			for (int i = 0; i < face.faceAttributes.hair.hairColor.Length; i++) {
				sbResult.Append(string.Format("  • Color: {0}", face.faceAttributes.hair.hairColor[i].color)+ string.Format("  • Confidence: {0}", face.faceAttributes.hair.hairColor[i].confidence)).AppendLine();
			}
		}
		sbResult.Append(string.Format("  • Glasses: {0}", face.faceAttributes.glasses)).AppendLine();
		sbResult.Append(string.Format("  • Facial Hair_beard: {0}", face.faceAttributes.facialHair.beard)).AppendLine();
		sbResult.Append(string.Format("  • Facial Hair_moustache: {0}", face.faceAttributes.facialHair.moustache)).AppendLine();
		sbResult.Append(string.Format("  • Facial Hair_sideburns: {0}", face.faceAttributes.facialHair.sideburns)).AppendLine();
		sbResult.Append(string.Format("  • MakeUp_eyes: {0}", face.faceAttributes.makeup.eyeMakeup)).AppendLine();
		sbResult.Append(string.Format("  • MakeUp_lips: {0}", face.faceAttributes.makeup.lipMakeup)).AppendLine();
		sbResult.Append(string.Format("  • Oclussion_forehead: {0}", face.faceAttributes.occlusion.foreheadOccluded)).AppendLine();
		sbResult.Append(string.Format("  • Oclussion_eyes: {0}", face.faceAttributes.occlusion.eyeOccluded)).AppendLine();
		sbResult.Append(string.Format("  • Oclussion_mouth: {0}", face.faceAttributes.occlusion.mouthOccluded)).AppendLine();
		if (face.faceAttributes.accessories.Length > 0) {
			for (int i = 0; i < face.faceAttributes.accessories.Length; i++) {
				sbResult.Append(string.Format("  • Accessories: {0}", face.faceAttributes.accessories[i].type) + string.Format("  • Confidence: {0}", face.faceAttributes.accessories[i].confidence)).AppendLine();
			}
		}

		int randomSelection = 0;
		string criteria = "";
		if (face.faceAttributes.accessories.Length > 0) {
			//criteria ="ACCESSORIE:"+ face.faceAttributes.accessories [Random.Range (0, face.faceAttributes.accessories.Length)].type;
			string accesorie = face.faceAttributes.accessories [Random.Range (0, face.faceAttributes.accessories.Length)].type;
			if (accesorie == "glasses") {
				//criteria = "CAREFUL WITH YOUR GLASSES, THEY MAY BREAK IN YOUR WAY TO HOME";
				criteria = "ACCESSORY:GLASSES";

			}
			else if(accesorie == "headwear"){
				//criteria = "DON'T TAKE OFF THAT HEADWEAR FROM YOU HEAD, YOU WILL NEED IT IN THE NEXT HOUR.";
				criteria = "ACCESSORY:HEADWEAR";
			}

		} else {
			randomSelection = Random.Range (1, 4);

			if (randomSelection == 1) {
				// is AGE
				//criteria = "AGE:"+face.faceAttributes.age;
				float age = face.faceAttributes.age;
				criteria = "";

				if (age < 10) {
					//criteria = "YOU ARE TO YOUNG TO GET TO KNOW YOUR DARK FUTURE";
					criteria = "AGE:LESS10-"+age;
				}
				else if(age >= 10 && age <= 12){
					//criteria = "IN 30 YEARS PEOPLE AT YOUR AGE (" +(age) + ") WILL KNOW QUANTICS MATHS AND ROCKET SCIENCE, WHY DONT YOU START NOW?";
					criteria = "AGE:10to12(TWEENS)-"+age;
					//criteria = "IN 30 YEARS PEOPLE AT YOUR AGE (" + (age-5.0f) +"-"+ (age+5.0f) + ") WILL KNOW QUANTICS MATHS AND ROCKET SCIENCE, WHY DONT YOU START NOW?";
				}
				else if(age >= 13 && age <= 14){
					//criteria = "IN 10 YEARS PEOPLE AT YOUR AGE (" + (age) + ") WILL START HAVING THEIR FIRST CHILDREN,ANYWAY, MOST OF THEM DON'T HAVE ANY";
					criteria = "AGE:13to14(EARLY TEENS)-"+age;
					//criteria = "IN 30 YEARS PEOPLE AT YOUR AGE (" + (age-5.0f) +"-"+ (age+5.0f) + ") WILL START AHVING THEIR FIRST CHILDREN,ANYWAY, MOST OF THEM DON'T HAVE ANY";
				}
				else if(age >= 15 && age <= 17){
					//criteria = "IN 10 YEARS PEOPLE AT YOUR AGE (" + (age) + ") WILL START HAVING THEIR FIRST CHILDREN,ANYWAY, MOST OF THEM DON'T HAVE ANY";
					criteria = "AGE:15to17(MID TEENS)-"+age;
					//criteria = "IN 30 YEARS PEOPLE AT YOUR AGE (" + (age-5.0f) +"-"+ (age+5.0f) + ") WILL START AHVING THEIR FIRST CHILDREN,ANYWAY, MOST OF THEM DON'T HAVE ANY";
				}
				else if(age >= 18 && age <= 19){
					//criteria = "IN 10 YEARS PEOPLE AT YOUR AGE (" + (age) + ") WILL START HAVING THEIR FIRST CHILDREN,ANYWAY, MOST OF THEM DON'T HAVE ANY";
					criteria = "AGE:18to19(LATE TEENS)-"+age;
					//criteria = "IN 30 YEARS PEOPLE AT YOUR AGE (" + (age-5.0f) +"-"+ (age+5.0f) + ") WILL START AHVING THEIR FIRST CHILDREN,ANYWAY, MOST OF THEM DON'T HAVE ANY";
				}
				else if(age >= 20 && age <= 23){
					//criteria = "IN 10 YEARS PEOPLE AT YOUR AGE (" + (age) + ") WILL START HAVING THEIR FIRST CHILDREN,ANYWAY, MOST OF THEM DON'T HAVE ANY";
					criteria = "AGE:20to23(EARLY 20S)-"+age;
					//criteria = "IN 30 YEARS PEOPLE AT YOUR AGE (" + (age-5.0f) +"-"+ (age+5.0f) + ") WILL START AHVING THEIR FIRST CHILDREN,ANYWAY, MOST OF THEM DON'T HAVE ANY";
				}
				else if(age >= 24 && age <= 26){
					//criteria = "IN 10 YEARS PEOPLE AT YOUR AGE (" + (age) + ") WILL START HAVING THEIR FIRST CHILDREN,ANYWAY, MOST OF THEM DON'T HAVE ANY";
					criteria = "AGE:24to26(MID 20S)-"+age;
					//criteria = "IN 30 YEARS PEOPLE AT YOUR AGE (" + (age-5.0f) +"-"+ (age+5.0f) + ") WILL START AHVING THEIR FIRST CHILDREN,ANYWAY, MOST OF THEM DON'T HAVE ANY";
				}
				else if(age >= 27 && age <= 29){
					//criteria = "IN 10 YEARS PEOPLE AT YOUR AGE (" + (age) + ") WILL START HAVING THEIR FIRST CHILDREN,ANYWAY, MOST OF THEM DON'T HAVE ANY";
					criteria = "AGE:27to29(LATE 20S)-"+age;
					//criteria = "IN 30 YEARS PEOPLE AT YOUR AGE (" + (age-5.0f) +"-"+ (age+5.0f) + ") WILL START AHVING THEIR FIRST CHILDREN,ANYWAY, MOST OF THEM DON'T HAVE ANY";
				}
				else if(age >= 30 && age <= 33){
					//criteria = "IN 10 YEARS PEOPLE AT YOUR AGE (" + (age) + ") WILL START HAVING THEIR FIRST CHILDREN,ANYWAY, MOST OF THEM DON'T HAVE ANY";
					criteria = "AGE:30to33(EARLY 30S)-"+age;
					//criteria = "IN 30 YEARS PEOPLE AT YOUR AGE (" + (age-5.0f) +"-"+ (age+5.0f) + ") WILL START AHVING THEIR FIRST CHILDREN,ANYWAY, MOST OF THEM DON'T HAVE ANY";
				}
				else if(age >= 34 && age <= 36){
					//criteria = "IN 10 YEARS PEOPLE AT YOUR AGE (" + (age) + ") WILL START HAVING THEIR FIRST CHILDREN,ANYWAY, MOST OF THEM DON'T HAVE ANY";
					criteria = "AGE:34to36(MID 30S)-"+age;
					//criteria = "IN 30 YEARS PEOPLE AT YOUR AGE (" + (age-5.0f) +"-"+ (age+5.0f) + ") WILL START AHVING THEIR FIRST CHILDREN,ANYWAY, MOST OF THEM DON'T HAVE ANY";
				}
				else if(age >= 37 && age <= 39){
					//criteria = "IN 10 YEARS PEOPLE AT YOUR AGE (" + (age) + ") WILL START HAVING THEIR FIRST CHILDREN,ANYWAY, MOST OF THEM DON'T HAVE ANY";
					criteria = "AGE:37to39(LATE 30S)-"+age;
					//criteria = "IN 30 YEARS PEOPLE AT YOUR AGE (" + (age-5.0f) +"-"+ (age+5.0f) + ") WILL START AHVING THEIR FIRST CHILDREN,ANYWAY, MOST OF THEM DON'T HAVE ANY";
				}
				else if(age >= 40 && age <= 43){
					//criteria = "IN 10 YEARS PEOPLE AT YOUR AGE (" + (age) + ") WILL START HAVING THEIR FIRST CHILDREN,ANYWAY, MOST OF THEM DON'T HAVE ANY";
					criteria = "AGE:40to43(EARLY 40S)-"+age;
					//criteria = "IN 30 YEARS PEOPLE AT YOUR AGE (" + (age-5.0f) +"-"+ (age+5.0f) + ") WILL START AHVING THEIR FIRST CHILDREN,ANYWAY, MOST OF THEM DON'T HAVE ANY";
				}
				else if(age >=44  && age <= 46){
					//criteria = "IN 10 YEARS PEOPLE AT YOUR AGE (" + (age) + ") WILL START HAVING THEIR FIRST CHILDREN,ANYWAY, MOST OF THEM DON'T HAVE ANY";
					criteria = "AGE:44to46(MID 40S)-"+age;
					//criteria = "IN 30 YEARS PEOPLE AT YOUR AGE (" + (age-5.0f) +"-"+ (age+5.0f) + ") WILL START AHVING THEIR FIRST CHILDREN,ANYWAY, MOST OF THEM DON'T HAVE ANY";
				}
				else if(age >= 47 && age <= 49){
					//criteria = "IN 10 YEARS PEOPLE AT YOUR AGE (" + (age) + ") WILL START HAVING THEIR FIRST CHILDREN,ANYWAY, MOST OF THEM DON'T HAVE ANY";
					criteria = "AGE:47to49(LATE 40S)-"+age;
					//criteria = "IN 30 YEARS PEOPLE AT YOUR AGE (" + (age-5.0f) +"-"+ (age+5.0f) + ") WILL START AHVING THEIR FIRST CHILDREN,ANYWAY, MOST OF THEM DON'T HAVE ANY";
				}
				else if(age >= 50 && age <= 53){
					//criteria = "IN 10 YEARS PEOPLE AT YOUR AGE (" + (age) + ") WILL START HAVING THEIR FIRST CHILDREN,ANYWAY, MOST OF THEM DON'T HAVE ANY";
					criteria = "AGE:50to53(EARLY 50S)-"+age;
					//criteria = "IN 30 YEARS PEOPLE AT YOUR AGE (" + (age-5.0f) +"-"+ (age+5.0f) + ") WILL START AHVING THEIR FIRST CHILDREN,ANYWAY, MOST OF THEM DON'T HAVE ANY";
				}
				else if(age >= 54 && age <= 56){
					//criteria = "IN 10 YEARS PEOPLE AT YOUR AGE (" + (age) + ") WILL START HAVING THEIR FIRST CHILDREN,ANYWAY, MOST OF THEM DON'T HAVE ANY";
					criteria = "AGE:54to56(MID 50S)-"+age;
					//criteria = "IN 30 YEARS PEOPLE AT YOUR AGE (" + (age-5.0f) +"-"+ (age+5.0f) + ") WILL START AHVING THEIR FIRST CHILDREN,ANYWAY, MOST OF THEM DON'T HAVE ANY";
				}
				else if(age >= 56 && age <= 59){
					//criteria = "IN 10 YEARS PEOPLE AT YOUR AGE (" + (age) + ") WILL START HAVING THEIR FIRST CHILDREN,ANYWAY, MOST OF THEM DON'T HAVE ANY";
					criteria = "AGE:56to59(LATE 50S)-"+age;
					//criteria = "IN 30 YEARS PEOPLE AT YOUR AGE (" + (age-5.0f) +"-"+ (age+5.0f) + ") WILL START AHVING THEIR FIRST CHILDREN,ANYWAY, MOST OF THEM DON'T HAVE ANY";
				}
				else if(age >= 60){
					//criteria = "IN 10 YEARS PEOPLE AT YOUR AGE (" + (age) + ") WILL START HAVING THEIR FIRST CHILDREN,ANYWAY, MOST OF THEM DON'T HAVE ANY";
					criteria = "AGE:ABOVE60-"+age;
					//criteria = "IN 30 YEARS PEOPLE AT YOUR AGE (" + (age-5.0f) +"-"+ (age+5.0f) + ") WILL START AHVING THEIR FIRST CHILDREN,ANYWAY, MOST OF THEM DON'T HAVE ANY";
				}
					
			} else if (randomSelection == 2) {
				// is HAIR 
				//ITS BOLD
				if (face.faceAttributes.hair.bald > 0.5f) {
					//criteria = "BOLD:"+face.faceAttributes.hair.bald;
					//criteria = "EVOLUTION HAS TAUGH US, HUMAN BEING IS LOSING HAIR QUANTITY EVERY GENERATION. IN 30 YEARS PEOPLE WILL LOOK JUST LIKE YOU, NO HAIR UP THERE";
					criteria = "BOLD";

				} else {
					// It HAS HAIR
					if(face.faceAttributes.hair.hairColor.Length > 0){
						//criteria = "HAIR:"+ face.faceAttributes.hair.hairColor [0].color;
						string color = face.faceAttributes.hair.hairColor [0].color;
						if(color == "red"){
							//criteria = "IN 2040, RED HAIR WILL PROBABLY DESAPPEAR. LET ME TAKE SOME, WE WILL NEED IT LATER.";
							criteria = "HAIR:RED";
						}
						if(color == "gray"){
							//criteria = "IN 2040, GRAY HAIR AS YOURS WILL PROBABLY DESAPPEAR. LET ME TAKE SOME, WE WILL NEED IT LATER.";
							criteria = "HAIR:GRAY";
						}
						if(color == "brown"){
							//criteria = "IN 2040, BROWN HAIR WILL PROBABLY DESAPPEAR. LET ME TAKE SOME, WE WILL NEED IT LATER.";
							criteria = "HAIR:BROWN";
						}
						if(color == "black"){
							//criteria = "IN 2040, BLACK HAIR WILL PROBABLY DESAPPEAR. LET ME TAKE SOME, WE WILL NEED IT LATER. ";
							criteria = "HAIR:BLACK";
						}
						if(color == "blond"){
							//criteria = "BLOND HAIR WILL PROBABLY DESAPPEAR. LET ME TAKE SOME, WE WILL NEED IT LATER.";
							criteria = "HAIR:BLOND";
						}
						if(color == "other"){
							//criteria = "OHHHHHH, FIRST TIME I SEE THIS COLOR HAIR PASSING BY, I FORSEE IT WILL BE TREND ON 2030.";
							criteria = "HAIR:OTHER";
						}						

					}
				}
			} else if (randomSelection == 3) {
				// is GENDER
				int gender = Random.Range (0, 2);
				string g = face.faceAttributes.gender;
				if (gender == 0) {
					//IS GENDER
					//criteria = "GENDER:"+face.faceAttributes.gender;
					if (face.faceAttributes.gender == "female") {
						//criteria = "THE NEXT US PRESIDENT WILL BE A WOMAN, ARE YOU THE ONE?";
						criteria = "GENDER:FEMALE";
					}
					if (face.faceAttributes.gender == "male") {
						//criteria = "MAN LIKE YOU ARE NOT EASY TO FIND. YOUR LUCKY NUMBERS ARE: "+face.faceAttributes.age + " 20 55 35 98";
						criteria = "GENDER:MALE";
					}
				} else {
					//IS PHYSYCAL
					if (g == "male") {
						float b = face.faceAttributes.facialHair.beard;
						float m = face.faceAttributes.facialHair.moustache;
						//criteria = "EVOLUTION HAS TAUGH US, HUMAN BEING IS LOSING HAIR QUANTITY EVERY GENERATION. IN 30 YEARS PEOPLE WILL LOOK JUST LIKE YOU, NO HAIR IN FACE.";
						criteria = "GENDER:MALE";
						if (b > 0.65f) {
							///criteria = "FACIALHAIR:"+"beard:" +b;
							//criteria = "IN 20 YEARS FROM NOW, MORE BEARDED-PEOPLE WILL LIVE IN EARTH, SO YOU ARE LEADING THE CUTTING-EDGE FASHION TREN JUST PERFECTLY.";
							criteria = "FACIALHAIR:BEARD:Y";
						}
						else if (b < 0.15f) {
							///criteria = "FACIALHAIR:"+"beard:" +b;
							//criteria = "IN 20 YEARS FROM NOW, MORE BEARDED-PEOPLE WILL LIVE IN EARTH, SO YOU ARE LEADING THE CUTTING-EDGE FASHION TREN JUST PERFECTLY.";
							criteria = "GENDER:MALE";
						}
						else if (m > 0.65f) {
							///criteria = "FACIALHAIR:"+"beard:" +b;
							//criteria = "IN 20 YEARS FROM NOW, MORE BEARDED-PEOPLE WILL LIVE IN EARTH, SO YOU ARE LEADING THE CUTTING-EDGE FASHION TREN JUST PERFECTLY.";
							criteria = "FACIALHAIR:MOUSTACHE:Y";
						}
						else if (m < 0.15f) {
							///criteria = "FACIALHAIR:"+"beard:" +b;
							//criteria = "IN 20 YEARS FROM NOW, MORE BEARDED-PEOPLE WILL LIVE IN EARTH, SO YOU ARE LEADING THE CUTTING-EDGE FASHION TREN JUST PERFECTLY.";
							criteria = "GENDER:MALE";
						}
					}
					else if(g == "female"){
						bool eye = face.faceAttributes.makeup.eyeMakeup;
						bool lip = face.faceAttributes.makeup.lipMakeup;
						if(eye || lip){
							criteria = "MAKEUP:Y";
							//criteria = "MAKEUP:Y";
							//criteria = "BEAUTIFUL MAKEUP THE ONE YOU ARE USING. BE AWARE, IN THE FUTURE IT MAY BE ILLEGAL TO WEAR SOME.";
						}
						if (!eye && !lip) {
							criteria = "MAKEUP:N";
							//criteria = "MAKEUP:N";
							//criteria = "NO MAKE UP WILL CARE YOU SKIN HEALTH AND BESIDES YOU LOOK AMAZING WITHOUT IT.";
						}

					}
				}
			}
		}
		sbResult.Append ("CRITERIA WORD: " + criteria);
		//HAS ACCESSORY
		//AGE / BABY / TEENAGER 10 - 16/ ADULT 16-35/ MIDDLE ADULT 36 - 50 / OLD ADULT > 50
		//HAIR / NO HAIR / HAIR COLOR
		//GENDER
			//MAKEUP / NO MAKE UP
			//HAIRFACE / NO HAIRFACE





//			sbResult.Append(string.Format("  • Beard: {0}", face.FaceAttributes.FacialHair.Beard)).AppendLine();
//			sbResult.Append(string.Format("  • Moustache: {0}", face.FaceAttributes.FacialHair.Moustache)).AppendLine();
//			sbResult.Append(string.Format("  • Sideburns: {0}", face.FaceAttributes.FacialHair.Sideburns)).AppendLine().AppendLine();

		if(face.emotion != null && face.emotion.scores != null)
			sbResult.Append(string.Format("  • Emotion: {0}", GetEmotionScoresAsString(face.emotion))).AppendLine();

		sbResult.AppendLine();
		string[] messages = new string[2];
		messages [0] = sbResult.ToString ();
		messages [1] = criteria;

		return messages;
    }

	
	/// <summary>
	/// Gets the emotion scores as string.
	/// </summary>
	/// <returns>The emotion as string.</returns>
	/// <param name="emotion">Emotion.</param>
	public static string GetEmotionScoresAsString(Emotion emotion)
	{
		if(emotion == null || emotion.scores == null)
			return string.Empty;
		
		Scores es = emotion.scores; 
		StringBuilder emotStr = new StringBuilder();
		
		if(es.anger >= 0.01f) 
			emotStr.AppendFormat(" {0:F0}% angry,", es.anger * 100f);
		if(es.contempt >= 0.01f) 
			emotStr.AppendFormat(" {0:F0}% contemptuous,", es.contempt * 100f);
		if(es.disgust >= 0.01f) 
			emotStr.AppendFormat(" {0:F0}% disgusted,", es.disgust * 100f);
		if(es.fear >= 0.01f) 
			emotStr.AppendFormat(" {0:F0}% scared,", es.fear * 100f);
		if(es.happiness >= 0.01f) 
			emotStr.AppendFormat(" {0:F0}% happy,", es.happiness * 100f);
		if(es.neutral >= 0.01f) 
			emotStr.AppendFormat(" {0:F0}% neutral,", es.neutral * 100f);
		if(es.sadness >= 0.01f) 
			emotStr.AppendFormat(" {0:F0}% sad,", es.sadness * 100f);
		if(es.surprise >= 0.01f) 
			emotStr.AppendFormat(" {0:F0}% surprised,", es.surprise * 100f);
		
		if(emotStr.Length > 0)
		{
			emotStr.Remove(0, 1);
			emotStr.Remove(emotStr.Length - 1, 1);
		}
		
		return emotStr.ToString();
	}
	
	
	/// <summary>
	/// Gets the emotion scores as list of strings.
	/// </summary>
	/// <returns>The emotion as string.</returns>
	/// <param name="emotion">Emotion.</param>
	public static List<string> GetEmotionScoresList(Emotion emotion)
	{
		List<string> alScores = new List<string>();
		if(emotion == null || emotion.scores == null)
			return alScores;
		
		Scores es = emotion.scores; 
		
		if(es.anger >= 0.01f) 
			alScores.Add(string.Format("{0:F0}% angry", es.anger * 100f));
		if(es.contempt >= 0.01f) 
			alScores.Add(string.Format("{0:F0}% contemptuous", es.contempt * 100f));
		if(es.disgust >= 0.01f) 
			alScores.Add(string.Format("{0:F0}% disgusted,", es.disgust * 100f));
		if(es.fear >= 0.01f) 
			alScores.Add(string.Format("{0:F0}% scared", es.fear * 100f));
		if(es.happiness >= 0.01f) 
			alScores.Add(string.Format("{0:F0}% happy", es.happiness * 100f));
		if(es.neutral >= 0.01f) 
			alScores.Add(string.Format("{0:F0}% neutral", es.neutral * 100f));
		if(es.sadness >= 0.01f) 
			alScores.Add(string.Format("{0:F0}% sad", es.sadness * 100f));
		if(es.surprise >= 0.01f) 
			alScores.Add(string.Format("{0:F0}% surprised", es.surprise * 100f));
		
		return alScores;
	}

    public static Color[] FaceColors { get { return faceColors; } }
    public static string[] FaceColorNames { get { return faceColorNames; } }
}
