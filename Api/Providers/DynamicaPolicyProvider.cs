using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Api.Handlers;

namespace Api.Providers
{
    public class DynamicPolicyProvider : IAuthorizationPolicyProvider
    {
        public DefaultAuthorizationPolicyProvider PolicyProvider;

        public DynamicPolicyProvider(IOptions<AuthorizationOptions> options)
        {
            PolicyProvider = new(options);
        }

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
