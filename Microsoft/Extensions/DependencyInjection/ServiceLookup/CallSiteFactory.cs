using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.Extensions.Internal;

namespace Microsoft.Extensions.DependencyInjection.ServiceLookup
{
	internal sealed class CallSiteFactory : IServiceProviderIsService, IServiceProviderIsKeyedService
	{
		private struct ServiceDescriptorCacheItem
		{
			[_003C17812d78_002D9a9b_002D4ef4_002Db47b_002D9d33b53e23d5_003EDisallowNull]
			private ServiceDescriptor _item;

			[_003C17812d78_002D9a9b_002D4ef4_002Db47b_002D9d33b53e23d5_003EDisallowNull]
			private List<ServiceDescriptor> _items;

			public ServiceDescriptor Last
			{
				get
				{
					if (_items != null && _items.Count > 0)
					{
						return _items[_items.Count - 1];
					}
					return _item;
				}
			}

			public int Count
			{
				get
				{
					if (_item == null)
					{
						return 0;
					}
					return 1 + (_items?.Count ?? 0);
				}
			}

			public ServiceDescriptor this[int index]
			{
				get
				{
					if (index >= Count)
					{
						throw new ArgumentOutOfRangeException("index");
					}
					if (index == 0)
					{
						return _item;
					}
					return _items[index - 1];
				}
			}

			public int GetSlot(ServiceDescriptor descriptor)
			{
				if (descriptor == _item)
				{
					return Count - 1;
				}
				if (_items != null)
				{
					int num = _items.IndexOf(descriptor);
					if (num != -1)
					{
						return _items.Count - (num + 1);
					}
				}
				throw new InvalidOperationException(_003C17812d78_002D9a9b_002D4ef4_002Db47b_002D9d33b53e23d5_003ESR.ServiceDescriptorNotExist);
			}

			public ServiceDescriptorCacheItem Add(ServiceDescriptor descriptor)
			{
				ServiceDescriptorCacheItem result = default(ServiceDescriptorCacheItem);
				if (_item == null)
				{
					result._item = descriptor;
				}
				else
				{
					result._item = _item;
					result._items = _items ?? new List<ServiceDescriptor>();
					result._items.Add(descriptor);
				}
				return result;
			}
		}

		private const int DefaultSlot = 0;

		private readonly ServiceDescriptor[] _descriptors;

		private readonly ConcurrentDictionary<ServiceCacheKey, ServiceCallSite> _callSiteCache = new ConcurrentDictionary<ServiceCacheKey, ServiceCallSite>();

		private readonly Dictionary<ServiceIdentifier, ServiceDescriptorCacheItem> _descriptorLookup = new Dictionary<ServiceIdentifier, ServiceDescriptorCacheItem>();

		private readonly ConcurrentDictionary<ServiceIdentifier, object> _callSiteLocks = new ConcurrentDictionary<ServiceIdentifier, object>();

		private readonly StackGuard _stackGuard;

		internal ServiceDescriptor[] Descriptors => _descriptors;

		public CallSiteFactory(ICollection<ServiceDescriptor> descriptors)
		{
			_stackGuard = new StackGuard();
			_descriptors = new ServiceDescriptor[descriptors.Count];
			descriptors.CopyTo(_descriptors, 0);
			Populate();
		}

		private void Populate()
		{
			ServiceDescriptor[] descriptors = _descriptors;
			foreach (ServiceDescriptor serviceDescriptor in descriptors)
			{
				Type serviceType = serviceDescriptor.ServiceType;
				Type type;
				if (serviceType.IsGenericTypeDefinition)
				{
					Type implementationType = ServiceDescriptorExtensions.GetImplementationType(serviceDescriptor);
					if (implementationType == null || !implementationType.IsGenericTypeDefinition)
					{
						throw new ArgumentException(_003C17812d78_002D9a9b_002D4ef4_002Db47b_002D9d33b53e23d5_003ESR.Format(_003C17812d78_002D9a9b_002D4ef4_002Db47b_002D9d33b53e23d5_003ESR.OpenGenericServiceRequiresOpenGenericImplementation, serviceType), "descriptors");
					}
					if (implementationType.IsAbstract || implementationType.IsInterface)
					{
						throw new ArgumentException(_003C17812d78_002D9a9b_002D4ef4_002Db47b_002D9d33b53e23d5_003ESR.Format(_003C17812d78_002D9a9b_002D4ef4_002Db47b_002D9d33b53e23d5_003ESR.TypeCannotBeActivated, implementationType, serviceType));
					}
					Type[] genericArguments = serviceType.GetGenericArguments();
					Type[] genericArguments2 = implementationType.GetGenericArguments();
					if (genericArguments.Length != genericArguments2.Length)
					{
						throw new ArgumentException(_003C17812d78_002D9a9b_002D4ef4_002Db47b_002D9d33b53e23d5_003ESR.Format(_003C17812d78_002D9a9b_002D4ef4_002Db47b_002D9d33b53e23d5_003ESR.ArityOfOpenGenericServiceNotEqualArityOfOpenGenericImplementation, serviceType, implementationType), "descriptors");
					}
					if (ServiceProvider.VerifyOpenGenericServiceTrimmability)
					{
						ValidateTrimmingAnnotations(serviceType, genericArguments, implementationType, genericArguments2);
					}
				}
				else if (serviceDescriptor.TryGetImplementationType(out type) && (type.IsGenericTypeDefinition || type.IsAbstract || type.IsInterface))
				{
					throw new ArgumentException(_003C17812d78_002D9a9b_002D4ef4_002Db47b_002D9d33b53e23d5_003ESR.Format(_003C17812d78_002D9a9b_002D4ef4_002Db47b_002D9d33b53e23d5_003ESR.TypeCannotBeActivated, type, serviceType));
				}
				ServiceIdentifier key = ServiceIdentifier.FromDescriptor(serviceDescriptor);
				_descriptorLookup.TryGetValue(key, out var value);
				_descriptorLookup[key] = value.Add(serviceDescriptor);
			}
		}

		private static void ValidateTrimmingAnnotations(Type serviceType, Type[] serviceTypeGenericArguments, Type implementationType, Type[] implementationTypeGenericArguments)
		{
			for (int i = 0; i < serviceTypeGenericArguments.Length; i++)
			{
				Type type = serviceTypeGenericArguments[i];
				Type type2 = implementationTypeGenericArguments[i];
				_003C17812d78_002D9a9b_002D4ef4_002Db47b_002D9d33b53e23d5_003EDynamicallyAccessedMemberTypes dynamicallyAccessedMemberTypes = GetDynamicallyAccessedMemberTypes(type);
				_003C17812d78_002D9a9b_002D4ef4_002Db47b_002D9d33b53e23d5_003EDynamicallyAccessedMemberTypes dynamicallyAccessedMemberTypes2 = GetDynamicallyAccessedMemberTypes(type2);
				if (!AreCompatible(dynamicallyAccessedMemberTypes, dynamicallyAccessedMemberTypes2))
				{
					throw new ArgumentException(_003C17812d78_002D9a9b_002D4ef4_002Db47b_002D9d33b53e23d5_003ESR.Format(_003C17812d78_002D9a9b_002D4ef4_002Db47b_002D9d33b53e23d5_003ESR.TrimmingAnnotationsDoNotMatch, implementationType.FullName, serviceType.FullName));
				}
				bool flag = type.GenericParameterAttributes.HasFlag(GenericParameterAttributes.DefaultConstructorConstraint);
				if (type2.GenericParameterAttributes.HasFlag(GenericParameterAttributes.DefaultConstructorConstraint) && !flag)
				{
					throw new ArgumentException(_003C17812d78_002D9a9b_002D4ef4_002Db47b_002D9d33b53e23d5_003ESR.Format(_003C17812d78_002D9a9b_002D4ef4_002Db47b_002D9d33b53e23d5_003ESR.TrimmingAnnotationsDoNotMatch_NewConstraint, implementationType.FullName, serviceType.FullName));
				}
			}
		}

		private static _003C17812d78_002D9a9b_002D4ef4_002Db47b_002D9d33b53e23d5_003EDynamicallyAccessedMemberTypes GetDynamicallyAccessedMemberTypes(Type serviceGenericType)
		{
			foreach (CustomAttributeData customAttributesDatum in serviceGenericType.GetCustomAttributesData())
			{
				if (customAttributesDatum.AttributeType.FullName == "System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembersAttribute" && customAttributesDatum.ConstructorArguments.Count == 1 && customAttributesDatum.ConstructorArguments[0].ArgumentType.FullName == "System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes")
				{
					return (_003C17812d78_002D9a9b_002D4ef4_002Db47b_002D9d33b53e23d5_003EDynamicallyAccessedMemberTypes)(int)customAttributesDatum.ConstructorArguments[0].Value;
				}
			}
			return _003C17812d78_002D9a9b_002D4ef4_002Db47b_002D9d33b53e23d5_003EDynamicallyAccessedMemberTypes.None;
		}

		private static bool AreCompatible(_003C17812d78_002D9a9b_002D4ef4_002Db47b_002D9d33b53e23d5_003EDynamicallyAccessedMemberTypes serviceDynamicallyAccessedMembers, _003C17812d78_002D9a9b_002D4ef4_002Db47b_002D9d33b53e23d5_003EDynamicallyAccessedMemberTypes implementationDynamicallyAccessedMembers)
		{
			return serviceDynamicallyAccessedMembers.HasFlag(implementationDynamicallyAccessedMembers);
		}

		internal int? GetSlot(ServiceDescriptor serviceDescriptor)
		{
			if (_descriptorLookup.TryGetValue(ServiceIdentifier.FromDescriptor(serviceDescriptor), out var value))
			{
				return value.GetSlot(serviceDescriptor);
			}
			return null;
		}

		internal ServiceCallSite? GetCallSite(ServiceIdentifier serviceIdentifier, CallSiteChain callSiteChain)
		{
			if (!_callSiteCache.TryGetValue(new ServiceCacheKey(serviceIdentifier, 0), out var value))
			{
				return CreateCallSite(serviceIdentifier, callSiteChain);
			}
			return value;
		}

		internal ServiceCallSite? GetCallSite(ServiceDescriptor serviceDescriptor, CallSiteChain callSiteChain)
		{
			ServiceIdentifier serviceIdentifier = ServiceIdentifier.FromDescriptor(serviceDescriptor);
			if (_descriptorLookup.TryGetValue(serviceIdentifier, out var value))
			{
				return TryCreateExact(serviceDescriptor, serviceIdentifier, callSiteChain, value.GetSlot(serviceDescriptor));
			}
			return null;
		}

		private ServiceCallSite CreateCallSite(ServiceIdentifier serviceIdentifier, CallSiteChain callSiteChain)
		{
			if (!_stackGuard.TryEnterOnCurrentStack())
			{
				return _stackGuard.RunOnEmptyStack(new Func<ServiceIdentifier, CallSiteChain, ServiceCallSite>(CreateCallSite), serviceIdentifier, callSiteChain);
			}
			object orAdd = _callSiteLocks.GetOrAdd(serviceIdentifier, (ServiceIdentifier _) => new object());
			lock (orAdd)
			{
				callSiteChain.CheckCircularDependency(serviceIdentifier);
				return TryCreateExact(serviceIdentifier, callSiteChain) ?? TryCreateOpenGeneric(serviceIdentifier, callSiteChain) ?? TryCreateEnumerable(serviceIdentifier, callSiteChain);
			}
		}

		private ServiceCallSite TryCreateExact(ServiceIdentifier serviceIdentifier, CallSiteChain callSiteChain)
		{
			if (_descriptorLookup.TryGetValue(serviceIdentifier, out var value))
			{
				return TryCreateExact(value.Last, serviceIdentifier, callSiteChain, 0);
			}
			if (serviceIdentifier.ServiceKey != null)
			{
				ServiceIdentifier key = new ServiceIdentifier(KeyedService.AnyKey, serviceIdentifier.ServiceType);
				if (_descriptorLookup.TryGetValue(key, out value))
				{
					return TryCreateExact(value.Last, serviceIdentifier, callSiteChain, 0);
				}
			}
			return null;
		}

		private ServiceCallSite TryCreateOpenGeneric(ServiceIdentifier serviceIdentifier, CallSiteChain callSiteChain)
		{
			if (serviceIdentifier.IsConstructedGenericType)
			{
				ServiceIdentifier genericTypeDefinition = serviceIdentifier.GetGenericTypeDefinition();
				if (_descriptorLookup.TryGetValue(genericTypeDefinition, out var value))
				{
					return TryCreateOpenGeneric(value.Last, serviceIdentifier, callSiteChain, 0, throwOnConstraintViolation: true);
				}
				if (serviceIdentifier.ServiceKey != null)
				{
					ServiceIdentifier key = new ServiceIdentifier(KeyedService.AnyKey, genericTypeDefinition.ServiceType);
					if (_descriptorLookup.TryGetValue(key, out value))
					{
						return TryCreateOpenGeneric(value.Last, serviceIdentifier, callSiteChain, 0, throwOnConstraintViolation: true);
					}
				}
			}
			return null;
		}

		private ServiceCallSite TryCreateEnumerable(ServiceIdentifier serviceIdentifier, CallSiteChain callSiteChain)
		{
			ServiceCacheKey serviceCacheKey = new ServiceCacheKey(serviceIdentifier, 0);
			if (_callSiteCache.TryGetValue(serviceCacheKey, out var value))
			{
				return value;
			}
			try
			{
				callSiteChain.Add(serviceIdentifier);
				Type serviceType = serviceIdentifier.ServiceType;
				if (!serviceType.IsConstructedGenericType || serviceType.GetGenericTypeDefinition() != typeof(IEnumerable<>))
				{
					return null;
				}
				Type type = serviceType.GenericTypeArguments[0];
				ServiceIdentifier serviceIdentifier2 = new ServiceIdentifier(serviceIdentifier.ServiceKey, type);
				if (ServiceProvider.VerifyAotCompatibility && type.IsValueType)
				{
					throw new InvalidOperationException(_003C17812d78_002D9a9b_002D4ef4_002Db47b_002D9d33b53e23d5_003ESR.Format(_003C17812d78_002D9a9b_002D4ef4_002Db47b_002D9d33b53e23d5_003ESR.AotCannotCreateEnumerableValueType, type));
				}
				CallSiteResultCacheLocation cacheLocation = CallSiteResultCacheLocation.Root;
				ServiceCallSite[] array;
				List<KeyValuePair<int, ServiceCallSite>> callSitesByIndex;
				int slot;
				if (!type.IsConstructedGenericType && _descriptorLookup.TryGetValue(serviceIdentifier2, out var value2))
				{
					array = new ServiceCallSite[value2.Count];
					for (int i = 0; i < value2.Count; i++)
					{
						ServiceDescriptor descriptor = value2[i];
						int slot2 = value2.Count - i - 1;
						ServiceCallSite serviceCallSite = TryCreateExact(descriptor, serviceIdentifier2, callSiteChain, slot2);
						cacheLocation = GetCommonCacheLocation(cacheLocation, serviceCallSite.Cache.Location);
						array[i] = serviceCallSite;
					}
				}
				else
				{
					callSitesByIndex = new List<KeyValuePair<int, ServiceCallSite>>();
					slot = 0;
					for (int num = _descriptors.Length - 1; num >= 0; num--)
					{
						if (KeysMatch(_descriptors[num].ServiceKey, serviceIdentifier2.ServiceKey))
						{
							ServiceCallSite serviceCallSite2 = TryCreateExact(_descriptors[num], serviceIdentifier2, callSiteChain, slot);
							if (serviceCallSite2 != null)
							{
								AddCallSite(serviceCallSite2, num);
							}
						}
					}
					for (int num2 = _descriptors.Length - 1; num2 >= 0; num2--)
					{
						if (KeysMatch(_descriptors[num2].ServiceKey, serviceIdentifier2.ServiceKey))
						{
							ServiceCallSite serviceCallSite3 = TryCreateOpenGeneric(_descriptors[num2], serviceIdentifier2, callSiteChain, slot, throwOnConstraintViolation: false);
							if (serviceCallSite3 != null)
							{
								AddCallSite(serviceCallSite3, num2);
							}
						}
					}
					callSitesByIndex.Sort((KeyValuePair<int, ServiceCallSite> a, KeyValuePair<int, ServiceCallSite> b) => a.Key.CompareTo(b.Key));
					array = new ServiceCallSite[callSitesByIndex.Count];
					for (int j = 0; j < array.Length; j++)
					{
						array[j] = callSitesByIndex[j].Value;
					}
				}
				ResultCache cache = ((cacheLocation == CallSiteResultCacheLocation.Scope || cacheLocation == CallSiteResultCacheLocation.Root) ? new ResultCache(cacheLocation, serviceCacheKey) : new ResultCache(CallSiteResultCacheLocation.None, serviceCacheKey));
				return _callSiteCache[serviceCacheKey] = new IEnumerableCallSite(cache, type, array);
				void AddCallSite(ServiceCallSite callSite, int index)
				{
					slot++;
					cacheLocation = GetCommonCacheLocation(cacheLocation, callSite.Cache.Location);
					callSitesByIndex.Add(new KeyValuePair<int, ServiceCallSite>(index, callSite));
				}
			}
			finally
			{
				callSiteChain.Remove(serviceIdentifier);
			}
		}

		private static CallSiteResultCacheLocation GetCommonCacheLocation(CallSiteResultCacheLocation locationA, CallSiteResultCacheLocation locationB)
		{
			return (CallSiteResultCacheLocation)Math.Max((int)locationA, (int)locationB);
		}

		private ServiceCallSite TryCreateExact(ServiceDescriptor descriptor, ServiceIdentifier serviceIdentifier, CallSiteChain callSiteChain, int slot)
		{
			if (serviceIdentifier.ServiceType == descriptor.ServiceType)
			{
				ServiceCacheKey key = new ServiceCacheKey(serviceIdentifier, slot);
				if (_callSiteCache.TryGetValue(key, out var value))
				{
					return value;
				}
				ResultCache resultCache = new ResultCache(descriptor.Lifetime, serviceIdentifier, slot);
				ServiceCallSite serviceCallSite;
				if (descriptor.HasImplementationInstance())
				{
					serviceCallSite = new ConstantCallSite(descriptor.ServiceType, descriptor.GetImplementationInstance());
				}
				else if (!descriptor.IsKeyedService && descriptor.ImplementationFactory != null)
				{
					serviceCallSite = new FactoryCallSite(resultCache, descriptor.ServiceType, descriptor.ImplementationFactory);
				}
				else if (descriptor.IsKeyedService && descriptor.KeyedImplementationFactory != null)
				{
					serviceCallSite = new FactoryCallSite(resultCache, descriptor.ServiceType, serviceIdentifier.ServiceKey, descriptor.KeyedImplementationFactory);
				}
				else
				{
					if (!descriptor.HasImplementationType())
					{
						throw new InvalidOperationException(_003C17812d78_002D9a9b_002D4ef4_002Db47b_002D9d33b53e23d5_003ESR.InvalidServiceDescriptor);
					}
					serviceCallSite = CreateConstructorCallSite(resultCache, serviceIdentifier, ServiceDescriptorExtensions.GetImplementationType(descriptor), callSiteChain);
				}
				serviceCallSite.Key = descriptor.ServiceKey;
				return _callSiteCache[key] = serviceCallSite;
			}
			return null;
		}

		[_003C17812d78_002D9a9b_002D4ef4_002Db47b_002D9d33b53e23d5_003EUnconditionalSuppressMessage("ReflectionAnalysis", "IL2055:MakeGenericType", Justification = "MakeGenericType here is used to create a closed generic implementation type given the closed service type. Trimming annotations on the generic types are verified when 'Microsoft.Extensions.DependencyInjection.VerifyOpenGenericServiceTrimmability' is set, which is set by default when PublishTrimmed=true. That check informs developers when these generic types don't have compatible trimming annotations.")]
		[_003C17812d78_002D9a9b_002D4ef4_002Db47b_002D9d33b53e23d5_003EUnconditionalSuppressMessage("AotAnalysis", "IL3050:RequiresDynamicCode", Justification = "When ServiceProvider.VerifyAotCompatibility is true, which it is by default when PublishAot=true, this method ensures the generic types being created aren't using ValueTypes.")]
		private ServiceCallSite TryCreateOpenGeneric(ServiceDescriptor descriptor, ServiceIdentifier serviceIdentifier, CallSiteChain callSiteChain, int slot, bool throwOnConstraintViolation)
		{
			if (serviceIdentifier.IsConstructedGenericType && serviceIdentifier.ServiceType.GetGenericTypeDefinition() == descriptor.ServiceType)
			{
				ServiceCacheKey key = new ServiceCacheKey(serviceIdentifier, slot);
				if (_callSiteCache.TryGetValue(key, out var value))
				{
					return value;
				}
				Type implementationType = ServiceDescriptorExtensions.GetImplementationType(descriptor);
				ResultCache lifetime = new ResultCache(descriptor.Lifetime, serviceIdentifier, slot);
				Type implementationType2;
				try
				{
					Type[] genericTypeArguments = serviceIdentifier.ServiceType.GenericTypeArguments;
					if (ServiceProvider.VerifyAotCompatibility)
					{
						VerifyOpenGenericAotCompatibility(serviceIdentifier.ServiceType, genericTypeArguments);
					}
					implementationType2 = implementationType.MakeGenericType(genericTypeArguments);
				}
				catch (ArgumentException)
				{
					if (throwOnConstraintViolation)
					{
						throw;
					}
					return null;
				}
				return _callSiteCache[key] = CreateConstructorCallSite(lifetime, serviceIdentifier, implementationType2, callSiteChain);
			}
			return null;
		}

		private ConstructorCallSite CreateConstructorCallSite(ResultCache lifetime, ServiceIdentifier serviceIdentifier, [_003C17812d78_002D9a9b_002D4ef4_002Db47b_002D9d33b53e23d5_003EDynamicallyAccessedMembers(_003C17812d78_002D9a9b_002D4ef4_002Db47b_002D9d33b53e23d5_003EDynamicallyAccessedMemberTypes.PublicConstructors)] Type implementationType, CallSiteChain callSiteChain)
		{
			try
			{
				callSiteChain.Add(serviceIdentifier, implementationType);
				ConstructorInfo[] constructors = implementationType.GetConstructors();
				ServiceCallSite[] parameterCallSites = null;
				if (constructors.Length == 0)
				{
					throw new InvalidOperationException(_003C17812d78_002D9a9b_002D4ef4_002Db47b_002D9d33b53e23d5_003ESR.Format(_003C17812d78_002D9a9b_002D4ef4_002Db47b_002D9d33b53e23d5_003ESR.NoConstructorMatch, implementationType));
				}
				if (constructors.Length == 1)
				{
					ConstructorInfo constructorInfo = constructors[0];
					ParameterInfo[] parameters = constructorInfo.GetParameters();
					if (parameters.Length == 0)
					{
						return new ConstructorCallSite(lifetime, serviceIdentifier.ServiceType, constructorInfo);
					}
					parameterCallSites = CreateArgumentCallSites(serviceIdentifier, implementationType, callSiteChain, parameters, throwIfCallSiteNotFound: true);
					return new ConstructorCallSite(lifetime, serviceIdentifier.ServiceType, constructorInfo, parameterCallSites);
				}
				Array.Sort(constructors, (ConstructorInfo a, ConstructorInfo b) => b.GetParameters().Length.CompareTo(a.GetParameters().Length));
				ConstructorInfo constructorInfo2 = null;
				HashSet<Type> hashSet = null;
				for (int i = 0; i < constructors.Length; i++)
				{
					ParameterInfo[] parameters2 = constructors[i].GetParameters();
					ServiceCallSite[] array = CreateArgumentCallSites(serviceIdentifier, implementationType, callSiteChain, parameters2, throwIfCallSiteNotFound: false);
					if (array == null)
					{
						continue;
					}
					if (constructorInfo2 == null)
					{
						constructorInfo2 = constructors[i];
						parameterCallSites = array;
						continue;
					}
					if (hashSet == null)
					{
						hashSet = new HashSet<Type>();
						ParameterInfo[] parameters3 = constructorInfo2.GetParameters();
						foreach (ParameterInfo parameterInfo in parameters3)
						{
							hashSet.Add(parameterInfo.ParameterType);
						}
					}
					ParameterInfo[] array2 = parameters2;
					foreach (ParameterInfo parameterInfo2 in array2)
					{
						if (!hashSet.Contains(parameterInfo2.ParameterType))
						{
							throw new InvalidOperationException(string.Join(Environment.NewLine, _003C17812d78_002D9a9b_002D4ef4_002Db47b_002D9d33b53e23d5_003ESR.Format(_003C17812d78_002D9a9b_002D4ef4_002Db47b_002D9d33b53e23d5_003ESR.AmbiguousConstructorException, implementationType), constructorInfo2, constructors[i]));
						}
					}
				}
				if (constructorInfo2 == null)
				{
					throw new InvalidOperationException(_003C17812d78_002D9a9b_002D4ef4_002Db47b_002D9d33b53e23d5_003ESR.Format(_003C17812d78_002D9a9b_002D4ef4_002Db47b_002D9d33b53e23d5_003ESR.UnableToActivateTypeException, implementationType));
				}
				return new ConstructorCallSite(lifetime, serviceIdentifier.ServiceType, constructorInfo2, parameterCallSites);
			}
			finally
			{
				callSiteChain.Remove(serviceIdentifier);
			}
		}

		private ServiceCallSite[] CreateArgumentCallSites(ServiceIdentifier serviceIdentifier, Type implementationType, CallSiteChain callSiteChain, ParameterInfo[] parameters, bool throwIfCallSiteNotFound)
		{
			ServiceCallSite[] array = new ServiceCallSite[parameters.Length];
			for (int i = 0; i < parameters.Length; i++)
			{
				ServiceCallSite serviceCallSite = null;
				bool flag = false;
				Type parameterType = parameters[i].ParameterType;
				object[] customAttributes = parameters[i].GetCustomAttributes(inherit: true);
				foreach (object obj in customAttributes)
				{
					if (serviceIdentifier.ServiceKey != null && obj is ServiceKeyAttribute)
					{
						if (parameterType != serviceIdentifier.ServiceKey!.GetType())
						{
							throw new InvalidOperationException(_003C17812d78_002D9a9b_002D4ef4_002Db47b_002D9d33b53e23d5_003ESR.InvalidServiceKeyType);
						}
						serviceCallSite = new ConstantCallSite(parameterType, serviceIdentifier.ServiceKey);
						break;
					}
					FromKeyedServicesAttribute fromKeyedServicesAttribute = obj as FromKeyedServicesAttribute;
					if (fromKeyedServicesAttribute != null)
					{
						ServiceIdentifier serviceIdentifier2 = new ServiceIdentifier(fromKeyedServicesAttribute.Key, parameterType);
						serviceCallSite = GetCallSite(serviceIdentifier2, callSiteChain);
						flag = true;
						break;
					}
				}
				if ((!flag || ServiceProvider.s_allowNonKeyedServiceInject) && serviceCallSite == null)
				{
					serviceCallSite = GetCallSite(ServiceIdentifier.FromServiceType(parameterType), callSiteChain);
				}
				if (serviceCallSite == null && _003C17812d78_002D9a9b_002D4ef4_002Db47b_002D9d33b53e23d5_003EParameterDefaultValue.TryGetDefaultValue(parameters[i], out var defaultValue))
				{
					serviceCallSite = new ConstantCallSite(parameterType, defaultValue);
				}
				if (serviceCallSite == null)
				{
					if (throwIfCallSiteNotFound)
					{
						throw new InvalidOperationException(_003C17812d78_002D9a9b_002D4ef4_002Db47b_002D9d33b53e23d5_003ESR.Format(_003C17812d78_002D9a9b_002D4ef4_002Db47b_002D9d33b53e23d5_003ESR.CannotResolveService, parameterType, implementationType));
					}
					return null;
				}
				array[i] = serviceCallSite;
			}
			return array;
		}

		private static void VerifyOpenGenericAotCompatibility(Type serviceType, Type[] genericTypeArguments)
		{
			foreach (Type type in genericTypeArguments)
			{
				if (type.IsValueType)
				{
					throw new InvalidOperationException(_003C17812d78_002D9a9b_002D4ef4_002Db47b_002D9d33b53e23d5_003ESR.Format(_003C17812d78_002D9a9b_002D4ef4_002Db47b_002D9d33b53e23d5_003ESR.AotCannotCreateGenericValueType, serviceType, type));
				}
			}
		}

		public void Add(ServiceIdentifier serviceIdentifier, ServiceCallSite serviceCallSite)
		{
			_callSiteCache[new ServiceCacheKey(serviceIdentifier, 0)] = serviceCallSite;
		}

		public bool IsService(Type serviceType)
		{
			return IsService(new ServiceIdentifier(null, serviceType));
		}

		public bool IsKeyedService(Type serviceType, object? key)
		{
			return IsService(new ServiceIdentifier(key, serviceType));
		}

		internal bool IsService(ServiceIdentifier serviceIdentifier)
		{
			Type serviceType = serviceIdentifier.ServiceType;
			if ((object)serviceType == null)
			{
				throw new ArgumentNullException("serviceType");
			}
			if (serviceType.IsGenericTypeDefinition)
			{
				return false;
			}
			if (_descriptorLookup.ContainsKey(serviceIdentifier))
			{
				return true;
			}
			if (serviceIdentifier.ServiceKey != null && _descriptorLookup.ContainsKey(new ServiceIdentifier(KeyedService.AnyKey, serviceType)))
			{
				return true;
			}
			if (serviceType.IsConstructedGenericType)
			{
				Type genericTypeDefinition = serviceType.GetGenericTypeDefinition();
				if ((object)genericTypeDefinition != null)
				{
					if (!(genericTypeDefinition == typeof(IEnumerable<>)))
					{
						return _descriptorLookup.ContainsKey(serviceIdentifier.GetGenericTypeDefinition());
					}
					return true;
				}
			}
			if (!(serviceType == typeof(IServiceProvider)) && !(serviceType == typeof(IServiceScopeFactory)) && !(serviceType == typeof(IServiceProviderIsService)))
			{
				return serviceType == typeof(IServiceProviderIsKeyedService);
			}
			return true;
		}

		private static bool KeysMatch(object key1, object key2)
		{
			if (key1 == null && key2 == null)
			{
				return true;
			}
			if (key1 != null && key2 != null)
			{
				if (!key1.Equals(KeyedService.AnyKey))
				{
					return key1.Equals(key2);
				}
				return true;
			}
			return false;
		}
	}
}
