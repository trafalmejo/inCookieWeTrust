using UnityEngine;
using System.Collections;
using System;
using System.IO;
 



namespace RedScarf.EasyCSV.Demo
{
    public class CsvTest : MonoBehaviour
    {
        public TextAsset text;
		public int numberOfMessages;

        CsvTable table;

        private void Start()
        {
            CsvHelper.Init();
            table = CsvHelper.Create(text.name, text.text);
			//numberOfMessages = table.RowCount;
			numberOfMessages = 10;
			//Debug.Log(table.Read(2,2));

        }
		public String getMessage(int column){
			return table.Read ((int)UnityEngine.Random.Range(2, numberOfMessages), column);
		}
		public int categoryToColumn(string category){
			//ACCESSORY:GLASSES	ACCESSORY:HEADWEAR	AGE:LESS10	AGE:11to30	AGE:31to50	AGE:51to70	AGE:more:70	GENDER:MALE	GENDER:FEMALE	MAKEUP:Y	MAKEUP:N	FACIALHAIR:MOUSTACHE:Y	FACIALHAIR:MOUSTACHE:N	FACIALHAIR:BEARD:Y	FACIALHAIR:BEARD:N	HAIR:BLACK	HAIR:BROWN	HAIR:BLOND	HAIR:GRAY	HAIR:RED	HAIR:OTHER	BOLD
			if (category == "ACCESSORY:GLASSES") {
				return 0;
			}			
			else if (category == "ACCESSORY:HEADWEAR") {
				return 1;
			}
			else if (category == "AGE:LESS10") {
				return 2;
			}
			else if (category == "AGE:10to12(TWEENS)") {
				return 3;
			}
			else if (category == "AGE:13to14(EARLY TEENS)") {
				return 4;
			}
			else if (category == "AGE:15to17(MID TEENS)") {
				return 5;
			}
			else if (category == "AGE:18to19(LATE TEENS)") {
				return 6;
			}
			else if (category == "AGE:20to23(EARLY 20S)") {
				return 7;
			}
			else if (category == "AGE:24to26(MID 20S)") {
				return 8;
			}
			else if (category == "AGE:27to29(LATE 20S)") {
				return 9;
			}
			else if (category == "AGE:30to33(EARLY 30S)") {
				return 10;
			}
			else if (category == "AGE:34to36(MID 30S)") {
				return 11;
			}
			else if (category == "AGE:37to39(LATE 30S)") {
				return 12;
			}
			//FACIALHAIR:BEARD:Y	FACIALHAIR:BEARD:N	HAIR:BLACK	HAIR:BROWN	HAIR:BLOND	HAIR:GRAY	HAIR:RED	HAIR:OTHER	BOLD
			else if (category == "AGE:40to43(EARLY 40S)") {
				return 13;
			}
			else if (category == "AGE:44to46(MID 40S)") {
				return 14;
			}
			else if (category == "AGE:47to49(LATE 40S)") {
				return 15;
			}
			else if (category == "AGE:50to53(EARLY 50S)") {
				return 16;
			}
			else if (category == "AGE:54to56(MID 50S)") {
				return 17;
			}
			else if (category == "AGE:56to59(LATE 50S)") {
				return 18;
			}
			else if (category == "AGE:ABOVE60") {
				return 19;
			}
			else if (category == "GENDER:MALE") {
				return 20;
			}
			else if (category == "GENDER:FEMALE") {
				return 21;
			}
			else if (category == "MAKEUP:Y") {
				return 22;
			}
			else if (category == "MAKEUP:N") {
				return 23;
			}
			else if (category == "FACIALHAIR:MOUSTACHE:Y") {
				return 24;
			}
			else if (category == "FACIALHAIR:MOUSTACHE:N") {
				return 25;
			}
			else if (category == "FACIALHAIR:BEARD:Y") {
				return 26;
			}
			else if (category == "FACIALHAIR:BEARD:N") {
				return 27;
			}
			else if (category == "HAIR:BLACK") {
				return 28;
			}
			else if (category == "HAIR:BROWN") {
				return 29;
			}
			else if (category == "HAIR:BLOND") {
				return 30;
			}
			else if (category == "HAIR:GRAY") {
				return 31;
			}
			else if (category == "HAIR:RED") {
				return 32;
			}
			else if (category == "HAIR:OTHER") {
				return 33;
			}
			else if (category == "BOLD") {
				return 34;
			}
			return 35;
		}
		public string getText(int row, int column){
			return table.Read (row, column);
		}


		int row=0;
        int column=0;
        string rowStr = "";
        string columnStr="";
        string readValue="";
        string writeValue="";
        string rowDataStr="";
        string rowID = "Jack";
        int buttonWidth = 150;



//        private void OnGUI()
//        {
//            GUILayout.Space(30);
//
//            //Display csv table
//            foreach (var row in table.RawDataList)
//            {
//                using (new GUILayout.HorizontalScope())
//                {
//                    foreach (var value in row)
//                    {
//                        GUILayout.Label(value,GUILayout.Width(150));
//                    }
//                }
//            }
//
//            GUILayout.Space(100);
//
//            //Modify UI
//            using (new GUILayout.HorizontalScope())
//            {
//                GUILayout.Label("Row:",GUILayout.Width(buttonWidth));
//                rowStr = GUILayout.TextField(rowStr);
//                int.TryParse(rowStr, out row);
//                rowStr = row.ToString();
//
//                GUILayout.Space(20);
//
//                GUILayout.Label("Column:", GUILayout.Width(buttonWidth));
//                columnStr = GUILayout.TextField(columnStr);
//                int.TryParse(columnStr, out column);
//                columnStr = column.ToString();
//            }
//            using (new GUILayout.HorizontalScope())
//            {
//                if (GUILayout.Button("Read", GUILayout.Width(buttonWidth)))
//                {
//                    readValue = table.Read(row,column);
//                }
//                GUILayout.TextArea(readValue);
//            }
//            using (new GUILayout.HorizontalScope())
//            {
//                if (GUILayout.Button("Write",GUILayout.Width(buttonWidth)))
//                {
//                    table.Write(row,column,writeValue);
//                }
//                writeValue = GUILayout.TextArea(writeValue);
//            }
//            using (new GUILayout.HorizontalScope())
//            {
//                using (new GUILayout.VerticalScope(GUILayout.Width(buttonWidth)))
//                {
//                    if (GUILayout.Button("PaddingData"))
//                    {
//                        var testRowData = CsvHelper.PaddingData<TestRowData>(text.name, rowID);
//                        rowDataStr = testRowData.ToString();
//                    }
//                    rowID = GUILayout.TextField(rowID);
//                }
//                GUILayout.TextArea(rowDataStr);
//            }
//        }
    }
}