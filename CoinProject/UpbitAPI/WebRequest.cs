using WebSocketSharp;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace UpbitAPI
{ 
    public class WebRequest
    {
        static string Request(string url)
        {
            JObject job = new JObject();
            JArray jar = new JArray();
            throw new NotImplementedException();
            // WinHttpRequest http = new WinHttpRequest();
            // url = $"https://crix-api-endpoint.upbit.com/v1/crix/candles/minutes/1?code=CRIX.UPBIT.KRW-BTC&count=1";
            // http.Open("GET", url);
            // http.Send();
            // 
            // return http.ResponseText;


            // upbit open api
            // url = "https://api.upbit.com/v1/candles/minutes/1?market=KRW-BTC&count=1";
            // WebClient client = new WebClient();
        }

       public string WebSocketRequest(EventHandler<MessageEventArgs> eventHandler)
        {
            JArray array = new JArray();
            array.Add("KRW-BTC");
            //array.Add("KRW-ETH");

            JObject obj1 = new JObject();
            obj1["ticket"] = Guid.NewGuid();//UUID

            JObject obj2 = new JObject();
            obj2["type"] = "trade";  //TypeEnums.UpbitDataType.trade.ToString();
            obj2["codes"] = array;

            JObject obj3 = new JObject();
            obj3["type"] = "orderbook";  //TypeEnums.UpbitDataType.orderbook.ToString();
            obj3["codes"] = array;

            string sendMsg = string.Format("[{0},{1},{2}]", obj1.ToString(), obj2.ToString(), obj3.ToString());



            WebSocket webSocket = new WebSocket("wss://api.upbit.com/websocket/v1");
            webSocket.OnMessage += eventHandler;//Ws_OnMessage;
            webSocket.Connect();
            if (webSocket.ReadyState == WebSocketState.Open)
            {
                webSocket.Send(sendMsg);
            }

            return null;
        }

        private void Ws_OnMessage(object sender, MessageEventArgs e)
        {
            string requestMsg = Encoding.UTF8.GetString(e.RawData);
            Console.WriteLine(requestMsg);
        }

        
        /// <summary>
        /// Upbit Market에 상장된 모든 코인들의 정보를 불러와 리턴합니다.
        /// </summary>
        public async Task<string> GetMarkets()
        {
            string url = "https://api.upbit.com/v1/market/all";
            HttpClient client = new();
            HttpResponseMessage httpResponse = await client.GetAsync(url);
            httpResponse.EnsureSuccessStatusCode();
            string responseBody = await httpResponse.Content.ReadAsStringAsync();
            
            return responseBody;
        }
    }
}

// 특정 코인 시세조회
// string url = "https://api.upbit.com/v1/ticker";