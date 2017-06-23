using Scalable.Shared.Common;

namespace Scalable.Shared.Rules
{
    public abstract class RulesBase
    {
        protected void Check(IValidator rule)
        {
            var result = rule.Validate();
            if (!result.IsValid())
                throw new ValidationException(result);
        }
    }
}
