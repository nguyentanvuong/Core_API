

namespace WebApi.Helpers.Common
{
    public enum EnmSendTypeOption
    {
        Synchronize,
        AsSynchronize,
        Notifications
    }

    public enum EnmCacheAction
    {
        NoCached = 0,
        Cached = 1,
        ClearCached = 2
    }

    public enum EnmJsonResponse
    {
        O = 0,
        E = 1,
        W = 2
    }

    public enum EnmResultResponse
    {
        NOT_SUCCESS = -1,
        SUCCESS = 0,
        SYS_OVER_LIMIT = 1,
        SYS_APR_STATUS = 2,
        SYS_APR_AMT = 3,
        STK_CHANGE_BRANCH = 4,
        SYS_APR_WD = 5,
        SYS_APPROVAL_REQUIRED = 6,
        SYS_APR_RELEASE = 7,
        SYS_APR_CWR = 8,
        CIF_CHECK_APPROVAL = 9,
        SYS_APR_CRD = 10,
        MULTI_TRANS_ERR = 11,
        PMT_APR_MULTI = 12
    }
}
