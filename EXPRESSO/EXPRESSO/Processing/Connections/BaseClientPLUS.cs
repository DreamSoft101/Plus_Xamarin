using EXPRESSO.Utils;
using Newtonsoft.Json;
using RestSharp.Portable;
using RestSharp.Portable.HttpClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXPRESSO.Processing.Connections
{
    public class BaseClientPLUS
    {
        private string TAG = "BaseClient";
        private DateTime mdteStart;
        private RestClient mRstClient = new RestClient();
        private RestRequest mRstRequest = new RestRequest();
        private Dictionary<String, dynamic> mParameters = new Dictionary<string, dynamic>();
        private List<RestHeader> mLstHeader = new List<RestHeader>();
        private string mMethod;
        private string jsonrpc = "";
        private int id = 1;

        public string StrMethod
        {
            get { return mMethod; }
            set { this.mMethod = value; }
        }

        public async Task<string> getData()
        {
            JsonInput inputData = new JsonInput();
            inputData.method = StrMethod;
            //string input = JsonConvert.SerializeObject(mLstParameter);
            inputData.@params = mParameters;

            mRstClient = new RestClient(Cons.API_URL_PLUS);
            mRstRequest = new RestRequest("", Method.POST);
            //mRstClient.Timeout = new TimeSpan(;
            string strBody = JsonConvert.SerializeObject(inputData);
            mRstRequest.AddBody(inputData);

            AddOrUpdateHeader("Content-Type", "application/x-www-form-urlencoded");
            foreach (var header in mLstHeader)
            {
                mRstRequest.AddOrUpdateHeader(header.Name, header.Value);
            }
            //var result = mRstClient.Execute(mRstRequest) as RestResponse;
            try
            {
                mdteStart = DateTime.Now;
                LogUtils.WriteLog(TAG, "Input: " + strBody);
                var result = await mRstClient.Execute(mRstRequest);
                LogUtils.WriteLog(TAG, "Output: " + result.Content);
                LogUtils.WriteLog(TAG, "Time: " + (DateTime.Now - mdteStart).TotalMilliseconds + " ms");
                return result.Content;
            }
            catch (Exception ex)
            {
                LogUtils.WriteLog(TAG, "Input: " + strBody + "/Error: " + ex.Message + "/Time: " + (DateTime.Now - mdteStart).TotalMilliseconds + " ms");
                return null;
            }
        }

        public void AddParameter(RestParameter param)
        {
            mParameters.Add(param.Name, param.Value);
        }

        public void AddParameter(string key, dynamic value)
        {
            try
            {
                RestParameter param = new RestParameter() { Name = key, Value = value };
                this.AddParameter(param);
                //LogUtils.WriteLog("AddParameter", "Name: " + key + "/ Value: " + value);
            }
            catch (Exception ex)
            {
                LogUtils.WriteError("AddParameter", ex.Message + "(Name: " + key + "/ Value: " + value + ")");
            }

        }


        public void AddOrUpdateHeader(string strHeader, string strValue)
        {
            mLstHeader.Add(new RestHeader() { Name = strHeader, Value = strValue });
        }

        public class JsonInput
        {
            public string method;
            public string jsonrpc = "2.0";
            public object @params;
            public int id = 1;
        }

        public class RestHeader
        {
            private string mName;
            public string Name
            {
                get { return mName; }
                set { this.mName = value; }
            }

            private string mValue;
            public Object Value
            {
                get
                {
                    return mValue;
                }
                set
                {
                    mValue = value.ToString();
                }
            }
        }

        public class RestParameter
        {
            private string mName;
            public string Name
            {
                get { return mName; }
                set { this.mName = value; }
            }

            private dynamic mValue;
            public dynamic Value
            {
                get
                {
                    return mValue;
                }
                set
                {
                    mValue = value;
                }
            }
        }

    }
}
