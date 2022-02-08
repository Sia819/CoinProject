using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpbitUI.Model
{
    public class CoinInfo : ViewModelBase
    {
        private double _currentPrice;

        public string CoinNameKor { get; set; }
        public string CoinNameEng { get; set; }
        public double CurrentPrice { get => _currentPrice; set => Set(ref _currentPrice, value); }
        public double Change24 { get; set; }
    }
}
