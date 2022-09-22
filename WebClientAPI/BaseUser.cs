using Common.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebClientAPI
{
    public abstract class BaseUser:IDisposable
    {
        protected readonly string jsonPath;
        FileStream fileUserInfoStream;
        StreamWriter userInfoWriter;
        
        string _username;
        public string Username
        {
            get => _username;
            set
            {
                UpdateFileField("Username", value);
                _username = value;
            }
        }
  
        string _alias;
        public string Alias 
        {
            get => _alias;
            set
            {
                UpdateFileField("Alias", value);
                _alias = value;
            }
        }
        
        string _serverCert;
        public string ServerCert
        {
            get => _serverCert;
            set
            {
                UpdateFileField("ServerCert", value);
                _serverCert = value;
            }
        }
       
        string _password;
        public string Password
        {
            get => _password;
            set
            {
                UpdateFileField("Password", value);
                _password = value;
            }
        }
        public BaseUser(string username)
        {
            jsonPath = $"{APIContext.UsersPlace}\\{username}.json";
            
            try
            {
                fileUserInfoStream = File.Open(jsonPath, FileMode.Open);
                userInfoWriter = new(fileUserInfoStream);
            }
            catch(FileNotFoundException)
            {
                fileUserInfoStream = File.Open(jsonPath,FileMode.CreateNew);
                userInfoWriter = new(fileUserInfoStream);
                TokenGenerator generator = new();
                UpdateFileFields(new Dictionary<string,string>() 
                {
                    {"Username",username },
                    {"Alias",Alias??generator.GenerateToken() },
                    {"ServerCert",ServerCert??generator.GenerateToken() },
                    {"Password",Password??generator.GenerateToken() }
                });
                fileUserInfoStream = File.Open(jsonPath, FileMode.Open);
            }
            finally
            {
                SetFields();
                fileUserInfoStream = File.Open(jsonPath, FileMode.Open);
                userInfoWriter = new(fileUserInfoStream);
            }
        }
        dynamic ReadUserInfo(FileStream fileUserInfoStream)
        {
            using StreamReader reader = new(fileUserInfoStream);
            string info=reader.ReadToEnd();
            return JsonConvert.DeserializeObject(info);
        }
        void SetFields()
        {
            dynamic json = ReadUserInfo(fileUserInfoStream);
            _username = json["Username"];
            _alias = json["Alias"];
            _serverCert = json["ServerCert"];
            _password = json["Password"];
        }
        void UpdateFileField(string fieldName,string newValue)
        {

            JObject jobj = JObject.FromObject(this);

            jobj[fieldName] = newValue;
            
            string obj=JsonConvert.SerializeObject(jobj);
            userInfoWriter.Write(obj);
            userInfoWriter.Dispose();
            fileUserInfoStream = File.Open(jsonPath,FileMode.Open);
            userInfoWriter = new(fileUserInfoStream);
        }
        void UpdateFileFields(IEnumerable<KeyValuePair<string,string>> args)
        {
            //string str=JsonConvert.SerializeObject(this);
            JObject jobj = JObject.FromObject(this);// (JObject)JsonConvert.DeserializeObject(str);
            foreach (var arg in args)
                jobj[arg.Key] = arg.Value;
            string obj = JsonConvert.SerializeObject(jobj);
            userInfoWriter.Write(obj);
            userInfoWriter.Dispose();
        }
       
        public void Dispose()
        {
            userInfoWriter.Dispose();
            fileUserInfoStream.Dispose();
        }

    }
}
