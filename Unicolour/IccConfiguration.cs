using Wacton.Unicolour.Icc;

namespace Wacton.Unicolour;

public class IccConfiguration
{
    public static readonly IccConfiguration None = new(null, "None");

    public Profile? Profile { get; }
    public Intent Intent { get; }
    public string? Error { get; } 
    public string Name { get; }

    // NOTE: this is not the same as the ICC profile's PCS (profile connection space)
    // for simplicity and consistency, Unicolour converts ICC values via XYZ (even though typical profile PCS is LAB)
    // except when there is no profile, in which case we fall back to naive uncalibrated CMYK which is calculated directly from RGB
    internal bool HasSupportedProfile => Profile != null && Error == null;
    internal ColourSpace ConnectingSpace => HasSupportedProfile ? ColourSpace.Xyz : ColourSpace.Rgb;
    
    public IccConfiguration(string profilePath, Intent intent, string name = Utils.Unnamed)
    {
        Profile? profile;
        
        try
        {
            profile = new Profile(profilePath);
        }
        catch (Exception e)
        {
            Profile = null;
            Intent = intent;
            Error = e.Message;
            Name = name;
            return;
        }

        // profile header intent related to how a profile combines with another profile
        // which seems to make sense here since Unicolour is effectively the PCS between the two
        Profile = profile;
        Intent = intent == Intent.Unspecified ? Profile.Header.Intent : intent;
        Error = GetProfileSupportError(Profile, Intent);
        Name = name;
    }
    
    public IccConfiguration(Profile profile, Intent intent, string name = Utils.Unnamed)
    {
        Profile = profile;
        Intent = intent == Intent.Unspecified ? Profile.Header.Intent : intent;
        Error = GetProfileSupportError(Profile, Intent);
        Name = name;
    }
    
    public IccConfiguration(Profile? profile, string name = Utils.Unnamed)
    {
        Profile = profile;
        Intent = Profile == null ? Intent.Unspecified : Profile.Header.Intent;
        Error = Profile == null ? null : GetProfileSupportError(Profile, Intent);
        Name = name;
    }

    private static string? GetProfileSupportError(Profile profile, Intent intent)
    {
        try
        {
            profile.ErrorIfUnsupported(intent);
        }
        catch (Exception e)
        {
            return e.Message;
        }

        return null;
    }
    
    public override string ToString() => Name;
}