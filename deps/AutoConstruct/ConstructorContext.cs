using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Moq;

namespace AutoConstruct
{
    /// <summary>
    /// Allows for easy construction of objects during test.
    /// 
    /// If a class has many constructor dependencies, it becomes increasingly impractical to
    /// unit test the class, as each instantiation will require the setup of many mocks.
    /// It is entirely possible that a method that is being tested, does not depend on all constructor
    /// dependencies. In such cases, having the setup of all those dependencies/mocks is too noicy.
    /// 
    /// Another advantags is that if the contructor dependencies changes for a class that has many unit tests,
    /// it will still compile, enabling red/green testing without having to "fix" all constructor calls.
    /// </summary>
    /// 
    /// <remarks>Once an instance has been created of the specified type, this ConstructorContext is 
    /// used, and cannot be used again. Each object needs its own ConstructorContext.</remarks>
    /// 
    /// <typeparam name="T">The class which is to be autoconstructed</typeparam>
    /// 
    /// <example>This is an example of how the class may be used. Given the types:
    /// <code>public interface ISomeInterface {
    ///      void SomeOperation();
    /// }
    /// 
    /// public class Class {
    ///     private readonly ISomeInterface m_i;
    /// 
    ///     public Class(ISomeInterface i) {
    ///        m_i = i;
    ///     }
    /// 
    ///     public void DoStuff() {
    ///        m_i.SomeOperation();
    ///     }
    /// } 
    /// </code>
    /// We can write tests like this:
    /// <code>var context = new ConstructorContext&lt;Class&gt;();
    /// var mock = context.Inject(new Mock&lt;ISomeInterface&gt;());
    /// 
    /// mock.Setup(m => m.SomeOperation());
    /// 
    /// var objectUnderTest = context.New();
    /// objectUnderTest.DoStuff();</code>
    /// </example>
    public class ConstructorContext<T> where T : class
    {
        private bool m_constructed;
        private readonly ParameterInfo[] m_formalParameters;
        private readonly IDictionary<string, Type> m_namedFormalParameters;
        private readonly IDictionary<string, object> m_namedActualParameters  = new Dictionary<string, object>();
        private readonly IDictionary<Type, object> m_unnamedActualParameters = new Dictionary<Type, object>();

        /// <summary>
        /// Assumes that <see cref="T"/> has one, and only one constructor.
        /// </summary>
        public ConstructorContext()
        {
            var constructors = typeof (T).GetConstructors();
            if (constructors.Length < 1)
                throw new ArgumentException($"There are no public constructors for type {typeof(T).Name}.");

            if (constructors.Length > 1)
                throw new ArgumentException($"There are more than one constructor for type {typeof (T).Name}. Use overloaded constructor to disambiguate.");

            m_formalParameters = constructors[0].GetParameters();
            m_namedFormalParameters = constructors[0].GetParameters().ToDictionary(parameter => parameter.Name, parameter => parameter.ParameterType);
        }

        /// <summary>
        /// Used to disambiguate the constructors when <see cref="T"/> has multiple constructors.
        /// </summary>
        public ConstructorContext(params Type[] constructorParameters)
        {
            var constructors = typeof (T).GetConstructors();

            foreach (var constructor in constructors)
            {
                var parameters = constructor.GetParameters();
                if (parameters.Length == constructorParameters.Length)
                {
                    var isConstructorWeWant = parameters.Zip(constructorParameters, (paramInfo, paramType) => paramInfo.ParameterType == paramType)
                                                        .Aggregate(true, (acc, el) => acc && el);

                    if (isConstructorWeWant)
                    {
                        m_formalParameters = constructor.GetParameters();
                        m_namedFormalParameters = constructor.GetParameters().ToDictionary(parameter => parameter.Name, parameter => parameter.ParameterType);
                        return;
                    }
                }
            }

            throw new ArgumentException($"No constructor of {typeof(T).Name} matches ({string.Join(", ", constructorParameters.Select(cp => cp.Name))})");
        }

        /// <summary>
        /// Injects a mock into the context. If no <see cref="parameterName"/> is specified, it will match against parameter type.
        /// </summary>
        /// <remarks>If the constructor uses the same type but for different arguments, then specify the name of the parameter which you intend to inject.</remarks>
        /// <typeparam name="TMockedType">The mocked type, which matches the formal constructor argument type</typeparam>
        /// <param name="mock">The mock to pass on to the constructor</param>
        /// <param name="parameterName">The name of the parameter</param>
        /// <returns>The mock</returns>
        public Mock<TMockedType> Inject<TMockedType>(Mock<TMockedType> mock, string parameterName = null) where TMockedType : class
        {
            CheckCompatibility(typeof (TMockedType), parameterName);

            if (parameterName == null)
                m_unnamedActualParameters.Add(typeof(TMockedType), mock.Object);
            else
                m_namedActualParameters.Add(parameterName, mock.Object);

            return mock;
        }

        /// <summary>
        /// Injects a value into the context. If no <see cref="parameterName"/> is specified, it will match against parameter type.
        /// </summary>
        /// <remarks>If the constructor uses the same type but for different arguments, then specify the name of the parameter which you intend to inject.</remarks>
        /// <typeparam name="TValueType">The value type, which matches the formal constructor argument type</typeparam>
        /// <param name="value">The value to pass on to the constructor</param>
        /// <param name="parameterName">The name of the parameter</param>
        /// <returns>The value</returns>
        public TValueType Inject<TValueType>(TValueType value, string parameterName = null)
        {
            CheckCompatibility(typeof(TValueType), parameterName);

            if (parameterName == null)
                m_unnamedActualParameters.Add(typeof(TValueType), value);
            else
                m_namedActualParameters.Add(parameterName, value);

            return value;
        }

        /// <summary>
        /// Constructs an object of type <see cref="T"/>, using previous injections (see <see cref="Inject{TMockedType}(Moq.Mock{TMockedType},string)"/> and <see cref="Inject{TValueType}(TValueType,string)"/>).
        /// </summary>
        /// <returns>The constructed object</returns>
        public T New()
        {
            if (m_constructed)
                throw new InvalidOperationException("You may only call New() once.");

            var @object = Construct();
            m_constructed = true;
            return @object;
        }

        private void CheckCompatibility(Type parameterType, string parameterName)
        {
            if (parameterName == null)
            {
                if (m_namedFormalParameters.Values.All(type => type != parameterType))
                    throw new ArgumentException($"Cannot autoinject parameter of type {parameterType.Name} as it is not used in any constructor of {typeof(T).Name}");

                if (m_unnamedActualParameters.ContainsKey(parameterType))
                    throw new ArgumentException($"Unnamed parameter of type {parameterType.Name} has already been injected.");
            }
            else
            {
                if (!m_namedFormalParameters.ContainsKey(parameterName))
                    throw new ArgumentException($"Named parameter {parameterName} is not among formal parameters of constructor.");

                if(m_namedFormalParameters[parameterName] != parameterType)
                    throw new ArgumentException($"Named parameter {parameterName} is not of type {m_namedFormalParameters[parameterName].Name}");

                if (m_namedActualParameters.ContainsKey(parameterName))
                    throw new ArgumentException($"Named parameter {parameterName} has already been injected.");
            }
        }

        private T Construct()
        {
            var args = new object[m_formalParameters.Length];
            int i = 0;
            foreach (var param in m_formalParameters)
            {
                args[i++] = GetOrAutoMockParameter(param);
            }

            return (T) Activator.CreateInstance(typeof (T), args);
        }

        private object GetOrAutoMockParameter(ParameterInfo paramInfo)
        {
            object value;
            if (m_namedActualParameters.TryGetValue(paramInfo.Name, out value))
                return value;

            if (m_unnamedActualParameters.TryGetValue(paramInfo.ParameterType, out value))
                return value;

            return CreateMock(paramInfo.ParameterType);
        }

        private object CreateMock(Type type)
        {
            var mockType = typeof (Mock<>).MakeGenericType(type);
            var mock = Activator.CreateInstance(mockType, MockBehavior.Strict);
            return mockType.GetProperties(BindingFlags.Instance | BindingFlags.Public)
                           .Single(p => p.PropertyType == type && p.Name == "Object")
                           .GetValue(mock);
        }
    }
}
