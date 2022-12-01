using RulesEngine.Actions;
using RulesEngine.Models;

namespace DevTools.Core
{
    public class OnSuccessAction : ActionBase
    {
        public override ValueTask<object> Run(ActionContext context, RuleParameter[] ruleParameters)
        {
            return ValueTask.FromResult<object>(true);
        }
    }
}
