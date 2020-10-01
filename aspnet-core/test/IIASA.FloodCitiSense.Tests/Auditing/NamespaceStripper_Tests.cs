using IIASA.FloodCitiSense.Auditing;
using Shouldly;
using Xunit;

namespace IIASA.FloodCitiSense.Tests.Auditing
{
    public class NamespaceStripper_Tests: AppTestBase
    {
        private readonly INamespaceStripper _namespaceStripper;

        public NamespaceStripper_Tests()
        {
            _namespaceStripper = Resolve<INamespaceStripper>();
        }

        [Fact]
        public void Should_Stripe_Namespace()
        {
            var controllerName = _namespaceStripper.StripNameSpace("IIASA.FloodCitiSense.Web.Controllers.HomeController");
            controllerName.ShouldBe("HomeController");
        }

        [Theory]
        [InlineData("IIASA.FloodCitiSense.Auditing.GenericEntityService`1[[IIASA.FloodCitiSense.Storage.BinaryObject, IIASA.FloodCitiSense.Core, Version=1.10.1.0, Culture=neutral, PublicKeyToken=null]]", "GenericEntityService<BinaryObject>")]
        [InlineData("CompanyName.ProductName.Services.Base.EntityService`6[[CompanyName.ProductName.Entity.Book, CompanyName.ProductName.Core, Version=1.10.1.0, Culture=neutral, PublicKeyToken=null],[CompanyName.ProductName.Services.Dto.Book.CreateInput, N...", "EntityService<Book, CreateInput>")]
        [InlineData("IIASA.FloodCitiSense.Auditing.XEntityService`1[IIASA.FloodCitiSense.Auditing.AService`5[[IIASA.FloodCitiSense.Storage.BinaryObject, IIASA.FloodCitiSense.Core, Version=1.10.1.0, Culture=neutral, PublicKeyToken=null],[IIASA.FloodCitiSense.Storage.TestObject, IIASA.FloodCitiSense.Core, Version=1.10.1.0, Culture=neutral, PublicKeyToken=null],]]", "XEntityService<AService<BinaryObject, TestObject>>")]
        public void Should_Stripe_Generic_Namespace(string serviceName, string result)
        {
            var genericServiceName = _namespaceStripper.StripNameSpace(serviceName);
            genericServiceName.ShouldBe(result);
        }
    }
}
