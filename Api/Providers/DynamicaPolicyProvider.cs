using Api.Handlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace Api.Providers
{
    public class DynamicPolicyProvider(IOptions<AuthorizationOptions> options) 
        : IAuthorizationPolicyProvider
    {
        public DefaultAuthorizationPolicyProvider PolicyProvider = new(options);

        public Task<AuthorizationPolicy> GetDefaultPolicyAsync()
            => PolicyProvider.GetDefaultPolicyAsync();

        public Task<AuthorizationPolicy?> GetFallbackPolicyAsync()
            => PolicyProvider.GetFallbackPolicyAsync();

        public Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
        {
            if (policyName.StartsWith("Role", StringComparison.OrdinalIgnoreCase))
            {
                var role = policyName["Role".Length..];

                var policy = new AuthorizationPolicyBuilder();

                policy.AddRequirements(new DynamicRoleRequirement(role));

                return Task.FromResult<AuthorizationPolicy?>(policy.Build());
            }

            return PolicyProvider.GetPolicyAsync(policyName);
        }
    }
}
