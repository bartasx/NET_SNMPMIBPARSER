namespace NetSnmpMib;

public class MibManager
{
    private MibParser parser = new();
    private readonly string _mibFolderPath;
    
    public MibManager(string mibFolderPath)
    {
        _mibFolderPath = mibFolderPath;
    }
    
    public void Initialize(bool verbose = false)
    {
        foreach (string filePath in Directory.GetFiles(_mibFolderPath))
        {
            try
            {
                parser.ParseMibFile(filePath);
                if (verbose)
                {
                    Console.WriteLine($"Loaded {Path.GetFileNameWithoutExtension(filePath)}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error has occured when loading the following MIB File : {filePath}: {ex.Message}");
            }
        }

        Console.WriteLine("MibManager initialized");
    }

    public List<string> GetAllOids()
    {
        var allOids = new HashSet<string>();

        foreach (var table in parser.Tables.Values)
        {
            foreach (var group in table.Groups.Values)
            {
                if (group.OID != null)
                {
                    allOids.Add(group.OID);
                }
            }

            foreach (var item in table.Items.Values)
            {
                if (item.OID != null)
                {
                    allOids.Add(item.OID);
                }
            }
        }

        return allOids.ToList();
    }

    /// <summary>
    /// Finding name for specific OID 
    /// </summary>
    /// <param name="oid"></param>
    /// <returns>If not found, it returns the null string</returns>
    public string FindNameForOid(string oid)
    {
        foreach (var table in parser.Tables.Values)
        {
            var name = table.OidToName(oid);
            return name;
        }

        return null;
    }

    /// <summary>
    /// Finding the description for specific OID
    /// </summary>
    /// <param name="oid"></param>
    /// <returns>If not found, it returns the null string</returns>
    public string FindDescriptionForOid(string oid)
    {
        foreach (var table in parser.Tables.Values)
        {
            string name = table.OidToName(oid);
            return name;
        }

        return string.Empty;
    }
}