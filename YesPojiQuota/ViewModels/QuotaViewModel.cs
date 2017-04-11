using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YesPojiQuota.Data;

namespace YesPojiQuota.ViewModels
{
    public class QuotaViewModel : MainViewModel
    {
        public QuotaViewModel()
        {

        }

        public QuotaViewModel(Quota q)
        {
            Quota = q.Available;
        }


        private decimal _quota;

        public decimal Quota
        {
            get { return _quota; }
            set { Set("Quota", ref _quota, value); }
        }

    }
}
