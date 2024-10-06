


public static class Globals
{
    // Server variables;
    public static string address = "127.0.0.1";
    public static int port = 8000;

    // DatabaseManager variables
    // DataPath to SystemCatalog folder.
    public static string DataPath { get; set; } = "SystemCatalog/";
    public static string jsonFilePath = DataPath + "SystemCatalog.json";
    public static Metadata.DatabaseManager dm = new (jsonFilePath);
    
    // QueryProcessing variables.
    public static string set_database = "";



}
