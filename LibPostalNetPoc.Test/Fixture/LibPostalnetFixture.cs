using LibPostalNet;

namespace LibPostalNetPoc.Test.Fixture;

public class LibPostalnetFixture:IDisposable
{
    public LibPostalnetFixture()
    {
        var dataPath = Path.Combine(Environment.CurrentDirectory, "libpostal_data"); 
        Assert.True(libpostal.LibpostalSetupDatadir(dataPath));
        Assert.True(libpostal.LibpostalSetupParserDatadir(dataPath));
        Assert.True(libpostal.LibpostalSetupLanguageClassifierDatadir(dataPath));
    }

    public void Dispose()
    {
        libpostal.LibpostalTeardown();
        libpostal.LibpostalTeardownParser();
        libpostal.LibpostalTeardownLanguageClassifier();
    }
}