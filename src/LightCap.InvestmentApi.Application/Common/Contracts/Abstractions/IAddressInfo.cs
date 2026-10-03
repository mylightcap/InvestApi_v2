namespace CustOps.Application.Common.Contracts.Abstractions;

public interface IAddressInfo
{
    string Cif { get; }
    string HouseNumber { get; }
    string? FlatNumber { get; }
    string StreetName { get; }
    string Landmark { get; }
    string Country { get; }
    string State { get; }
    string City { get; }
    string Lga { get; }
    string? Lcda { get; }
    string? PostalCode { get; }
    string? Town { get; }
    string? AdditionalInformation { get; }
}
