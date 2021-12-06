using PasswordClassLibrary.Hashing;
using PasswordClassLibrary.Validation;
using PasswordClassLibrary.Validation.ValidationRules;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PasswordClassLibrary.Tests
{
    public class ValidatorTests
    {

        public ValidatorTests()
        {
            

        }

        [Theory]
        [InlineData("testString1234")]
        [InlineData("t")]
        [InlineData(123)]
        [InlineData(0)]
        public void NoNullRule_ValidInput_ShouldPass(object valueToValidate)
        {
            // Arrange
            Validator.AddRuleSet(new ValidationRuleSet("testrule", new List<IValidationRule>() { new NoNullRule() }));

            // Act
            try
            {
                Validator.ValidateAndThrow("testrule", valueToValidate);

            }
            catch (Exception)
            {
                // Assert
                Assert.True(false);
            }
            // Assert
            Assert.True(true);
        }

        [Theory]
        [InlineData(null)]
        public void NoNullRule_InvalidInput_ShouldThrowArgumentNullException(object valueToValidate)
        {
            // Arrange
            Validator.AddRuleSet(new ValidationRuleSet("testrule", new List<IValidationRule>() { new NoNullRule() }));

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => Validator.ValidateAndThrow("testrule", valueToValidate));
        }

        [Theory]
        [InlineData("testString1234")]
        [InlineData("t")]
        public void NoEmptyStringRule_ValidInput_ShouldPass(object valueToValidate)
        {
            // Arrange
            Validator.AddRuleSet(new ValidationRuleSet("testrule", new List<IValidationRule>() { new NoEmptyStringRule() }));

            // Act
            try
            {
                Validator.ValidateAndThrow("testrule", valueToValidate);

            }
            catch (Exception)
            {
                // Assert
                Assert.True(false);
            }

            // Assert
            Assert.True(true);
        }

        [Theory]
        [InlineData("")]
        [InlineData("    ")]
        [InlineData(" ")]
        public void NoEmptyStringRule_InvalidInput_ShouldThrowArgumentException(object valueToValidate)
        {
            // Arrange
            Validator.AddRuleSet(new ValidationRuleSet("testrule", new List<IValidationRule>() { new NoEmptyStringRule() }));

            // Act & Assert
            Assert.Throws<ArgumentException>(() => Validator.ValidateAndThrow("testrule", valueToValidate));
        }

        [Theory]
        [InlineData("testmail@mail.dk")]
        [InlineData("t@t.com")]
        public void ValidMailRule_ValidInput_ShouldPass(object valueToValidate)
        {
            // Arrange
            Validator.AddRuleSet(new ValidationRuleSet("testrule", new List<IValidationRule>() { new ValidMailRule() }));

            // Act
            try
            {
                Validator.ValidateAndThrow("testrule", valueToValidate);

            }
            catch (Exception)
            {
                // Assert
                Assert.True(false);
            }

            // Assert
            Assert.True(true);
        }

        [Theory]
        [InlineData("t@")]
        [InlineData("testmail")]
        [InlineData("kjnsadnlqwe@")]
        public void ValidMailRule_InvalidInput_ShouldThrowArgumentException(object valueToValidate)
        {
            // Arrange
            Validator.AddRuleSet(new ValidationRuleSet("testrule", new List<IValidationRule>() { new ValidMailRule() }));

            // Act & Assert
            Assert.Throws<ArgumentException>(() => Validator.ValidateAndThrow("testrule", valueToValidate));
        }


        [Theory]
        [InlineData("testmail@mail.dk", "mail.dk")]
        [InlineData("testmail@t.dk", "t.dk")]
        [InlineData("test@zbc.dk", "zbc.dk")]
        [InlineData("test@ZBC.DK", "zbc.dk")]
        public void MustHaveMailDomain_ValidInput_ShouldPass(object valueToValidate, string domain)
        {
            // Arrange
            Validator.AddRuleSet(new ValidationRuleSet("testrule", new List<IValidationRule>() { new MustHaveMailDomainRule(domain) }));

            // Act

            Validator.ValidateAndThrow("testrule", valueToValidate);

            // Assert
            Assert.True(true);
        }


        [Theory]
        [InlineData("testmail@mail.dk", "zbc.dk")]
        [InlineData("testmail@t.dk", "zbc.dk")]
        [InlineData("test@wqe.dk", "zbc.dk")]
        [InlineData("test@daa.DK", "zbc.dk")]
        public void MustHaveMailDomainRule_InvalidInput_ShouldThrowArgumentException(string valueToValidate, string domain)
        {
            // Arrange
            Validator.AddRuleSet(new ValidationRuleSet("testrule", new List<IValidationRule>() { new MustHaveMailDomainRule(domain) }));

            // Act & Assert
            Assert.Throws<ArgumentException>(() => Validator.ValidateAndThrow("testrule", valueToValidate));
        }

    }
}
