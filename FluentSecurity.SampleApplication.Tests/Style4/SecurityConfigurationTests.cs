using FluentSecurity.Policy;
using FluentSecurity.SampleApplication.Controllers;
using FluentSecurity.SampleApplication.Models;
using FluentSecurity.TestHelper;
using NUnit.Framework;

namespace FluentSecurity.SampleApplication.Tests.Style4
{
	[TestFixture]
	[Category("SecurityConfigurationTests")]
	public class When_security_is_configured
	{
		[Test]
		public void Should_be_configured_correctly()
		{
			var expectations = new PolicyExpectations();

			expectations.For<AccountController>(x => x.LogInAsAdministrator()).Has<DenyAuthenticatedAccessPolicy>();
			expectations.For<AccountController>(x => x.LogInAsPublisher()).Has<DenyAuthenticatedAccessPolicy>();
			expectations.For<AccountController>(x => x.LogOut()).Has<DenyAnonymousAccessPolicy>();

			expectations.For<ExampleController>(x => x.DenyAnonymousAccess()).Has<DenyAnonymousAccessPolicy>();
			expectations.For<ExampleController>(x => x.DenyAuthenticatedAccess()).Has<DenyAuthenticatedAccessPolicy>();

			expectations.For<ExampleController>(x => x.RequireAdministratorRole()).Has(new RequireRolePolicy(UserRole.Administrator));
			expectations.For<ExampleController>(x => x.RequirePublisherRole()).Has(new RequireRolePolicy(UserRole.Publisher));

			expectations.For<AdminController>().Has<AdministratorPolicy>();
			expectations.For<AdminController>(x => x.Index()).Has<IgnorePolicy>().DoesNotHave<AdministratorPolicy>();

			var results = expectations.VerifyAll(Bootstrapper.SetupFluentSecurity());

			Assert.That(results.Valid(), results.ErrorMessages());
		}
	}
}