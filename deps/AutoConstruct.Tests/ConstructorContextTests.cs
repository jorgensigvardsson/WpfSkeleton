using System;
using Moq;
using NUnit.Framework;

namespace AutoConstruct.Tests
{
    [TestFixture]
    public class ConstructorContextTests
    {
        [Test]
        public void ClassesWithNonPublicConstructorsAreRejected()
        {
            try
            {
                // ReSharper disable once ObjectCreationAsStatement
                new ConstructorContext<NonPublicConstructorClass>();
            }
            catch (ArgumentException ex)
            {
                Assert.AreEqual("There are no public constructors for type NonPublicConstructorClass.", ex.Message);
            }
            catch (Exception ex)
            {
                Assert.Fail("Unexpected exception: " + ex.GetType().Name);
            }
        }

        [Test]
        public void ClassesWithMultipleConstructorsRequireAssistance()
        {
            try
            {
                // ReSharper disable once ObjectCreationAsStatement
                new ConstructorContext<MultipleConstructorClass>();
            }
            catch (ArgumentException ex)
            {
                Assert.AreEqual("There are more than one constructor for type MultipleConstructorClass. Use overloaded constructor to disambiguate.", ex.Message);
            }
            catch (Exception ex)
            {
                Assert.Fail("Unexpected exception: " + ex.GetType().Name);
            }
        }

        [Test]
        public void InvalidConstructorDisambiguationIsRejected()
        {
            try
            {
                // ReSharper disable once ObjectCreationAsStatement
                new ConstructorContext<MultipleConstructorClass>(typeof (string), typeof(int));
            }
            catch (ArgumentException ex)
            {
                Assert.AreEqual("No constructor of MultipleConstructorClass matches (String, Int32)", ex.Message);
            }
            catch (Exception ex)
            {
                Assert.Fail("Unexpected exception: " + ex.GetType().Name);
            }
        }

        [Test]
        public void ValidConstructorDisambiguationIsAccepted()
        {
            // ReSharper disable once ObjectCreationAsStatement
            Assert.DoesNotThrow(() => new ConstructorContext<MultipleConstructorClass>(typeof (IDependency1), typeof (IDependency2)));
        }

        [Test]
        public void InjectionsOfUnnamedMocksThatDoNotMatchAnyConstructorArgumentAreRejected()
        {
            try
            {
                var context = new ConstructorContext<SingleConstructorClass>();
                context.Inject(new Mock<IDependency3>());
            }
            catch (ArgumentException ex)
            {
                Assert.AreEqual("Cannot autoinject parameter of type IDependency3 as it is not used in any constructor of SingleConstructorClass", ex.Message);
            }
            catch (Exception ex)
            {
                Assert.Fail("Unexpected exception: " + ex.GetType().Name);
            }
        }

        [Test]
        public void InjectionsOfUnnamedValuesThatDoNotMatchAnyConstructorArgumentAreRejected()
        {
            try
            {
                var context = new ConstructorContext<SingleConstructorClass>();
                context.Inject(123.45);
            }
            catch (ArgumentException ex)
            {
                Assert.AreEqual("Cannot autoinject parameter of type Double as it is not used in any constructor of SingleConstructorClass", ex.Message);
            }
            catch (Exception ex)
            {
                Assert.Fail("Unexpected exception: " + ex.GetType().Name);
            }
        }

        [Test]
        public void MultipleInjectionsOfUnnamedMocksAreRejected()
        {
            try
            {
                var context = new ConstructorContext<SingleConstructorClass>();
                context.Inject(new Mock<IDependency2>());
                context.Inject(new Mock<IDependency2>());
            }
            catch (ArgumentException ex)
            {
                Assert.AreEqual("Unnamed parameter of type IDependency2 has already been injected.", ex.Message);
            }
            catch (Exception ex)
            {
                Assert.Fail("Unexpected exception: " + ex.GetType().Name);
            }
        }

        [Test]
        public void MultipleInjectionsOfUnnamedValuesAreRejected()
        {
            try
            {
                var context = new ConstructorContext<SingleConstructorClass>();
                context.Inject(123);
                context.Inject(123);
            }
            catch (ArgumentException ex)
            {
                Assert.AreEqual("Unnamed parameter of type Int32 has already been injected.", ex.Message);
            }
            catch (Exception ex)
            {
                Assert.Fail("Unexpected exception: " + ex.GetType().Name);
            }
        }

        [Test]
        public void InjectionsOfNamedMocksThatDoNotMatchAnyConstructorArgumentNameAreRejected()
        {
            try
            {
                var context = new ConstructorContext<SingleConstructorClass>();
                context.Inject(new Mock<IDependency2>(), parameterName: "lukeSkywalker");
            }
            catch (ArgumentException ex)
            {
                Assert.AreEqual("Named parameter lukeSkywalker is not among formal parameters of constructor.", ex.Message);
            }
            catch (Exception ex)
            {
                Assert.Fail("Unexpected exception: " + ex.GetType().Name);
            }
        }

        [Test]
        public void InjectionsOfNamedValuesThatDoNotMatchAnyConstructorArgumentNameAreRejected()
        {
            try
            {
                var context = new ConstructorContext<SingleConstructorClass>();
                context.Inject(123, parameterName: "lukeSkywalker");
            }
            catch (ArgumentException ex)
            {
                Assert.AreEqual("Named parameter lukeSkywalker is not among formal parameters of constructor.", ex.Message);
            }
            catch (Exception ex)
            {
                Assert.Fail("Unexpected exception: " + ex.GetType().Name);
            }
        }

        [Test]
        public void InjectionsOfNamedMocksThatDoNotMatchConstructorArgumentTypeAreRejected()
        {
            try
            {
                var context = new ConstructorContext<SingleConstructorClass>();
                context.Inject(new Mock<IDependency3>(), parameterName: "dep2");
            }
            catch (ArgumentException ex)
            {
                Assert.AreEqual("Named parameter dep2 is not of type IDependency2", ex.Message);
            }
            catch (Exception ex)
            {
                Assert.Fail("Unexpected exception: " + ex.GetType().Name);
            }
        }

        [Test]
        public void InjectionsOfNamedValuesThatDoNotMatchConstructorArgumentTypeAreRejected()
        {
            try
            {
                var context = new ConstructorContext<SingleConstructorClass>();
                context.Inject(123, parameterName: "dep2");
            }
            catch (ArgumentException ex)
            {
                Assert.AreEqual("Named parameter dep2 is not of type IDependency2", ex.Message);
            }
            catch (Exception ex)
            {
                Assert.Fail("Unexpected exception: " + ex.GetType().Name);
            }
        }

        [Test]
        public void MultipleInjectionsOfNamedMocksAreRejected()
        {
            try
            {
                var context = new ConstructorContext<SingleConstructorClass>();
                context.Inject(new Mock<IDependency2>(), parameterName: "dep2");
                context.Inject(new Mock<IDependency2>(), parameterName: "dep2");
            }
            catch (ArgumentException ex)
            {
                Assert.AreEqual("Named parameter dep2 has already been injected.", ex.Message);
            }
            catch (Exception ex)
            {
                Assert.Fail("Unexpected exception: " + ex.GetType().Name);
            }
        }

        [Test]
        public void MultipleInjectionsOfNamedValuesAreRejected()
        {
            try
            {
                var context = new ConstructorContext<SingleConstructorClass>();
                context.Inject(123, parameterName: "value");
                context.Inject(123, parameterName: "value");
            }
            catch (ArgumentException ex)
            {
                Assert.AreEqual("Named parameter value has already been injected.", ex.Message);
            }
            catch (Exception ex)
            {
                Assert.Fail("Unexpected exception: " + ex.GetType().Name);
            }
        }

        [Test]
        public void TheCorrectMocksAndValuesAreUsedForTargetConstructor()
        {
            var context = new ConstructorContext<SingleConstructorClass>();
            var dep1 = context.Inject(new Mock<IDependency1>());
            var dep2 = context.Inject(new Mock<IDependency2>(), parameterName: "dep2");
            var value = context.Inject(123);

            var obj = context.New();

            Assert.AreSame(dep1.Object, obj.Dep1);
            Assert.AreSame(dep2.Object, obj.Dep2);
            Assert.AreEqual(value, obj.Value);
        }

        [Test]
        public void UninjectedMockableTypesAreMockedAutomatically()
        {
            var context = new ConstructorContext<SingleConstructorClass>();
            var value = context.Inject(123);

            var obj = context.New();

            Assert.IsNotNull(obj.Dep1);
            Assert.IsNotNull(obj.Dep2);
            Assert.AreEqual(value, obj.Value);
        }

        [Test]
        public void UnmockableTypesAreRejectedInNew()
        {
            var context = new ConstructorContext<SingleConstructorClass>();
            // Ints are not mockable: var value = context.Inject(123);
            
            Assert.Throws<ArgumentException>(() => context.New());
        }

        [Test]
        public void NewMayOnlyBeCalledOnce()
        {
            var context = new ConstructorContext<SingleConstructorClass>();
            context.Inject(123);

            context.New();
            Assert.Throws<InvalidOperationException>(() => context.New());
        }
    }
}
