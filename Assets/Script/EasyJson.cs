using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

namespace EasyJson
{
    public static class EasyToJson
    {
        /**
         * <summary>
         * Json ���Ϸ� ����
         * </summary>
         * <param name="obj">Json���� ������ ��ü</param>
         * <param name="jsonFileName">Json ���� �̸�</param>
         * <param name="prettyPrint">Json�� ���� ���� ����� �� ����</param>
         */
        public static void ToJson<T>(T obj, string jsonFileName, bool prettyPrint = false)
        {
            string path = Path.Combine(Application.dataPath, jsonFileName + ".json");
            string json = JsonUtility.ToJson(obj, prettyPrint);
            File.WriteAllText(path, json);
            Debug.Log(json);
        }

        /**
         * <summary>
         * Json ������ �о ��ü�� ��ȯ
         * </summary>
         * <param name="jsonFileName">Json ���� �̸�</param>
         * <returns>Json ������ �о ���� ��ü</returns>
         */
        public static T FromJson<T>(string jsonFileName)
        {
            string path = Path.Combine(Application.dataPath, jsonFileName + ".json");
            string json = File.ReadAllText(path);
            T obj = JsonUtility.FromJson<T>(json);
            return obj;
        }

        /**
         * <summary>
         * List�� Json ���Ϸ� ����
         * </summary>
         * <param name="list">Json���� ������ ����Ʈ</param>
         * <param name="jsonFileName">Json ���� �̸�</param>
         * <param name="prettyPrint">Json�� ���� ���� ����� �� ����</param>
         */
        public static void ListToJson<T>(List<T> list, string jsonFileName, bool prettyPrint = false)
        {
            string path = Path.Combine(Application.dataPath, jsonFileName + ".json");
            string json = JsonConvert.SerializeObject(list, prettyPrint ? Formatting.Indented : Formatting.None);
            File.WriteAllText(path, json);
            Debug.Log(json);
        }

        /**
         * <summary>
         * Json ������ �о List�� ��ȯ
         * </summary>
         * <param name="jsonFileName">Json ���� �̸�</param>
         * <returns>Json ������ �о ���� List</returns>
         */
        public static List<T> ListFromJson<T>(string jsonFileName)
        {
            string path = Path.Combine(Application.dataPath, jsonFileName + ".json");
            string json = File.ReadAllText(path);
            List<T> obj = JsonConvert.DeserializeObject<List<T>>(json);
            return obj;
        }

        /**
         * <summary>
         * Dictionary�� Json ���Ϸ� ����
         * </summary>
         * <param name="dictionary">Json���� ������ Dictionary</param>
         * <param name="jsonFileName">Json ���� �̸�</param>
         * <param name="prettyPrint">Json�� ���� ���� ����� �� ����</param>
         */
        public static void DictionaryToJson<T, U>(Dictionary<T, U> dictionary, string jsonFileName, bool prettyPrint = false)
        {
            string path = Path.Combine(Application.dataPath, jsonFileName + ".json");
            string json = JsonConvert.SerializeObject(dictionary, prettyPrint ? Formatting.Indented : Formatting.None);
            File.WriteAllText(path, json);
            Debug.Log(json);
        }

        /**
         * <summary>
         * Json ������ �о Dictionary�� ��ȯ
         * </summary>
         * <param name="jsonFileName">Json ���� �̸�</param>
         * <returns>Json ������ �о ���� Dictionary</returns>
         */
        public static Dictionary<T, U> DictionaryFromJson<T, U>(string jsonFileName)
        {
            string path = Path.Combine(Application.dataPath, jsonFileName + ".json");
            string json = File.ReadAllText(path);
            Dictionary<T, U> obj = JsonConvert.DeserializeObject<Dictionary<T, U>>(json);
            Debug.Log(json);
            return obj;
        }

        /**
         * <summary>
         * Queue�� Json ���Ϸ� ����
         * </summary>
         * <param name="queue">Json���� ������ Stack</param>
         * <param name="jsonFileName">Json ���� �̸�</param>
         * <param name="prettyPrint">Json�� ���� ���� ����� �� ����</param>
         */
        public static void QueueToJson<T>(Queue<T> queue, string jsonFileName, bool prettyPrint = false)
        {
            string path = Path.Combine(Application.dataPath, jsonFileName + ".json");
            string json = JsonConvert.SerializeObject(queue, prettyPrint ? Formatting.Indented : Formatting.None);
            File.WriteAllText(path, json);
            Debug.Log(json);
        }

        /**
         * <summary>
         * Json ������ �о Queue�� ��ȯ
         * </summary>
         * <param name="jsonFileName">Json ���� �̸�</param>
         * <returns>Json ������ �о ���� Stack</returns>
         */
        public static Queue<T> QueueFromJson<T>(string jsonFileName)
        {
            string path = Path.Combine(Application.dataPath, jsonFileName + ".json");
            string json = File.ReadAllText(path);
            Queue<T> obj = JsonConvert.DeserializeObject<Queue<T>>(json);
            Debug.Log(json);
            return obj;
        }
    }
}
