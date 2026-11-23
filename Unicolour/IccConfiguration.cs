using Wacton.Unicolour.Icc;

namespace Wacton.Unicolour;

public class IccConfiguration
{
    public static readonly IccConfiguration None = new(null, name: "None");

    public Profile? Profile { get; }
    public Intent Intent { get; }
    public bool NoClip { get; }
    public string? Error { get; } 
    public string Name { get; }

    // NOTE: this is not the same as the ICC profile's PCS (profile connection space)
    // for simplicity and consistency, Unicolour converts ICC values via XYZ (even though typical profile PCS is LAB)
    // except when there is no profile, in which case we fall back to naive uncalibrated CMYK which is calculated directly from RGB
    internal bool HasSupportedProfile => Profile != null && Error == null;
    internal ColourSpace ConnectingSpace => HasSupportedProfile ? ColourSpace.Xyz : ColourSpace.Rgb;

    public IccConfiguration(string profilePath, Intent intent, bool noClip = false, string name = Utils.Unnamed) : this(GetProfile(profilePath), intent, noClip, name) {}
    public IccConfiguration(byte[] bytes, Intent intent, bool noClip = false, string name = Utils.Unnamed) : this(GetProfile(bytes), intent, noClip, name) {}
    public IccConfiguration(Stream stream, Intent intent, bool noClip = false, string name = Utils.Unnamed) : this(GetProfile(stream), intent, noClip, name) {}
    private IccConfiguration((Profile? profile, string? error) profileResult, Intent intent, bool noClip, string name)
    {
        if (profileResult.profile == null)
        {
            Profile = null;
            Intent = intent;
            NoClip = noClip;
            Error = profileResult.error;
            Name = name;
            return;
        }
        
        // profile header intent related to how a profile combines with another profile
        // which seems to make sense here since Unicolour is effectively the PCS between the two
        Profile = profileResult.profile;
        Intent = intent == Intent.Unspecified ? Profile.Header.Intent : intent;
        NoClip = noClip;
        Error = GetProfileSupportError(Profile);
        Name = name;
    }
    
    public IccConfiguration(Profile profile, Intent intent, bool noClip = false, string name = Utils.Unnamed)
    {
        Profile = profile;
        Intent = intent == Intent.Unspecified ? Profile.Header.Intent : intent;
        NoClip = noClip;
        Error = GetProfileSupportError(Profile);
        Name = name;
    }
    
    public IccConfiguration(Profile? profile, bool noClip = false, string name = Utils.Unnamed)
    {
        Profile = profile;
        Intent = Profile == null ? Intent.Unspecified : Profile.Header.Intent;
        NoClip = noClip;
        Error = Profile == null ? null : GetProfileSupportError(Profile);
        Name = name;
    }
    
    private static (Profile? profile, string? error) GetProfile(string filePath) => GetProfile(filePath, x => new Profile(x));
    private static (Profile? profile, string? error) GetProfile(byte[] bytes) => GetProfile(bytes, x => new Profile(x));
    private static (Profile? profile, string? error) GetProfile(Stream stream) => GetProfile(stream, x => new Profile(x));
    private static (Profile? profile, string? error) GetProfile<T>(T parameter, Func<T, Profile> getProfile)
    {
        Profile? profile = null;
        string? error = null;

        try
        {
            profile = getProfile(parameter);
        }
        catch (Exception e)
        {
            error = e.Message;
        }

        return (profile, error);
    }

    private static string? GetProfileSupportError(Profile profile)
    {
        try
        {
            profile.ErrorIfUnsupported();
        }
        catch (Exception e)
        {
            return e.Message;
        }

        return null;
    }
    
    public override string ToString() => $"{Name} · profile {(Profile == null ? "-" : Profile.Name)} · intent {Intent}";
}