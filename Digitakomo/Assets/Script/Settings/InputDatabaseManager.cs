using System.IO;

public class InputDatabaseManager
{
    string path;
    readonly string filename = "/Resources/UserSetting.dgkm";

    public InputDatabaseManager(string ApplicationPath)
    {
        path = ApplicationPath;
    }

    public void WriteLine(string text)
    {
        File.WriteAllText(path + filename, text);
    }

    public string Read()
    {
        string result = "";
        if (File.Exists(path + filename))
        {
            // Read a text file line by line.
            string[] lines = File.ReadAllLines(path + filename);
            foreach (string line in lines)
            {
                result += line;
            }
        }
        return result;
    }
}
