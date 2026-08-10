using System;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Extensions.Options
{
	internal class OptionsBuilder<TOptions> where TOptions : class
	{
		private const string DefaultValidationFailureMessage = "A validation error has occurred.";

		public string Name { get; }

		public IServiceCollection Services { get; }

		public OptionsBuilder(IServiceCollection services, string? name)
		{
			_003C7f228368_002D946a_002D4fc2_002Daf41_002Da7f494284193_003EThrowHelper.ThrowIfNull(services, "services");
			Services = services;
			Name = name ?? Options.DefaultName;
		}

		public virtual OptionsBuilder<TOptions> Configure(Action<TOptions> configureOptions)
		{
			_003C7f228368_002D946a_002D4fc2_002Daf41_002Da7f494284193_003EThrowHelper.ThrowIfNull(configureOptions, "configureOptions");
			Services.AddSingleton((IConfigureOptions<TOptions>)new ConfigureNamedOptions<TOptions>(Name, configureOptions));
			return this;
		}

		public virtual OptionsBuilder<TOptions> Configure<TDep>(Action<TOptions, TDep> configureOptions) where TDep : class
		{
			Action<TOptions, TDep> configureOptions2 = configureOptions;
			_003C7f228368_002D946a_002D4fc2_002Daf41_002Da7f494284193_003EThrowHelper.ThrowIfNull(configureOptions2, "configureOptions");
			Services.AddTransient((Func<IServiceProvider, IConfigureOptions<TOptions>>)((IServiceProvider sp) => new ConfigureNamedOptions<TOptions, TDep>(Name, sp.GetRequiredService<TDep>(), configureOptions2)));
			return this;
		}

		public virtual OptionsBuilder<TOptions> Configure<TDep1, TDep2>(Action<TOptions, TDep1, TDep2> configureOptions) where TDep1 : class where TDep2 : class
		{
			Action<TOptions, TDep1, TDep2> configureOptions2 = configureOptions;
			_003C7f228368_002D946a_002D4fc2_002Daf41_002Da7f494284193_003EThrowHelper.ThrowIfNull(configureOptions2, "configureOptions");
			Services.AddTransient((Func<IServiceProvider, IConfigureOptions<TOptions>>)((IServiceProvider sp) => new ConfigureNamedOptions<TOptions, TDep1, TDep2>(Name, sp.GetRequiredService<TDep1>(), sp.GetRequiredService<TDep2>(), configureOptions2)));
			return this;
		}

		public virtual OptionsBuilder<TOptions> Configure<TDep1, TDep2, TDep3>(Action<TOptions, TDep1, TDep2, TDep3> configureOptions) where TDep1 : class where TDep2 : class where TDep3 : class
		{
			Action<TOptions, TDep1, TDep2, TDep3> configureOptions2 = configureOptions;
			_003C7f228368_002D946a_002D4fc2_002Daf41_002Da7f494284193_003EThrowHelper.ThrowIfNull(configureOptions2, "configureOptions");
			Services.AddTransient((Func<IServiceProvider, IConfigureOptions<TOptions>>)((IServiceProvider sp) => new ConfigureNamedOptions<TOptions, TDep1, TDep2, TDep3>(Name, sp.GetRequiredService<TDep1>(), sp.GetRequiredService<TDep2>(), sp.GetRequiredService<TDep3>(), configureOptions2)));
			return this;
		}

		public virtual OptionsBuilder<TOptions> Configure<TDep1, TDep2, TDep3, TDep4>(Action<TOptions, TDep1, TDep2, TDep3, TDep4> configureOptions) where TDep1 : class where TDep2 : class where TDep3 : class where TDep4 : class
		{
			Action<TOptions, TDep1, TDep2, TDep3, TDep4> configureOptions2 = configureOptions;
			_003C7f228368_002D946a_002D4fc2_002Daf41_002Da7f494284193_003EThrowHelper.ThrowIfNull(configureOptions2, "configureOptions");
			Services.AddTransient((Func<IServiceProvider, IConfigureOptions<TOptions>>)((IServiceProvider sp) => new ConfigureNamedOptions<TOptions, TDep1, TDep2, TDep3, TDep4>(Name, sp.GetRequiredService<TDep1>(), sp.GetRequiredService<TDep2>(), sp.GetRequiredService<TDep3>(), sp.GetRequiredService<TDep4>(), configureOptions2)));
			return this;
		}

		public virtual OptionsBuilder<TOptions> Configure<TDep1, TDep2, TDep3, TDep4, TDep5>(Action<TOptions, TDep1, TDep2, TDep3, TDep4, TDep5> configureOptions) where TDep1 : class where TDep2 : class where TDep3 : class where TDep4 : class where TDep5 : class
		{
			Action<TOptions, TDep1, TDep2, TDep3, TDep4, TDep5> configureOptions2 = configureOptions;
			_003C7f228368_002D946a_002D4fc2_002Daf41_002Da7f494284193_003EThrowHelper.ThrowIfNull(configureOptions2, "configureOptions");
			Services.AddTransient((Func<IServiceProvider, IConfigureOptions<TOptions>>)((IServiceProvider sp) => new ConfigureNamedOptions<TOptions, TDep1, TDep2, TDep3, TDep4, TDep5>(Name, sp.GetRequiredService<TDep1>(), sp.GetRequiredService<TDep2>(), sp.GetRequiredService<TDep3>(), sp.GetRequiredService<TDep4>(), sp.GetRequiredService<TDep5>(), configureOptions2)));
			return this;
		}

		public virtual OptionsBuilder<TOptions> PostConfigure(Action<TOptions> configureOptions)
		{
			_003C7f228368_002D946a_002D4fc2_002Daf41_002Da7f494284193_003EThrowHelper.ThrowIfNull(configureOptions, "configureOptions");
			Services.AddSingleton((IPostConfigureOptions<TOptions>)new PostConfigureOptions<TOptions>(Name, configureOptions));
			return this;
		}

		public virtual OptionsBuilder<TOptions> PostConfigure<TDep>(Action<TOptions, TDep> configureOptions) where TDep : class
		{
			Action<TOptions, TDep> configureOptions2 = configureOptions;
			_003C7f228368_002D946a_002D4fc2_002Daf41_002Da7f494284193_003EThrowHelper.ThrowIfNull(configureOptions2, "configureOptions");
			Services.AddTransient((Func<IServiceProvider, IPostConfigureOptions<TOptions>>)((IServiceProvider sp) => new PostConfigureOptions<TOptions, TDep>(Name, sp.GetRequiredService<TDep>(), configureOptions2)));
			return this;
		}

		public virtual OptionsBuilder<TOptions> PostConfigure<TDep1, TDep2>(Action<TOptions, TDep1, TDep2> configureOptions) where TDep1 : class where TDep2 : class
		{
			Action<TOptions, TDep1, TDep2> configureOptions2 = configureOptions;
			_003C7f228368_002D946a_002D4fc2_002Daf41_002Da7f494284193_003EThrowHelper.ThrowIfNull(configureOptions2, "configureOptions");
			Services.AddTransient((Func<IServiceProvider, IPostConfigureOptions<TOptions>>)((IServiceProvider sp) => new PostConfigureOptions<TOptions, TDep1, TDep2>(Name, sp.GetRequiredService<TDep1>(), sp.GetRequiredService<TDep2>(), configureOptions2)));
			return this;
		}

		public virtual OptionsBuilder<TOptions> PostConfigure<TDep1, TDep2, TDep3>(Action<TOptions, TDep1, TDep2, TDep3> configureOptions) where TDep1 : class where TDep2 : class where TDep3 : class
		{
			Action<TOptions, TDep1, TDep2, TDep3> configureOptions2 = configureOptions;
			_003C7f228368_002D946a_002D4fc2_002Daf41_002Da7f494284193_003EThrowHelper.ThrowIfNull(configureOptions2, "configureOptions");
			Services.AddTransient((Func<IServiceProvider, IPostConfigureOptions<TOptions>>)((IServiceProvider sp) => new PostConfigureOptions<TOptions, TDep1, TDep2, TDep3>(Name, sp.GetRequiredService<TDep1>(), sp.GetRequiredService<TDep2>(), sp.GetRequiredService<TDep3>(), configureOptions2)));
			return this;
		}

		public virtual OptionsBuilder<TOptions> PostConfigure<TDep1, TDep2, TDep3, TDep4>(Action<TOptions, TDep1, TDep2, TDep3, TDep4> configureOptions) where TDep1 : class where TDep2 : class where TDep3 : class where TDep4 : class
		{
			Action<TOptions, TDep1, TDep2, TDep3, TDep4> configureOptions2 = configureOptions;
			_003C7f228368_002D946a_002D4fc2_002Daf41_002Da7f494284193_003EThrowHelper.ThrowIfNull(configureOptions2, "configureOptions");
			Services.AddTransient((Func<IServiceProvider, IPostConfigureOptions<TOptions>>)((IServiceProvider sp) => new PostConfigureOptions<TOptions, TDep1, TDep2, TDep3, TDep4>(Name, sp.GetRequiredService<TDep1>(), sp.GetRequiredService<TDep2>(), sp.GetRequiredService<TDep3>(), sp.GetRequiredService<TDep4>(), configureOptions2)));
			return this;
		}

		public virtual OptionsBuilder<TOptions> PostConfigure<TDep1, TDep2, TDep3, TDep4, TDep5>(Action<TOptions, TDep1, TDep2, TDep3, TDep4, TDep5> configureOptions) where TDep1 : class where TDep2 : class where TDep3 : class where TDep4 : class where TDep5 : class
		{
			Action<TOptions, TDep1, TDep2, TDep3, TDep4, TDep5> configureOptions2 = configureOptions;
			_003C7f228368_002D946a_002D4fc2_002Daf41_002Da7f494284193_003EThrowHelper.ThrowIfNull(configureOptions2, "configureOptions");
			Services.AddTransient((Func<IServiceProvider, IPostConfigureOptions<TOptions>>)((IServiceProvider sp) => new PostConfigureOptions<TOptions, TDep1, TDep2, TDep3, TDep4, TDep5>(Name, sp.GetRequiredService<TDep1>(), sp.GetRequiredService<TDep2>(), sp.GetRequiredService<TDep3>(), sp.GetRequiredService<TDep4>(), sp.GetRequiredService<TDep5>(), configureOptions2)));
			return this;
		}

		public virtual OptionsBuilder<TOptions> Validate(Func<TOptions, bool> validation)
		{
			return Validate(validation, "A validation error has occurred.");
		}

		public virtual OptionsBuilder<TOptions> Validate(Func<TOptions, bool> validation, string failureMessage)
		{
			_003C7f228368_002D946a_002D4fc2_002Daf41_002Da7f494284193_003EThrowHelper.ThrowIfNull(validation, "validation");
			Services.AddSingleton((IValidateOptions<TOptions>)new ValidateOptions<TOptions>(Name, validation, failureMessage));
			return this;
		}

		public virtual OptionsBuilder<TOptions> Validate<TDep>(Func<TOptions, TDep, bool> validation) where TDep : notnull
		{
			return Validate(validation, "A validation error has occurred.");
		}

		public virtual OptionsBuilder<TOptions> Validate<TDep>(Func<TOptions, TDep, bool> validation, string failureMessage) where TDep : notnull
		{
			Func<TOptions, TDep, bool> validation2 = validation;
			string failureMessage2 = failureMessage;
			_003C7f228368_002D946a_002D4fc2_002Daf41_002Da7f494284193_003EThrowHelper.ThrowIfNull(validation2, "validation");
			Services.AddTransient((Func<IServiceProvider, IValidateOptions<TOptions>>)((IServiceProvider sp) => new ValidateOptions<TOptions, TDep>(Name, sp.GetRequiredService<TDep>(), validation2, failureMessage2)));
			return this;
		}

		public virtual OptionsBuilder<TOptions> Validate<TDep1, TDep2>(Func<TOptions, TDep1, TDep2, bool> validation) where TDep1 : notnull where TDep2 : notnull
		{
			return Validate(validation, "A validation error has occurred.");
		}

		public virtual OptionsBuilder<TOptions> Validate<TDep1, TDep2>(Func<TOptions, TDep1, TDep2, bool> validation, string failureMessage) where TDep1 : notnull where TDep2 : notnull
		{
			Func<TOptions, TDep1, TDep2, bool> validation2 = validation;
			string failureMessage2 = failureMessage;
			_003C7f228368_002D946a_002D4fc2_002Daf41_002Da7f494284193_003EThrowHelper.ThrowIfNull(validation2, "validation");
			Services.AddTransient((Func<IServiceProvider, IValidateOptions<TOptions>>)((IServiceProvider sp) => new ValidateOptions<TOptions, TDep1, TDep2>(Name, sp.GetRequiredService<TDep1>(), sp.GetRequiredService<TDep2>(), validation2, failureMessage2)));
			return this;
		}

		public virtual OptionsBuilder<TOptions> Validate<TDep1, TDep2, TDep3>(Func<TOptions, TDep1, TDep2, TDep3, bool> validation) where TDep1 : notnull where TDep2 : notnull where TDep3 : notnull
		{
			return Validate(validation, "A validation error has occurred.");
		}

		public virtual OptionsBuilder<TOptions> Validate<TDep1, TDep2, TDep3>(Func<TOptions, TDep1, TDep2, TDep3, bool> validation, string failureMessage) where TDep1 : notnull where TDep2 : notnull where TDep3 : notnull
		{
			Func<TOptions, TDep1, TDep2, TDep3, bool> validation2 = validation;
			string failureMessage2 = failureMessage;
			_003C7f228368_002D946a_002D4fc2_002Daf41_002Da7f494284193_003EThrowHelper.ThrowIfNull(validation2, "validation");
			Services.AddTransient((Func<IServiceProvider, IValidateOptions<TOptions>>)((IServiceProvider sp) => new ValidateOptions<TOptions, TDep1, TDep2, TDep3>(Name, sp.GetRequiredService<TDep1>(), sp.GetRequiredService<TDep2>(), sp.GetRequiredService<TDep3>(), validation2, failureMessage2)));
			return this;
		}

		public virtual OptionsBuilder<TOptions> Validate<TDep1, TDep2, TDep3, TDep4>(Func<TOptions, TDep1, TDep2, TDep3, TDep4, bool> validation) where TDep1 : notnull where TDep2 : notnull where TDep3 : notnull where TDep4 : notnull
		{
			return Validate(validation, "A validation error has occurred.");
		}

		public virtual OptionsBuilder<TOptions> Validate<TDep1, TDep2, TDep3, TDep4>(Func<TOptions, TDep1, TDep2, TDep3, TDep4, bool> validation, string failureMessage) where TDep1 : notnull where TDep2 : notnull where TDep3 : notnull where TDep4 : notnull
		{
			Func<TOptions, TDep1, TDep2, TDep3, TDep4, bool> validation2 = validation;
			string failureMessage2 = failureMessage;
			_003C7f228368_002D946a_002D4fc2_002Daf41_002Da7f494284193_003EThrowHelper.ThrowIfNull(validation2, "validation");
			Services.AddTransient((Func<IServiceProvider, IValidateOptions<TOptions>>)((IServiceProvider sp) => new ValidateOptions<TOptions, TDep1, TDep2, TDep3, TDep4>(Name, sp.GetRequiredService<TDep1>(), sp.GetRequiredService<TDep2>(), sp.GetRequiredService<TDep3>(), sp.GetRequiredService<TDep4>(), validation2, failureMessage2)));
			return this;
		}

		public virtual OptionsBuilder<TOptions> Validate<TDep1, TDep2, TDep3, TDep4, TDep5>(Func<TOptions, TDep1, TDep2, TDep3, TDep4, TDep5, bool> validation) where TDep1 : notnull where TDep2 : notnull where TDep3 : notnull where TDep4 : notnull where TDep5 : notnull
		{
			return Validate(validation, "A validation error has occurred.");
		}

		public virtual OptionsBuilder<TOptions> Validate<TDep1, TDep2, TDep3, TDep4, TDep5>(Func<TOptions, TDep1, TDep2, TDep3, TDep4, TDep5, bool> validation, string failureMessage) where TDep1 : notnull where TDep2 : notnull where TDep3 : notnull where TDep4 : notnull where TDep5 : notnull
		{
			Func<TOptions, TDep1, TDep2, TDep3, TDep4, TDep5, bool> validation2 = validation;
			string failureMessage2 = failureMessage;
			_003C7f228368_002D946a_002D4fc2_002Daf41_002Da7f494284193_003EThrowHelper.ThrowIfNull(validation2, "validation");
			Services.AddTransient((Func<IServiceProvider, IValidateOptions<TOptions>>)((IServiceProvider sp) => new ValidateOptions<TOptions, TDep1, TDep2, TDep3, TDep4, TDep5>(Name, sp.GetRequiredService<TDep1>(), sp.GetRequiredService<TDep2>(), sp.GetRequiredService<TDep3>(), sp.GetRequiredService<TDep4>(), sp.GetRequiredService<TDep5>(), validation2, failureMessage2)));
			return this;
		}
	}
}
