using GalaSoft.MvvmLight.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using YesPojiQuota.Data;
using YesPojiQuota.Utils;
using Windows.UI.Core;
using System.Collections.Concurrent;
using GalaSoft.MvvmLight.Threading;
using YesPojiQuota.Utils.Interfaces;
using YesPojiQuota.Utils.Helpers;

namespace YesPojiQuota.ViewModels
{
    public class AccountViewModel : MainViewModel
    {
        public delegate void MyEventHandler(object source);
        public event MyEventHandler OnSuccessOperation;
        public event MyEventHandler Removed;

        private bool _isLoaded = false;

        #region Constructors
        public AccountViewModel(INavigationService navigationService) : base(navigationService)
        {
            Account = new Account();
        }

        public AccountViewModel() : base()
        {
            //Username = "";
            //Password = "";
        }

        public AccountViewModel(Account account) : this()
        {
            Account = account;
        }

        public AccountViewModel(string u, string p = "") : this()
        {
            var account = new Account
            {
                Username = u,
                Password = p
            };

            Account = account;
        }
        #endregion Constructors

        #region Properties
        private Account _account;
        public Account Account
        {
            get => _account;
            set
            {
                Set("Account", ref _account, value);
                Username = value.Username;
                Password = value.Password;
                Id = value.AccountId;
            }
        }

        private string _username;
        public string Username
        {
            get => _username;
            set => Set("Username", ref _username, value);
        }

        private string _password;
        public string Password
        {
            get => _password;
            set
            {
                Set("Password", ref _password, value);
                RaisePropertyChanged("CanLogin");
            }
        }

        private int _id;
        public int Id
        {
            get => _id;
            set => Set("Id", ref _id, value);
        }

        public List<string> AccountTypes
        {
            get
            {
                return new string[]{
                    "Student (@live.utm.my)",
                    "Staff (@utm.my)"
                }.ToList();
            }
        }

        private AccountType _type;
        public int Type
        {
            get { return (int)_type; }
            set { Set("Type", ref _type, (AccountType)value); }
        }

        private double _quota;
        public double Quota
        {
            get => _quota;
            set
            {
                Set("Quota", ref _quota, value);
                RaisePropertyChanged("QuotaString");
            }
        }

        public double MaxQuota
        {
            get
            {
                var a = ServiceLocator.Current.GetInstance<IQuotaService>();
                return a.GetMaxQuota(Username);
            }
        }

        public string QuotaString
        {
            get => String.Format($"{Quota:0.###} MB Available");
        }

        public new bool CanLogin
        {
            get => Password?.Length > 0;
        }

        private bool _enableLogin;
        public bool EnableLogin
        {
            get => _enableLogin;
            set => Set("EnableLogin", ref _enableLogin, value);
        }
        #endregion Properties

        #region Methods
        public override async Task Init()
        {
            await base.Init();
            if(!_isLoaded)
                InitQuotaFromDb();

            await InitQuota();

            _isLoaded = true;
        }

        public void LightInit()
        {
            InitQuotaFromDb();

            _isLoaded = true;
        }

        private void InitQuotaFromDb()
        {
            decimal quota = 0;
            try
            {
                var db = ServiceLocator.Current.GetInstance<YesContext>();

                var q = db.Quotas.Where(x => x.AccountId == Id).First();
                quota = q.Available;
            }
            catch (Exception ee)
            {
                Debug.WriteLine($"Exception {ee}");
            }

            DispatcherHelper.RunAsync(() => Quota = (double)quota);
        }

        private async Task InitQuota()
        {
            var quotaService = ServiceLocator.Current.GetInstance<IQuotaService>();

            decimal quota = 0;
            try
            {
                quota = await quotaService.GetQuota(Username);
                await SaveQuota(quota);

                await DispatcherHelper.RunAsync(() => Quota = (double)quota);
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Exception {e}");
            }
        }

        public async Task SaveQuota(decimal available)
        {
            var db = ServiceLocator.Current.GetInstance<YesContext>();

            var quota = await db.Quotas.Where(x => x.AccountId == Id).FirstOrDefaultAsync();
            if (quota == null)
            {
                quota = new Quota()
                {
                    AccountId = Id
                };
                db.Quotas.Add(quota);

                quota.Available = available;
                await db.SaveChangesAsync();
            }
            else if (Convert.ToDouble(quota.Available) != Quota)
            {
                quota.Available = available;
                await db.SaveChangesAsync();
            }
        }

        public async void Save()
        {
            if (!Validate(out string error))
            {
                SendNotificationMessage(error);
                return;
            }

            var db = ServiceLocator.Current.GetInstance<YesContext>();

            Username += _type == AccountType.Student ? "@live.utm.my" : "@utm.my";

            if (null != db.Accounts.Where(x => x.Username == Username).FirstOrDefault())
            {
                SendNotificationMessage("Username Already Exist");
                return;
            }
            string pw = null;
            if (Password != null)
            {
                pw = EncryptionHelper.AES_Encrypt(Password,Username);
            }
            var account = new Account(Username, pw); 
            db.Accounts.Add(account);
            await db.SaveChangesAsync();

            Account = account;

            OnSuccessOperation(this);
            InitQuota();
        }

        public async void Login()
        {
            var loginService = ServiceLocator.Current.GetInstance<ILoginService>();

            var a = await loginService.LoginAsync(Username, EncryptionHelper.AES_Decrypt(Password,Username));
        }

        public async void Remove()
        {
            var db = ServiceLocator.Current.GetInstance<YesContext>();

            db.Remove(Account);
            await db.SaveChangesAsync();

            Removed(this);
        }

        private bool Validate(out string error)
        {
            error = "Username | Password cannot be blank";

            if (null == Username)
                return false;
            if (EnableLogin && null == Password)
                return false;

            var filled = EnableLogin ? Username.Length > 0 && Password.Length > 0 : Username.Length > 0;

            if (!filled)
                return false;

            error = "Invalid Username | Password";
            var uValid = !Regex.Match(Username, " ").Success;
            var pValid = true;

            if (EnableLogin)
                pValid = !Regex.Match(Password, " ").Success;

            return uValid && pValid;
        }
        #endregion Methods
    }

    public enum AccountType
    {
        Student, Staff
    }
}
