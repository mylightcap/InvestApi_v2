namespace LightCap.InvestmentApi.Application.Common.Contracts.Abstractions;

public interface IUtilityService
{
    void SetInMemoryCache<TItem>(object key, TItem value, TimeSpan absoluteExpirationRelativeToNow);
    bool TryGetInMemoryCache<TItem>(string key, out TItem? value);
    void RemoveInMemoryCache(object key);

    (string UserId, string StaffId, string Email, string Role,string BranchId) GetClaims();

}