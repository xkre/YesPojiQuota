
namespace YesPojiQuotaUtmLibs.Enums
{
    public enum LoginFailureReason
    {
        UserNotFound = 1,
        WrongPassword = 2,
        NoZoneAccess = 3,
        InactivatedAccessMode = 4,
        InconsistentAccessMode = 5,
        DisabledUserAccount = 6,
        MaximumDeviceReaed = 7,
        ExceededQuota = 8,
        UTMIdNotFound = 9,
        NoTimeleft = 10,
        MaximumNoOfSimultaneousSessions = 11,
        UsernamePasswordIncorrect = 12,
        InternalSyntaxError = 13,
        InactivatedRealm = 14,
        FailedOpaqueCredentials = 15,
        ServiceTemporaryClosed = 16,
        LDAPLookupFailde = 17,
        InactiveUserAccount = 18,
        ExpiredAccount = 19,
        HTTPError = 100
    }
}
