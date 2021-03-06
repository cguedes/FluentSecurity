using System;
using System.Collections.Generic;
using System.Linq;
using FluentSecurity.TestHelper.Specification.TestData;
using Moq;
using NUnit.Framework;

namespace FluentSecurity.TestHelper.Specification
{
	[TestFixture]
	[Category("ExpectationVerifyerSpec")]
	public class When_creating_a_new_ExpectationVerifyer
	{
		[Test]
		public void Should_throw_when_configuration_is_null()
		{
			// Arrange
			const ISecurityConfiguration configuration = null;
			var expectationViolationHandler = new Mock<IExpectationViolationHandler>().Object;
			
			// Act & assert
			Assert.Throws<ArgumentNullException>(() => new ExpectationVerifyer(configuration, expectationViolationHandler));
		}

		[Test]
		public void Should_throw_when_exception_violation_handler_is_null()
		{
			// Arrange
			var configuration = new Mock<ISecurityConfiguration>().Object;
			const IExpectationViolationHandler expectationViolationHandler = null;

			// Act & assert
			Assert.Throws<ArgumentNullException>(() => new ExpectationVerifyer(configuration, expectationViolationHandler));
		}
	}

	[TestFixture]
	[Category("ExpectationVerifyerSpec")]
	public class When_verifying_expectations_with_ExpectationVerifyer
	{
		[Test]
		public void Should_throw_when_expectation_groups_is_null()
		{
			// Arrange
			var configuration = FluentSecurityFactory.CreateSecurityConfiguration();

			var expectationViolationHandler = new Mock<IExpectationViolationHandler>();
			expectationViolationHandler.Setup(x => x.Handle(It.IsAny<string>())).Returns(ExpectationResult.CreateSuccessResult);

			var expectationVerifyer = new ExpectationVerifyer(configuration, expectationViolationHandler.Object);
			IEnumerable<ExpectationGroup> expectationGroups = null;

			// Act & assert
			Assert.Throws<ArgumentNullException>(() => expectationVerifyer.VerifyExpectationsOf(expectationGroups));
		}

		[Test]
		public void Should_not_fail_and_return_14_results()
		{
			// Arrange
			var configuration = FluentSecurityFactory.CreateSecurityConfiguration();

			var expectationViolationHandler = new Mock<IExpectationViolationHandler>();
			expectationViolationHandler.Setup(x => x.Handle(It.IsAny<string>())).Returns(ExpectationResult.CreateSuccessResult);

			var expectationVerifyer = new ExpectationVerifyer(configuration, expectationViolationHandler.Object);
			var expectationGroups = FluentSecurityFactory.CreateExpectationsGroups();

			// Act
			var expectationResults = expectationVerifyer.VerifyExpectationsOf(expectationGroups);

			// Assert
			expectationViolationHandler.Verify(x => x.Handle(It.IsAny<string>()), Times.Never());
			Assert.That(expectationResults.Count(), Is.EqualTo(14));
		}

		[Test]
		public void Should_fail_11_times_and_return_14_results()
		{
			// Arrange
			var configuration = FluentSecurityFactory.CreateEmptySecurityConfiguration();

			var expectationViolationHandler = new Mock<IExpectationViolationHandler>();
			expectationViolationHandler.Setup(x => x.Handle(It.IsAny<string>())).Returns(ExpectationResult.CreateSuccessResult);

			var expectationVerifyer = new ExpectationVerifyer(configuration, expectationViolationHandler.Object);
			var expectationGroups = FluentSecurityFactory.CreateExpectationsGroups();

			// Act
			var expectationResults = expectationVerifyer.VerifyExpectationsOf(expectationGroups);

			// Assert
			expectationViolationHandler.Verify(x => x.Handle(It.IsAny<string>()), Times.Exactly(11));
			Assert.That(expectationResults.Count(), Is.EqualTo(14));
		}

		[Test]
		public void Should_fail_13_times_and_return_14_results_for_configuration_with_no_expectations()
		{
			// Arrange
			var configuration = FluentSecurityFactory.CreateSecurityConfigurationWithTwoExpectations();

			var expectationViolationHandler = new Mock<IExpectationViolationHandler>();
			expectationViolationHandler.Setup(x => x.Handle(It.IsAny<string>())).Returns(ExpectationResult.CreateSuccessResult);

			var expectationVerifyer = new ExpectationVerifyer(configuration, expectationViolationHandler.Object);
			var expectationGroups = FluentSecurityFactory.CreateExpectationsGroups();

			// Act
			var expectationResults = expectationVerifyer.VerifyExpectationsOf(expectationGroups);

			// Assert
			expectationViolationHandler.Verify(x => x.Handle(It.IsAny<string>()), Times.Exactly(13));
			Assert.That(expectationResults.Count(), Is.EqualTo(14));
		}
	} 
}