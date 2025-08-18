using System;
using MySql.Data.MySqlClient;
using UnityEngine;
using System.Text;

public class SqlAccess
{
     public static MySqlConnection dbConnection;
     private string host;
     private string port;
     private string username; 
     private string pwd;
     private string database;
    #region singleton
    private static SqlAccess instance;
    public static SqlAccess getInstance()
    {
        if (instance == null)
        {
            instance = new SqlAccess();
        }
        return instance; ;
    }
    #endregion

    public SqlAccess()
    {
        OpenSql();
    }

    /// <summary>
    /// Connect database
    /// </summary>
    public  void OpenSql()
    {
        try
        {
            host = ReadConfig.instance.db_host;
            port = ReadConfig.instance.db_port;
            database = ReadConfig.instance.db_databaseName;
            username = ReadConfig.instance.db_username;
            pwd = ReadConfig.instance.db_password;
            string connectionString = string.Format("server = {0};port={1};database = {2};user = {3};password = {4};", host, port, database, username, pwd);
            //Debug.Log(connectionString);
            dbConnection = new MySqlConnection(connectionString);
            dbConnection.Open();
            Debug.Log("OpenSql()--unity connect mysql success");
        }
        catch (Exception e)
        {
            Debug.Log("OpenSql()--unity connect mysql fail, message:" + e.Message);
        }
    }

    /// <summary>
    /// Close database connection
    /// </summary>
    public void Close()
    {
        if (dbConnection != null)
        {
            dbConnection.Close();
            dbConnection.Dispose();
            dbConnection = null;
            Debug.Log("Close()--unity close mysql success");
        }
    }

    #region table_01_001
    public int queryPatientSite(string patientId)
    {
        //GameSettings settings = null;
        int res =0;//给一个默认值，左手
        string r1 = "";
        string sql = "select C010 FROM t_01_001 where C001=@patientId";
        try
        {
            using (MySqlCommand cmd = new MySqlCommand(sql, dbConnection))
            {
                cmd.Parameters.AddWithValue("@patientId", patientId);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        r1 = reader.GetString(0);
                    }
                }
            }
        }
        catch (Exception ee)
        {
            Debug.Log("queryPatientSite()--query table t_01_001 Exception:" + ee.Message);
            Debug.Log("queryPatientSite()--query table t_01_001 Exception: sql：" + sql);
            Debug.Log("Exception:Parameters: patientId=" + patientId);
        }
        //处理下数据
        if ("右手".Equals(r1))
        {
            res =1;
        }
        return res;
    }
    #endregion

    #region table t_02_002
    public void addTable_t_02_002(T_table t_02_002)
    {
        string sql =  "INSERT INTO t_02_002 VALUES (@C001, @C002, @C003, @C004, @C005, @C006, @C007, @C008, @C009, @C010, @C011, @C012, @C013, @C014, @C015, @C016, @C017, @C018, @C019, @C020)";
        try
        {
            using (MySqlCommand cmd = new MySqlCommand(sql, dbConnection))
            {
                cmd.Parameters.AddWithValue("@C001", t_02_002.C001 ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@C002", t_02_002.C002 ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@C003", t_02_002.C003 ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@C004", t_02_002.C004 ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@C005", t_02_002.C005 ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@C006", t_02_002.C006 ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@C007", t_02_002.C007 ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@C008", t_02_002.C008 ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@C009", t_02_002.C009 ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@C010", t_02_002.C010 ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@C011", t_02_002.C011 ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@C012", t_02_002.C012 ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@C013", t_02_002.C013 ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@C014", t_02_002.C014 ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@C015", t_02_002.C015 ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@C016", t_02_002.C016 ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@C017", t_02_002.C017 ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@C018", t_02_002.C018 ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@C019", t_02_002.C019 ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@C020", t_02_002.C020 ?? (object)DBNull.Value);
                int result = cmd.ExecuteNonQuery();
                if (result <= 0)
                {
                    Debug.Log("addTable_t_02_002()--insert table t_02_002 failure:insertNum:" + result + ",sql：" + sql);
                    PrintEntry(t_02_002);
                }
            }
        }
        catch (Exception ee)
        {
            Debug.Log("addTable_t_02_002()--insert table t_02_002 Exception:" + ee.Message);
            Debug.Log("addTable_t_02_002()--insert table t_02_002 Exception: sql：" + sql);
            PrintEntry(t_02_002);
        }
    }

    public int queryGameTimes(string patientId, string gameId)
    {
        int result = 0;
        string sql = "select count(C001) from t_02_002 where C001=@patientId and C002=@gameId";
        try
        {
            using (MySqlCommand cmd = new MySqlCommand(sql, dbConnection))
            {
                cmd.Parameters.AddWithValue("@patientId", patientId);
                cmd.Parameters.AddWithValue("@gameId", gameId);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result = reader.GetInt32(0);
                    }
                }
            }
        }
        catch (Exception ee)
        {
            Debug.Log("SearchTable_t_02_002()--query table t_02_002 Exception:" + ee.Message);
            Debug.Log("addTable_t_02_002()--insert table t_02_002 Exception: sql：" + sql);
            Debug.Log("Exception:Parameters: patientId=" + patientId + ", gameId=" + gameId);
        }
        return result;
    }

    public void UpdateTable_t_02_002(T_table t_02_002)
    {
        string sql = "UPDATE t_02_002 SET C004=@C004, C005=@C005, C007=@C007, C008=@C008, C009=@C009, " +
                     "C010=@C010, C011=@C011, C012=@C012, C013=@C013, C014=@C014, C015=@C015, C016=@C016, " +
                     "C017=@C017, C018=@C018, C019=@C019, C020=@C020 " +
                     "WHERE C001=@C001 AND C002=@C002 AND C003=@C003";
        try
        {
            using (MySqlCommand cmd = new MySqlCommand(sql, dbConnection))
            {
                cmd.Parameters.AddWithValue("@C001", t_02_002.C001);
                cmd.Parameters.AddWithValue("@C002", t_02_002.C002);
                cmd.Parameters.AddWithValue("@C003", t_02_002.C003);
                cmd.Parameters.AddWithValue("@C004", t_02_002.C004 ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@C005", t_02_002.C005 ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@C007", t_02_002.C007 ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@C008", t_02_002.C008 ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@C009", t_02_002.C009 ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@C010", t_02_002.C010 ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@C011", t_02_002.C011 ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@C012", t_02_002.C012 ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@C013", t_02_002.C013 ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@C014", t_02_002.C014 ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@C015", t_02_002.C015 ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@C016", t_02_002.C016 ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@C017", t_02_002.C017 ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@C018", t_02_002.C018 ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@C019", t_02_002.C019 ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@C020", t_02_002.C020 ?? (object)DBNull.Value);
                int result = cmd.ExecuteNonQuery();
                if (result <= 0)
                {
                    Debug.Log("addTable_t_02_002()--update table t_02_002 failure:insertNum:" + result + ",sql：" + sql);
                    PrintEntry(t_02_002);
                }
            }
        }
        catch (Exception ee)
        {
            Debug.Log("UpdateScore()--update table t_02_002 Exception:" + ee.Message);
            Debug.Log("addTable_t_02_002()--update table t_02_002 Exception:insertNum:sql：" + sql);
            PrintEntry(t_02_002);
        }
    }
    #endregion

    #region table t_02_003
    public void AddData(T_table t_02_003)
    {
        string sql = "INSERT INTO t_02_003 VALUES (@C001, @C002, @C003, @C004, @C005, @C006, @C007, @C008, @C009, @C010, @C011, @C012, @C013, @C014, @C015, @C016, @C017, @C018, @C019, @C020)";
        try
        {
            using (MySqlCommand cmd = new MySqlCommand(sql, dbConnection))
            {
                cmd.Parameters.AddWithValue("@C001", t_02_003.C001 ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@C002", t_02_003.C002 ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@C003", t_02_003.C003 ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@C004", t_02_003.C004 ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@C005", t_02_003.C005 ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@C006", t_02_003.C006 ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@C007", t_02_003.C007 ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@C008", t_02_003.C008 ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@C009", t_02_003.C009 ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@C010", t_02_003.C010 ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@C011", t_02_003.C011 ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@C012", t_02_003.C012 ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@C013", t_02_003.C013 ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@C014", t_02_003.C014 ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@C015", t_02_003.C015 ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@C016", t_02_003.C016 ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@C017", t_02_003.C017 ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@C018", t_02_003.C018 ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@C019", t_02_003.C019 ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@C020", t_02_003.C020 ?? (object)DBNull.Value);
                int result = cmd.ExecuteNonQuery();
                if (result <= 0)
                {
                    Debug.Log("AddData()--insert table t_02_003 failure:insertNum:" + result + ",sql：" + sql);
                    PrintEntry(t_02_003);
                }
            }
        }
        catch (Exception ee)
        {
            Debug.Log("AddData()--insert table t_02_003 Exception:" + ee.Message);
            Debug.Log("AddData()--insert table t_02_003 Exception:sql：" + sql);
            PrintEntry(t_02_003);
        }
    }
    #endregion

    #region table_02_005
    public GameSettings queryGameSettings(string patientId)
    {
        GameSettings settings = null;
        string sql= "select C008,C009,C010,C011,C012,C013 FROM t_02_005 where C001=@patientId"; 
        try
        {
            using (MySqlCommand cmd = new MySqlCommand(sql, dbConnection))
            {
                cmd.Parameters.AddWithValue("@patientId", patientId);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        settings = new GameSettings
                        {
                            C008 = reader.GetString(0),
                            C009 = reader.GetString(1),
                            C010 = reader.GetString(2),
                            C011 = reader.GetString(3),
                            C012 = reader.GetString(4),
                            C013 = reader.GetString(5)
                        };
                    }
                }
            }
        }
        catch (Exception ee)
        {
            Debug.Log("SearchTable_t_02_005()--query table t_02_005 Exception:" + ee.Message);
            Debug.Log("addTable_t_02_005()--insert table t_02_005 Exception: sql：" + sql);
            Debug.Log("Exception:Parameters: patientId=" + patientId);
        }
        return settings;
    }
    #endregion

    #region table t_03_002
    public int queryEvaTimes(string patientId, string evaId)
    {
        int result = 0;
        string sql = "select count(C001) from t_03_002 where C001=@patientId and C002=@evaId";
        try
        {
            using (MySqlCommand cmd = new MySqlCommand(sql, dbConnection))
            {
                cmd.Parameters.AddWithValue("@patientId", patientId);
                cmd.Parameters.AddWithValue("@evaId", evaId);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result = reader.GetInt32(0);
                    }
                }
            }
        }
        catch (Exception ee)
        {
            Debug.Log("SearchTable_t_02_002()--query table t_02_002 Exception:" + ee.Message);
            Debug.Log("addTable_t_02_002()--insert table t_02_002 Exception: sql：" + sql);
            Debug.Log("Exception:Parameters: patientId=" + patientId + ", evaId=" + evaId);
        }
        return result;
    }
    public void addTable_t_03_002(T1_table t_03_002)
    {
        string sql = "INSERT INTO t_03_002 VALUES (@C001, @C002, @C003, @C004, @C005, @C006, @C007, @C008, @C009, @C010, @C011, @C012, @C013, @C014, @C015, @C016, @C017, @C018, @C019, @C020, @C021, @C022, @C023, @C024)";
        try
        {
            using (MySqlCommand cmd = new MySqlCommand(sql, dbConnection))
            {
                cmd.Parameters.AddWithValue("@C001", t_03_002.C001 ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@C002", t_03_002.C002 ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@C003", t_03_002.C003 ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@C004", t_03_002.C004 ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@C005", t_03_002.C005 ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@C006", t_03_002.C006 ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@C007", t_03_002.C007 ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@C008", t_03_002.C008 ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@C009", t_03_002.C009 ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@C010", t_03_002.C010 ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@C011", t_03_002.C011 ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@C012", t_03_002.C012 ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@C013", t_03_002.C013 ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@C014", t_03_002.C014 ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@C015", t_03_002.C015 ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@C016", t_03_002.C016 ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@C017", t_03_002.C017 ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@C018", t_03_002.C018 ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@C019", t_03_002.C019 ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@C020", t_03_002.C020 ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@C021", t_03_002.C021 ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@C022", t_03_002.C022 ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@C023", t_03_002.C023 ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@C024", t_03_002.C024 ?? (object)DBNull.Value);
                int result = cmd.ExecuteNonQuery();
                if (result <= 0)
                {
                    Debug.Log("addTable_t_03_002()--insert table t_03_002 failure:insertNum:" + result + ",sql：" + sql);
                    PrintEntry1(t_03_002);
                }
            }
        }
        catch (Exception ee)
        {
            Debug.Log("addTable_t_03_002()--insert table t_03_002 Exception:" + ee.Message);
            Debug.Log("addTable_t_03_002()--insert table t_03_002 Exception: sql：" + sql);
            PrintEntry1(t_03_002);
        }
    }

    public void UpdateTable_t_03_002(T_table t_03_002)
    {
        string sql = "UPDATE t_02_002 SET C004=@C004, C005=@C005, C007=@C007, C008=@C008, C009=@C009, " +
                     "C010=@C010, C011=@C011, C012=@C012, C013=@C013, C014=@C014, C015=@C015, C016=@C016, " +
                     "C017=@C017, C018=@C018, C019=@C019, C020=@C020 " +
                     "WHERE C001=@C001 AND C002=@C002 AND C003=@C003";
        try
        {
            using (MySqlCommand cmd = new MySqlCommand(sql, dbConnection))
            {
                cmd.Parameters.AddWithValue("@C001", t_03_002.C001);
                cmd.Parameters.AddWithValue("@C002", t_03_002.C002);
                cmd.Parameters.AddWithValue("@C003", t_03_002.C003);
                cmd.Parameters.AddWithValue("@C004", t_03_002.C004 ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@C005", t_03_002.C005 ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@C007", t_03_002.C007 ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@C008", t_03_002.C008 ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@C009", t_03_002.C009 ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@C010", t_03_002.C010 ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@C011", t_03_002.C011 ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@C012", t_03_002.C012 ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@C013", t_03_002.C013 ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@C014", t_03_002.C014 ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@C015", t_03_002.C015 ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@C016", t_03_002.C016 ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@C017", t_03_002.C017 ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@C018", t_03_002.C018 ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@C019", t_03_002.C019 ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@C020", t_03_002.C020 ?? (object)DBNull.Value);
                int result = cmd.ExecuteNonQuery();
                if (result <= 0)
                {
                    Debug.Log("addTable_t_03_002()--update table t_03_002 failure:insertNum:" + result + ",sql：" + sql);
                    PrintEntry(t_03_002);
                }
            }
        }
        catch (Exception ee)
        {
            Debug.Log("UpdateScore()--update table t_03_002 Exception:" + ee.Message);
            Debug.Log("addTable_t_03_002()--update table t_03_002 Exception:insertNum:sql：" + sql);
            PrintEntry(t_03_002);
        }
    }
    #endregion

    #region table t_03_003
    public void AddDataT_03_003(T_table t_03_003)
    {
        string sql = "INSERT INTO t_03_003 VALUES (@C001, @C002, @C003, @C004, @C005, @C006, @C007, @C008, @C009, @C010, @C011, @C012, @C013, @C014, @C015, @C016, @C017, @C018, @C019, @C020)";
        try
        {
            using (MySqlCommand cmd = new MySqlCommand(sql, dbConnection))
            {
                cmd.Parameters.AddWithValue("@C001", t_03_003.C001 ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@C002", t_03_003.C002 ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@C003", t_03_003.C003 ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@C004", t_03_003.C004 ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@C005", t_03_003.C005 ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@C006", t_03_003.C006 ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@C007", t_03_003.C007 ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@C008", t_03_003.C008 ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@C009", t_03_003.C009 ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@C010", t_03_003.C010 ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@C011", t_03_003.C011 ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@C012", t_03_003.C012 ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@C013", t_03_003.C013 ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@C014", t_03_003.C014 ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@C015", t_03_003.C015 ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@C016", t_03_003.C016 ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@C017", t_03_003.C017 ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@C018", t_03_003.C018 ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@C019", t_03_003.C019 ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@C020", t_03_003.C020 ?? (object)DBNull.Value);
                int result = cmd.ExecuteNonQuery();
                if (result <= 0)
                {
                    Debug.Log("AddData()--insert table t_03_003 failure:insertNum:" + result + ",sql：" + sql);
                    PrintEntry(t_03_003);
                }
            }
        }
        catch (Exception ee)
        {
            Debug.Log("AddData()--insert table t_03_003 Exception:" + ee.Message);
            Debug.Log("AddData()--insert table t_03_003 Exception:sql：" + sql);
            PrintEntry(t_03_003);
        }
    }
    #endregion
    public void PrintEntry(T_table t)
    {
        StringBuilder sb = new StringBuilder(); 
        sb.Append("Parameters: ");
        sb.Append("C001=" + t.C001 + ", ");
        sb.Append("C002=" + t.C002 + ", ");
        sb.Append("C003=" + t.C003 + ", ");
        sb.Append("C004=" + t.C004 + ", ");
        sb.Append("C005=" + t.C005 + ", ");
        sb.Append("C006=" + t.C006 + ", ");
        sb.Append("C007=" + t.C007 + ", ");
        sb.Append("C008=" + t.C008 + ", ");
        sb.Append("C009=" + t.C009 + ", ");
        sb.Append("C010=" + t.C010 + ", ");
        sb.Append("C011=" + t.C011 + ", ");
        sb.Append("C012=" + t.C012 + ", ");
        sb.Append("C013=" + t.C013 + ", ");
        sb.Append("C014=" + t.C014 + ", ");
        sb.Append("C015=" + t.C015 + ", ");
        sb.Append("C016=" + t.C016 + ", ");
        sb.Append("C017=" + t.C017 + ", ");
        sb.Append("C018=" + t.C018 + ", ");
        sb.Append("C019=" + t.C019 + ", ");
        sb.Append("C020=" + t.C020);
        Debug.Log(sb.ToString());
    }
    public void PrintEntry1(T1_table t)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("Parameters: ");
        sb.Append("C001=" + t.C001 + ", ");
        sb.Append("C002=" + t.C002 + ", ");
        sb.Append("C003=" + t.C003 + ", ");
        sb.Append("C004=" + t.C004 + ", ");
        sb.Append("C005=" + t.C005 + ", ");
        sb.Append("C006=" + t.C006 + ", ");
        sb.Append("C007=" + t.C007 + ", ");
        sb.Append("C008=" + t.C008 + ", ");
        sb.Append("C009=" + t.C009 + ", ");
        sb.Append("C010=" + t.C010 + ", ");
        sb.Append("C011=" + t.C011 + ", ");
        sb.Append("C012=" + t.C012 + ", ");
        sb.Append("C013=" + t.C013 + ", ");
        sb.Append("C014=" + t.C014 + ", ");
        sb.Append("C015=" + t.C015 + ", ");
        sb.Append("C016=" + t.C016 + ", ");
        sb.Append("C017=" + t.C017 + ", ");
        sb.Append("C018=" + t.C018 + ", ");
        sb.Append("C019=" + t.C019 + ", ");
        sb.Append("C020=" + t.C020 + ", ");
        sb.Append("C021=" + t.C021 + ", ");
        sb.Append("C022=" + t.C022 + ", ");
        sb.Append("C023=" + t.C023 + ", ");
        sb.Append("C024=" + t.C024);
        Debug.Log(sb.ToString());
    }
}
