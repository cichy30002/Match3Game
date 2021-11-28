using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class Save
{
    public static void SaveLevel(int level)
	{
		BinaryFormatter formatter = new BinaryFormatter();
		string path = Application.persistentDataPath + "/save.data";
		FileStream stream = new FileStream(path, FileMode.Create);

		SaveData data = new SaveData(level);
		
		formatter.Serialize(stream, data);
		stream.Close();
	}

	public static SaveData LoadLevel()
	{
		string path = Application.persistentDataPath + "/save.data";
		if(File.Exists(path))
		{
			BinaryFormatter formatter = new BinaryFormatter();
			FileStream stream = new FileStream(path, FileMode.Open);
			SaveData data = formatter.Deserialize(stream) as SaveData;
			stream.Close();
			return data;
		}
		else
		{
			Debug.LogError("File not found " + path);
			return null;
		}
	}
}
