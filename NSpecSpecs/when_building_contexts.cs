using System.Linq;
using NSpec;
using NSpec.Domain;
using NUnit.Framework;
using Rhino.Mocks;

namespace NSpecNUnit.when_building_contexts
{
    public class child : parent{}
    public class sibling: parent{}
    public class parent : nspec{}

    [TestFixture]
    public class when_building_contexts
    {
        [SetUp]
        public void setup()
        { 
            finder = MockRepository.GenerateMock<ISpecFinder>();

            finder.Stub(f => f.SpecClasses()).IgnoreArguments().Return(new[] { typeof(child), typeof(parent), typeof(sibling) });

            builder = new ContextBuilder(finder);

            builder.Contexts();
        }

        [Test]
        public void should_get_specs_from_specFinder()
        {
            finder.AssertWasCalled(f => f.SpecClasses());
        }

        [Test]
        public void the_primary_context_should_be_parent()
        {
            builder.Contexts().First().Name.should_be(typeof(parent).Name);
        }

        [Test]
        public void the_parent_should_have_the_child_context()
        {
            builder.Contexts().First().Contexts.First().Name.should_be(typeof(child).Name);
        }

        [Test]
        public void it_should_only_have_the_parent_once()
        {
            builder.Contexts().Count().should_be(1);
        }

        [Test]
        public void it_should_have_the_sibling()
        {
            builder.Contexts().First().Contexts.should_contain(c=>c.Name==typeof(sibling).Name);
        }

        private ISpecFinder finder;
        private ContextBuilder builder;
    }
}