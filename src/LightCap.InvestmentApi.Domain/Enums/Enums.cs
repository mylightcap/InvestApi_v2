namespace LightCap.InvestmentApi.Domain;

public enum ApprovalStatusEnum
{
    Unknown = 0,
    Pending = 1,
    Approved = 2,
    Rejected = 3
}
public enum MonthEnum
{
    January = 1,
    February = 2,
    March = 3,
    April = 4,
    May = 5,
    June = 6,
    July = 7,
    August = 8,
    September = 9,
    October = 10,
    November = 11,
    December = 12
}

public enum Activities
{
   
}

public enum InvestmentLifestyleEnum
{
  
}

public enum SavingsInvestmentEnum
{
}
public enum TransactionStatus
{
    Unknown = 0,
    Pending,
    Success,
    Failed
}

public enum ChannelType
{
    Mobile = 1,
    Web = 2,
}

public enum MessageType
{
    
}

public enum NotificationStatus
{
    Success = 1,
    Failed = 2,
    Pending = 3,
    Info = 4,
    Authenticate = 10,
    Declined = 11
}

public enum NotificationChannel
{
    WebSocket = 1,
    PushNotification = 2,
    Both = 3
}

public enum DocumentStatus
{
    Pending = 1,
    Approved = 2,
    Rejected = 3
}

public enum DocumentType
{
    Identity = 2,
    Identity_Back = 3,
    Passport = 4,
    Resident_Permit = 5,
    Utility = 6,
    Signature = 7,
    Residential_Address = 9,
    WorkId = 10,
    WorkId_Back = 11,
    Statement = 12,
    TIN = 13,
    CertificateCAC = 15,
    CertificateSCUML = 16,
    CBNLicense = 17,
    Referee = 18
}