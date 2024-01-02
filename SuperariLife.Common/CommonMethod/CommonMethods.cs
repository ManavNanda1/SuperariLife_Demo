using System.Web;
using System.Text.RegularExpressions;
using System.Data;
using SuperariLife.Model.ReqResponse;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using SuperariLife.Common.Helpers;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;


namespace SuperariLife.Common.CommonMethod
{
    public class CommonMethods
    {
     
        public static void DatatableToCSV(DataTable dtDataTable, string strFilePath)
        {
            StreamWriter sw = new StreamWriter(strFilePath, false);
            //headers    
            for (int i = 0; i < dtDataTable.Columns.Count; i++)
            {
                sw.Write(dtDataTable.Columns[i]);
                if (i < dtDataTable.Columns.Count - 1)
                {
                    sw.Write(",");
                }
            }
            sw.Write(sw.NewLine);
            foreach (DataRow dr in dtDataTable.Rows)
            {
                for (int i = 0; i < dtDataTable.Columns.Count; i++)
                {
                    if (!Convert.IsDBNull(dr[i]))
                    {
                        string value = dr[i].ToString();
                        if (value.Contains(','))
                        {
                            value = String.Format("\"{0}\"", value);
                            sw.Write(value);
                        }
                        else
                        {
                            sw.Write(dr[i].ToString());
                        }
                    }
                    if (i < dtDataTable.Columns.Count - 1)
                    {
                        sw.Write(",");
                    }
                }
                sw.Write(sw.NewLine);
            }
            sw.Close();
        }
        public static void DeleteDirectory(string DirectoryPath)
        {
            string BasePath = Path.Combine(Directory.GetCurrentDirectory(), DirectoryPath);

            if (Directory.Exists(BasePath))
            {

                string[] allEntries = Directory.GetFileSystemEntries(BasePath);

                foreach (string entry in allEntries)
                {

                    if (File.Exists(entry))
                    {
                        File.Delete(entry);
                    }

                    else if (Directory.Exists(entry))
                    {
                        DeleteDirectory(entry);
                    }
                }


                Directory.Delete(BasePath);
            }
            else
            {
                return;
            }

        }
        public static void DeleteFileByName(string folderPath, string fileName)
        {
            if(fileName!=null)
            {
                string filePath = Path.Combine(folderPath, fileName);
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
                else
                {
                    return;
                }
            }
            else
            {
                return;
            }
           
        }

        #region GetKeyValues
        /// <summary>
        /// Get key value pair result
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static ParamValue GetKeyValues(HttpContext context)
        {
            ParamValue paramValues = new ParamValue();
            var headerValue = string.Empty;
            var queryString = string.Empty;
            var jsonString = string.Empty;
            StringValues outValue = string.Empty;

            // for from header value
            if (context.Request.Headers.TryGetValue(Constants.RequestModel, out outValue))
            {
                headerValue = outValue.FirstOrDefault();
                JObject jsonobj = JsonConvert.DeserializeObject<JObject>(headerValue);
                if (jsonobj != null)
                {
                    Dictionary<string, string> keyValueMap = new Dictionary<string, string>();
                    foreach (KeyValuePair<string, JToken> keyValuePair in jsonobj)
                    {
                        keyValueMap.Add(keyValuePair.Key, keyValuePair.Value.ToString());
                    }
                    List<ReqResponseKeyValue> keyValueMapNew = keyValueMap.ToList().Select(i => new ReqResponseKeyValue
                    {
                        Key = i.Key,
                        Value = i.Value
                    }).ToList();
                    jsonString = JsonConvert.SerializeObject(keyValueMapNew);
                }
            }
            // for from query value
            if (context.Request.QueryString.HasValue)
            {
                var dict = HttpUtility.ParseQueryString(context.Request.QueryString.Value);
                queryString = System.Text.Json.JsonSerializer.Serialize(
                                    dict.AllKeys.ToDictionary(k => k, k => dict[k]));
            }


            paramValues.HeaderValue = jsonString;
            paramValues.QueryStringValue = queryString;
            return paramValues;

        }
        #endregion
        public static string[] GenerateEncryptedPasswordAndPasswordSalt(string password)
        {
            string hashed = EncryptionDecryption.Hash(EncryptionDecryption.GetEncrypt(password));
            string[] segments = hashed.Split(":");
            string EncryptedHash = EncryptionDecryption.GetEncrypt(segments[0]);
            string EncryptedSalt = EncryptionDecryption.GetEncrypt(segments[1]);
            string Hash = EncryptionDecryption.GetDecrypt(EncryptedHash);
            string Salt = EncryptionDecryption.GetDecrypt(EncryptedSalt);
            string[] EncryptedPassword = new string[4];
            EncryptedPassword[0] = EncryptedHash;
            EncryptedPassword[1] = EncryptedSalt;
            EncryptedPassword[2] = Hash;
            EncryptedPassword[3] = Salt;
            return EncryptedPassword;
        }
        public static string GenerateNewRandom()
        {
            Random generator = new Random();
            String r = generator.Next(0, 1000000).ToString("D6");
            if (r.Distinct().Count() == 1)
            {
                r = GenerateNewRandom();
            }
            return r;
        }
        public static bool IsValidEmail(string email)
        {
            const string pattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";
            return Regex.IsMatch(email, pattern);
        }
        public static bool IsPasswordStrong(string CreatePassword)
        {
            const string pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!#%*?&])[A-Za-z\d@$!#%*?&]{8,}$";

            return Regex.IsMatch(CreatePassword, pattern);
        }
        public static async Task<string> UploadImage(IFormFile userProfile, string UserProfilePath, string Email)
        {
            Guid guidFile = Guid.NewGuid();
            string FileName;
            string BasePath;
            string path;
            string Photo = string.Empty;

            FileName = guidFile + Path.GetExtension(userProfile.FileName);
            BasePath = Path.Combine(Directory.GetCurrentDirectory(), UserProfilePath);

            if (!Directory.Exists(BasePath))
            {
                Directory.CreateDirectory(BasePath);
            }
            path = Path.Combine(BasePath, FileName);
            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                await userProfile.CopyToAsync(stream);
            }
            Photo = FileName;
            return Photo;
        }
      
    }
}