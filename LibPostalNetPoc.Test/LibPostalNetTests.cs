using LibPostalNet;
using LibPostalNetPoc.Test.Fixture;

namespace LibPostalNetPoc.Test;

public class LibPostalNetTests:IClassFixture<LibPostalnetFixture>
{
    private LibPostalnetFixture _fixture;
    public LibPostalNetTests(LibPostalnetFixture fixture)
    {
        _fixture = fixture;
    }
    
    [Theory]
    [InlineData("Av. Beira Mar 1647 - Salgueiros, 4400-382 Vila Nova de Gaia")]
    [InlineData("1211 88th st Niagara falls 14304 NY")]
    [InlineData("100 main st buffalo ny")]
    public void ParseResultsOnlyContainsOriginalAddressElements(string query)
    {
        
        var response = libpostal.LibpostalParseAddress(query, new LibpostalAddressParserOptions());
        Assert.NotNull(response.Results);

        query = query.ToLower(); // because output for parse is lower case
        foreach (var result in response.Results)
        {
            Assert.Contains(result.Value,query);
        }

        libpostal.LibpostalAddressParserResponseDestroy(response);
    }

    [Theory]
    [InlineData(LibpostalNormalizeOptions.LIBPOSTAL_ADDRESS_HOUSE_NUMBER, true)]
    [InlineData(LibpostalNormalizeOptions.LIBPOSTAL_ADDRESS_UNIT, true)]
    [InlineData(LibpostalNormalizeOptions.LIBPOSTAL_ADDRESS_HOUSE_NUMBER|LibpostalNormalizeOptions.LIBPOSTAL_ADDRESS_UNIT, true)]
    public void UsingSettingOptions_ExpansionResponseIsShorter(ushort testOptions, bool setOptions)
    {
        var query = "123 main st apt 2";

        var options = libpostal.LibpostalGetDefaultOptions();
        var expansion = libpostal.LibpostalExpandAddress(query, options);

        if(setOptions) options.AddressComponents = testOptions;
        var expansion2 = libpostal.LibpostalExpandAddress(query, options);
        
        Assert.NotNull(expansion);
        Assert.NotNull(expansion.Expansions);
        Assert.True(expansion.Expansions.Length >= expansion2.Expansions.Length, "Expansions should be reduced.");
    }
}