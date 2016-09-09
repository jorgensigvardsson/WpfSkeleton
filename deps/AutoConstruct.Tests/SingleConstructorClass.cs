namespace AutoConstruct.Tests
{
    internal class SingleConstructorClass
    {
        public SingleConstructorClass(IDependency1 dep1, IDependency2 dep2, int value)
        {
            Dep1 = dep1;
            Dep2 = dep2;
            Value = value;
        }

        public IDependency1 Dep1 { get; }
        public IDependency2 Dep2 { get; }
        public int Value { get; }
    }
}