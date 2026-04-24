using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Tools.Models.Tokens;

public class TokenSettingsModel
{
    /// <summary>
    /// The secret key used to sign the JWT tokens.
    /// </summary>
    public string SecretKey { get; set; } = string.Empty;
    /// <summary>
    /// The issuer of the JWT tokens.
    /// </summary>
    public string Issuer { get; set; } = string.Empty;
    /// <summary>
    /// The audience for which the JWT tokens are intended.
    /// </summary>
    public string Audience { get; set; } = string.Empty;
    /// <summary>
    /// The expiration time for the JWT tokens in minutes.
    /// </summary>
    public int ExpirationMinutes { get; set; } = 60;
    public int ExpirationSeconds => ExpirationMinutes * 60;
    public int ExpirationHours => ExpirationMinutes / 60;
    public int ExpirationDays => ExpirationHours / 24;

}


