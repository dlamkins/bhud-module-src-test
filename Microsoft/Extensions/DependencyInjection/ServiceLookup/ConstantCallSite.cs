using System;

namespace Microsoft.Extensions.DependencyInjection.ServiceLookup
{
	internal sealed class ConstantCallSite : ServiceCallSite
	{
		private readonly Type _serviceType;

		internal object? DefaultValue => base.Value;

		public override Type ServiceType => _serviceType;

		public override Type ImplementationType => DefaultValue?.GetType() ?? _serviceType;

		public override CallSiteKind Kind { get; } = CallSiteKind.Constant;


		public ConstantCallSite(Type serviceType, object? defaultValue)
			: base(ResultCache.None(serviceType))
		{
			_serviceType = serviceType ?? throw new ArgumentNullException("serviceType");
			if (defaultValue != null && !serviceType.IsInstanceOfType(defaultValue))
			{
				throw new ArgumentException(_003C17812d78_002D9a9b_002D4ef4_002Db47b_002D9d33b53e23d5_003ESR.Format(_003C17812d78_002D9a9b_002D4ef4_002Db47b_002D9d33b53e23d5_003ESR.ConstantCantBeConvertedToServiceType, defaultValue!.GetType(), serviceType));
			}
			base.Value = defaultValue;
		}
	}
}
