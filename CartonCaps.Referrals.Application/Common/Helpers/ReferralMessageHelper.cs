using System.Text;

namespace CartonCaps.Referrals.Application.Common.Helpers
{
    public static class ReferralMessageHelper
    {
        public static string ReplaceTokens(string template, Dictionary<string, string> tokens)
        {
            var result = new StringBuilder();
            result.Append(template);
            foreach (var token in tokens)
            {
                result = result.Replace($"[{token.Key}]", token.Value);
            }
            return result.ToString();
        }
    }
}
