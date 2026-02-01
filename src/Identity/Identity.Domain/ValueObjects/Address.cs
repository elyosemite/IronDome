using System.Collections.Generic;
using Ardalis.SharedKernel;

namespace Identity.Domain.ValueObjects;

public class Address : ValueObject
{
    public string Street { get; private set; }
    public string Number { get; private set; }
    public string City { get; private set; }
    public string State { get; private set; }
    public string ZipCode { get; private set; }

    // EF Core requires empty constructor
    private Address() { }

    public Address(string street, string number, string city, string state, string zipCode)
    {
        Street = street;
        Number = number;
        City = city;
        State = state;
        ZipCode = zipCode;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Street;
        yield return Number;
        yield return City;
        yield return State;
        yield return ZipCode;
    }
}
