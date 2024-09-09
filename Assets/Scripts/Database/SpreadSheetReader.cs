using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;

namespace DefaultNamespace.Database
{
    public class SpreadSheetReader
    {
        public readonly string ADRESS = "1BksCb5wDTNaXzDFMsazgt0wz4vhYwZtQU3zJdCOGDQk";
        public readonly string RANGE = "A2:P";
        public readonly long SHEET_ID = 0;

        public bool IsDataLoaded = false;
        public string rawData;

        public IEnumerator LoadData()
        {
            UnityWebRequest www = UnityWebRequest.Get(ToFullAddress(ADRESS, RANGE, SHEET_ID));
            yield return www.SendWebRequest();
            IsDataLoaded = true;
            rawData = www.downloadHandler.text;
        }

        public string ToFullAddress(string adress, string range, long sheetID) =>
            $"https://docs.google.com/spreadsheets/d/{adress}/export?format=tsv&range={range}&gid={sheetID}";
    }
}