using PasswordClassLibrary.Validation.ValidationRules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordClassLibrary.Validation
{
    /// <summary>
    /// Used to validate inputs based on provided validation rulesets.
    /// </summary>
    public static class Validator
    {
        static Object thisLock = new Object();
        static private List<ValidationRuleSet> validationRuleSets = new List<ValidationRuleSet>();

        /// <summary>
        /// Add a ruleset that can be used to validate against.
        /// </summary>
        /// <param name="validationRuleSet">Validation rulesset containing an identifying name and a list of IValdationRules</param>
        static public void AddRuleSet(ValidationRuleSet validationRuleSet)
        {
            // If the ruleset allready exists - replace it.
            lock (thisLock)
            {
                ValidationRuleSet validationRuleSetToReplace = validationRuleSets.SingleOrDefault(x => x.Name == validationRuleSet.Name);

                if (validationRuleSetToReplace != null)
                {
                    validationRuleSets.RemoveAll(x => x.Name == validationRuleSet.Name);
                }
            }

            // Add the new ruleset to the validator.
            Validator.validationRuleSets.Add(validationRuleSet);
        }

        /// <summary>
        /// Validate an input value against the rulseset with the provided name.
        /// Throws exceptions if any validation rules are broken.
        /// </summary>
        /// <param name="ruleSetName">Name of the ruleset to validate against. This must have been setup via the AddRuleSet method.</param>
        /// <param name="valueToValidate">Value to validate</param>
        static public void ValidateAndThrow(string ruleSetName, object valueToValidate)
        {
            // Check if the ruleset has been setup by the AddRuleSet method.
            ValidationRuleSet ruleSetToUse = validationRuleSets.SingleOrDefault(x => x.Name.ToUpper() == ruleSetName.ToUpper());

            // If the ruleset has not been set up, throw an exception.
            if (ruleSetToUse == null) throw new ArgumentException("The given rule set name does not exist.", nameof(ruleSetName));

            // Go through the validation rules contained in the ruleset one by one.
            // - Each IValidationRule must contain its own logic for validating.
            foreach (IValidationRule validationRule in ruleSetToUse.Rules)
            {
                validationRule.CheckRule(ruleSetToUse.Name, valueToValidate);
            }
        }

        /// <summary>
        /// Validate an input value against the rulseset with the provided name.
        /// Returns a lsit of all rules broken.
        /// </summary>
        /// <param name="ruleSetName">Name of the ruleset to validate against. This must have been setup via the AddRuleSet method.</param>
        /// <param name="valueToValidate">Value to validate</param>
        /// <returns>List<string> containing all the error messages of broken rules.</returns>
        static public IEnumerable<string> ValidateAndReturnErrors(string ruleSetName, object valueToValidate)
        {
            // Check if the ruleset has been setup by the AddRuleSet method.
            ValidationRuleSet ruleSetToUse = validationRuleSets.SingleOrDefault(x => x.Name == ruleSetName);

            if (ruleSetToUse == null) throw new ArgumentException("The given rule set name does not exist.", nameof(ruleSetName));

            // A list to contain the error messages.
            List<string> errorList = new List<string>();

            foreach (IValidationRule validationRule in ruleSetToUse.Rules)
            {
                try
                {
                    validationRule.CheckRule(ruleSetToUse.Name, valueToValidate);
                }
                catch (Exception e)
                {
                    // If the validation fails, and the IValidationRule throws an exception
                    // this will add it to the error list instead of throwing.
                    errorList.Add(e.Message);
                }
            }

            return errorList;
        }

    }
}
