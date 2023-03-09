using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class ValuesController : ApiController
    {
        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }

        public class DingDingBackEntity
        {
            public string msg_signature { get; set; }
            public string encrypt { get; set; }
            public string timeStamp { get; set; }
            public string nonce { get; set; }
        }
        /// <summary>
        /// 钉钉回调事件方法
        /// </summary>
        /// <param name="signature">消息体签名</param>
        /// <param name="timestamp">事件戳</param>
        /// <param name="nonce">随机字符串</param>
        /// <returns></returns>
        [HttpPost]
        public DingDingBackEntity DingDingCallBack(string signature, string timestamp, string nonce)
        {

            //这两句代码是为了接收body体中传入的加密json串
            Request.Content.ReadAsStreamAsync().Result.Seek(0, System.IO.SeekOrigin.Begin);
            string content = Request.Content.ReadAsStringAsync().Result;
            //反序列化json串拿去加密字符串
            JToken json = JToken.Parse(content);
            string ever = json["encrypt"].ToString();
            //实例化钉钉解密类构造参数为对应的 应用中的token、appkeys、AppKey值
            DingTalkEncryptor dingTalkEncryptor = new DingTalkEncryptor("xUk2p1zYZGGqUhQ5VHU27PcaoKL7TfnBcxM", "Fz7so8SmhfHlfpOeVCxpMDPvY83FSizwtolwIXu8yUV", "dingfdlyx2gwllucyoxn");
            //定义字符串接收解密后的值
            string returnDate = dingTalkEncryptor.getDecryptMsg(signature, timestamp, nonce, ever);
            JToken jToken = JToken.Parse(returnDate);
            //取出事件类型字段
            string EventType = jToken["EventType"].ToString();
            //这里解密以后取出的这个EventType这个参数对应的就是你所要匹配的事件了具体的事件参数类型可查看钉钉的接口文档
            //https://open.dingtalk.com/document/org/event-list-1








            //将json串中的指定值取出           
            var msg = dingTalkEncryptor.getEncryptedMap("success");
            DingDingBackEntity back = new DingDingBackEntity();
            back.msg_signature = msg["msg_signature"];
            back.encrypt = msg["encrypt"];
            back.timeStamp = msg["timeStamp"];
            back.nonce = msg["nonce"];
            // string backmsg = JsonConvert.SerializeObject(msg);
            return back;


        } 
    }
}
