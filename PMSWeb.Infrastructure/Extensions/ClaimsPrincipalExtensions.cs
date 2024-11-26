namespace Microsoft.Extensions.DependencyInjection
{
    using System.Security.Claims;

    public static class ClaimsPrincipalExtensions
    {
        public static string? GetUserId(this ClaimsPrincipal userClaimsPrincipal)
        {
            if (userClaimsPrincipal is null)
            {
                throw new ArgumentNullException(nameof(userClaimsPrincipal));
            }

            return userClaimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);
        }

    }
}
