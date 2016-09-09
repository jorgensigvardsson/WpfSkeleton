namespace AutoConstruct.Tests
{
    internal class MultipleConstructorClass
    {
        public MultipleConstructorClass(IDependency1 dep1, IDependency2 dep2)
        {
        }

        public MultipleConstructorClass(IDependency2 dep2, IDependency1 dep1)
        {
        }
    }
}