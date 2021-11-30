using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordClassLibrary.Validation
{
    /// <summary>
    /// A ruleset containing a list of rules to validate against.
    /// </summary>
    public class ValidationRuleSet
    {
        /// <summary>
        /// The name of the ruleset, this will be used to select the rule set for later validation.
        /// </summary>
        internal string Name { get; private set; }
        
        /// <summary>
        /// The list of rules this ruleset should enforce, when validated against.
        /// </summary>
        internal List<IValidationRule> Rules { get; private set; }

        /// <summary>
        /// A ruleset containing a list of rules to validate against, as well as an name to identify it by.
        /// </summary>
        /// <param name="name">The name of the ruleset.</param>
        /// <param name="ruleSet">The list of rules this ruleset should enforce, when validated against.</param>
        public ValidationRuleSet(string name, List<IValidationRule> ruleSet)
        {
            Name = name;
            Rules = ruleSet;
        }
    }
}
