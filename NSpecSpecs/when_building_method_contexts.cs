using System;
using System.Linq;
using System.Reflection;
using NSpec;
using NSpec.Domain;
using NUnit.Framework;
using Rhino.Mocks;

namespace NSpecNUnit
{
    [TestFixture]
    public class when_building_method_contexts
    {
        private Context classContext;

        private class SpecClass : nspec
        {
            public void public_method() { }

            void private_method() { }

            void before_each() { }

            void act_each() { }
        }

        [SetUp]
        public void setup()
        {
            var finder = MockRepository.GenerateMock<ISpecFinder>();

            var builder = new ContextBuilder(finder);

            classContext = new Context("class");

            builder.BuildMethodContexts(classContext, typeof(SpecClass));
        }

        [Test]
        public void it_should_add_the_public_method_as_a_sub_context()
        {
            classContext.Contexts.should_contain(c => c.Name == "public method");
        }

        [Test]
        public void it_should_not_create_a_sub_context_for_the_private_method()
        {
            classContext.Contexts.should_contain(c => c.Name == "private method");
        }

        [Test]
        public void it_should_disregard_method_called_before_each()
        {
            classContext.Contexts.should_not_contain(c => c.Name == "before each");   
        }

        [Test]
        public void it_should_disregard_method_called_act_each()
        {
            classContext.Contexts.should_not_contain(c => c.Name == "act each");
        }
    }
}