using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using UpbitUI.Model;
using WebSocketSharp;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.ComponentModel;
using GalaSoft.MvvmLight;
using System.Windows;

namespace UpbitUI.ViewModel
{

    public class MainViewModel : ViewModelBase
    {
        #region Private Field
        private ObservableCollection<CoinInfo> _coinInfos;
        #endregion


        #region Public Property
        public ObservableCollection<CoinInfo> CoinInfos { get => _coinInfos; set => Set(ref _coinInfos, value); }
        #endregion


        #region Public Command
        public ICommand Test1 { get; set; }
        #endregion


        #region Constructor
        public MainViewModel()
        {
            // ViewModel Property Init
            this.CoinInfos = new ObservableCollection<CoinInfo>();
            this.Test1 = new RelayCommand(Func1);

            CoinInfo testinfo1 = new CoinInfo()
            {
                CoinNameEng = "BTC",
                CoinNameKor = "비트코인",
                CurrentPrice = 5000000,
                Change24 = -5.5
            };
            CoinInfo testinfo2 = new CoinInfo()
            {
                CoinNameEng = "ETH",
                CoinNameKor = "이더리움",
                CurrentPrice = 2000000,
                Change24 = +5.4
            };

            CoinInfos.Add(testinfo1);
            CoinInfos.Add(testinfo2);
        }
        #endregion


        #region Custom Function
        /// <summary>
        /// Temp Test Function !
        /// </summary>
        private void Func1()
        {
            // Upbit API Test
            UpbitAPI.WebRequest web = new UpbitAPI.WebRequest();

            // 웹소켓을 연결하여, 결과를 받을 때 마다 해당 이벤트 함수를 실행시킵니다.
            //web.WebSocketRequest(Ws_OnMessage);

            // 모든 업비트 상장 코인을 불러옵니다.
            var awaiter = web.GetMarkets().GetAwaiter();
            awaiter.OnCompleted(() => 
            {
                string httpResponse = awaiter.GetResult();
                _ = MessageBox.Show(httpResponse);
            });

        }
        #endregion


        #region Custom Events
        private void Ws_OnMessage(object sender, MessageEventArgs e)
        {
            string responseMessage = Encoding.UTF8.GetString(e.RawData);
            JObject result = JObject.Parse(responseMessage);
            string parseString = null;
            try
            {
                if (result.SelectToken("type").ToString() == "trade")// 트레이드일때만 하기
                {
                    parseString = result.SelectToken("trade_price").ToString();
                    double.TryParse(parseString, out double coinPriceData);
                    CoinInfos[0].CurrentPrice = coinPriceData;
                }
            }
            catch (Exception ex)
            {

            }
            RaisePropertyChanged(nameof(CoinInfos));
        }
        #endregion
    }
}

