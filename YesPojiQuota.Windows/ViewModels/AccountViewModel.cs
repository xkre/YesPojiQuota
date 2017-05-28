using GalaSoft.MvvmLight.Messaging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using YesPojiQuota.Core.Data;
using YesPojiQuota.Core.Interfaces;
using YesPojiQuota.Core.Helpers;
using YesPojiQuota.Core.Enums;
using YesPojiQuota.Core.Models;
using YesPojiQuota.Utils;
using YesPojiQuota.Core.Observers;
using GalaSoft.MvvmLight.Threading;

namespace YesPojiQuota.ViewModels
{
    public class AccountViewModel : MainViewModel
    {
        public delegate void MyEventHandler(object source);
        public event MyEventHandler OnSuccessOperation;
        public event MyEventHandler Removed;

        //private bool _isLoaded = false;

        private YesContext _db;
        private ILoginService _ls;
        private NetworkChangeHandler _nch;

        #region Constructors
        public AccountViewModel(YesContext db, ILoginService ls, NetworkChangeHandler nch)
        {
            _db = db;
            _ls = ls;
            _nch = nch;
        }

        #endregion Constructors

        #region Properties
        private Account _account;
        public Account Account
        {
            get => _account;
            set
            {
                Username = value.Username;
                Password = value.Password;
                Id = value.AccountId;

                var qvm = ServiceLocator.Current.GetInstance<QuotaViewModel>(Id.ToString());
                qvm.Quota = value.Quota;
                Quota = qvm;

                Set("Account", ref _account, value);
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

        private QuotaViewModel _quota;
        public QuotaViewModel Quota
        {
            get => _quota;
            set
            {
                Set("Quota", ref _quota, value);
                RaisePropertyChanged("QuotaString");
            }
        }

        public bool LoginEnabled
        {
            get => Password?.Length > 0;
        }

        private bool _canLogin;
        public new bool CanLogin
        {
            get => _canLogin && LoginEnabled;
            set => Set("CanLogin", ref _canLogin, value);
        }

        private bool _enableLogin;
        public bool EnableLogin
        {
            get => _enableLogin;
            set => Set("EnableLogin", ref _enableLogin, value);
        }
        #endregion Properties

        #region Methods
        public override async Task InitAsync()
        {
            await base.InitAsync();

            _nch.YesConnected += YesConnected;
            _nch.YesDisconnected += YesDisconnected;

            await Quota.InitAsync();
        }

        private void YesConnected()
        {
            DispatcherHelper.RunAsync(() => CanLogin = true);
        }

        private void YesDisconnected()
        {
            DispatcherHelper.RunAsync(() => CanLogin = false);
        }


        public async Task RefreshDataAsync() => await Quota.RefreshQuotaAsync();
        public async Task SaveDataIfNecessary() => await Quota.SaveDataIfNecessary();
        public async Task SaveData() => await Quota.SaveData();

        public async void Save()
        {
            if (!Validate(out string error))
            {
                SendNotificationMessage(error);
                return;
            }

            Username = Username.ToLower();
            Username += _type == AccountType.Student ? "@live.utm.my" : "@utm.my";

            if (null != _db.Accounts.Where(x => x.Username == Username).FirstOrDefault())
            {
                SendNotificationMessage("Username Already Exist");
                return;
            }

            string pw = null;
            if (Password != null)
            {
                pw = EncryptionHelper.AES_Encrypt(Password, Username);
            }
            var account = new Account(Username, pw);
            var quota = new Quota();

            account.Quota = quota;

            _db.Accounts.Add(account);
            await _db.SaveChangesAsync();

            Account = account;

            OnSuccessOperation(this);
        }

        public async void Login()
        {
            Messenger.Default.Send(new LoadingMessage() { IsLoading = true, Message = "Logging in" });
            var a = await _ls.LoginAsync(Username, EncryptionHelper.AES_Decrypt(Password, Username));
            if (a)
                Messenger.Default.Send(new LoadingMessage() { IsLoading = false, Message = "Successful log in" });
            else
                Messenger.Default.Send(new LoadingMessage() { IsLoading = false, Message = "fail log in" });

        }

        public async void Remove()
        {
            _db.Remove(Account);
            await _db.SaveChangesAsync();

            Removed(this);
        }

        public void RefreshCanConnectProperty()
        {
            var currentConnectivity = _nch.CurrentNetwork;

            if (currentConnectivity == NetworkCondition.YesWifiConnected ||
                currentConnectivity == NetworkCondition.Online)
            {
                DispatcherHelper.RunAsync(()=> CanLogin = true);
            }
        }

        private bool Validate(out string error)
        {
            error = "Username | Password cannot be blank";

            //Validate Username and password Entered
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
}
